using Microsoft.Extensions.Logging;
using Common.Core.Domain;
using Common.Core.Interfaces;
using Common.Core.Validation;
using System.Net;
using System.Text;

namespace Common.Core.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly FileStorageSettings _fileStorageSettings;
        private readonly IReadOnlyDictionary<string, IFileContentValidator> _fileContentValidators;
        private readonly ILogger<FileValidator> _logger;

        public static ICollection<string> NonSigExtensions = new List<string>() { "txt", "csv", "prn" };

        public FileValidator(
            FileStorageSettings fileStorageSettings,
            IReadOnlyDictionary<string, IFileContentValidator> fileContentValidators,
            ILogger<FileValidator> logger)
        {
            _fileStorageSettings = fileStorageSettings;
            _fileContentValidators = fileContentValidators ?? new Dictionary<string, IFileContentValidator>();
            _logger = logger;
        }

        private void ThrowError(string message, string? fileName = null, string? reason = null)
        {
            ThrowError(new ValidationRule(message), fileName, reason);
        }

        private void ThrowError(ValidationRule validationRule, string? fileName = null, string? reason = null)
        {
            Guard.IsNotNull(validationRule, nameof(validationRule));

            var logMessageBuilder = new StringBuilder("File failed validation process.");

            if (!string.IsNullOrWhiteSpace(fileName))
                logMessageBuilder.Append($" Filename: {fileName}");

            logMessageBuilder.Append($" {validationRule.ToString()?.TrimEnd(".")}.");

            if (!string.IsNullOrWhiteSpace(reason))
                logMessageBuilder.Append($" Reason: {reason.TrimEnd(".")}.");

            _logger.LogWarning(logMessageBuilder.ToString().Sanitize());

            throw new ValidationException(validationRule);
        }

        public virtual string GenerateSafeFileName(string unsafeFileName)
        {
            if (string.IsNullOrWhiteSpace(unsafeFileName))
                ThrowError("Invalid or empty filename.");

            string extension = Path.GetExtension(unsafeFileName);
            if (string.IsNullOrWhiteSpace(extension))
                ThrowError("Extension invalid.", unsafeFileName);

            return Path.ChangeExtension(string.Concat(Guid.NewGuid().ToString().Replace("-", ""), ".tmp"), extension);
        }

        public virtual string SanitizeFileName(string unsafeFileName, bool makeUnique = true)
        {
            if (string.IsNullOrWhiteSpace(unsafeFileName))
                ThrowError("Invalid or empty filename.");

            string fileName = unsafeFileName.Trim();

            if (PathHelper.IsDirectory(fileName))
                ThrowError("Filename invalid.", fileName, "Filename is a directory");

            // ensure working with just the filename (removes path)
            fileName = Path.GetFileName(unsafeFileName);
            if (string.IsNullOrWhiteSpace(fileName))
                ThrowError("Filename invalid.", unsafeFileName, "Filename is null or empty after Path.GetFileName");

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
                ThrowError("Extension invalid.", fileName, "Extension is null or empty.");

            // replace periods and spaces and encode
            fileName = $"{Path.GetFileNameWithoutExtension(fileName).Replace(".", "_").Replace(" ", "_")}{extension}";
            fileName = PathHelper.EncodeFileName(fileName);

            // remove invalid chars
            fileName = RegularExpressions.InvalidFileNameCharacters.Replace(fileName, "");

            if (string.IsNullOrWhiteSpace(fileName))
                ThrowError("Filename invalid.", unsafeFileName, "Filename is null or empty after removing invalid filename characters");

            // remove special chars (NOTE: this may not actually be necessary, but extra cleanup to be sure)
            fileName = fileName.RemoveSpecialCharacters("[*'\",&#^@$!%()~`]");

            if (string.IsNullOrWhiteSpace(fileName))
                ThrowError("Filename invalid.", unsafeFileName, "Filename is null or empty after removing special characters");

            // trim filename length down if too long
            // NOTE: using NumUniqueCharsToAdd in calculating how much of the filename to trim from the end
            //       so that when making the filename unique, it will not push the filename past the max.
            //       Static value of "5" is added to the trim amount to ensure enough of the filename is
            //       removed incase of extension or whatever (it's just to be extra sure, probably not necessary. filenames should not be this long.)
            if (_fileStorageSettings.MaxFileNameLength > 0 && Path.GetFileNameWithoutExtension(fileName).Length > _fileStorageSettings.MaxFileNameLength)
                fileName = Path.GetFileNameWithoutExtension(fileName).Substring(0, _fileStorageSettings.MaxFileNameLength - (_fileStorageSettings.NumUniqueCharsToAdd + 5)) + Path.GetExtension(fileName);

            if (makeUnique)
            {
                if (_fileStorageSettings.NumUniqueCharsToAdd < 4 || _fileStorageSettings.NumUniqueCharsToAdd > 24)
                    throw new InvalidOperationException($"File storage setting {nameof(_fileStorageSettings.NumUniqueCharsToAdd)} must have a value between 4 and 24 to append unique chars to filenames.");

                fileName = string.Concat(Path.GetFileNameWithoutExtension(fileName),
                                         "_",
                                         Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(0, _fileStorageSettings.NumUniqueCharsToAdd),
                                         Path.GetExtension(fileName));
            }

            return fileName;
        }

        public virtual IFileValidator ValidateContents(string filename, byte[] fileData)
        {
            if (string.IsNullOrWhiteSpace(filename))
                ThrowError("Invalid or empty filename.");

            var extension = PathHelper.GetLowerExtension(filename);
            if (string.IsNullOrWhiteSpace(extension))
                ThrowError("Invalid or empty extension.", filename, "Extension is null or empty.");

            if (_fileContentValidators.TryGetValue(extension, out IFileContentValidator? contentValidator))
            {
                if (fileData == null || !fileData.Any())
                    ThrowError("Invalid or empty file.", filename, $"FileData is null or empty array when attempting to validate file contents. Extension: {WebUtility.HtmlEncode(extension)}");
                else
                    contentValidator.Validate(fileData);
            }

            return this;
        }

        public virtual IFileValidator ValidateExtension(string filename, DocumentDirectory directory = null!)
        {
            if (string.IsNullOrWhiteSpace(filename))
                ThrowError("Invalid or empty filename.");

            string extension = PathHelper.GetLowerExtension(filename);
            if (string.IsNullOrWhiteSpace(extension))
                ThrowError("Invalid or empty extension.", filename, "Extension is null or empty.");

            if (!_fileStorageSettings.AllowedExtensions.ContainsKey(extension))
                ThrowError($"File extension '{WebUtility.HtmlEncode(extension)}' not allowed.", filename, "Extension not found in whitelist of allowed extensions.");

            if (directory != null! && !directory.ExtensionAccepted(extension))
                ThrowError(DocumentDirectory.FileExtensionNotAllowedRule(extension), filename, $"Extension not allowed in '{directory.Name}' directory.");

            return this;
        }

        public virtual IFileValidator ValidatePath(string unsafePath, bool allowAbsolute = false)
        {
            if (string.IsNullOrWhiteSpace(unsafePath))
                ThrowError("Invalid or empty path.");

            string? directoryPath = Path.GetDirectoryName(unsafePath);

            if (!string.IsNullOrWhiteSpace(directoryPath) && RegularExpressions.InvalidPathCharacters.IsMatch(directoryPath))
                ThrowError("Invalid file path.", unsafePath);

            if (PathHelper.IsAbsolutePath(unsafePath) && !allowAbsolute)
                ThrowError("Invalid file path.", unsafePath);

            if (_fileStorageSettings.MaxFullPathLength > 0 && unsafePath.Length > _fileStorageSettings.MaxFullPathLength)
            {
                ThrowError("Invalid file path. Supplied path exceeds character limit.",
                    unsafePath, 
                    $"File path char length: {unsafePath.Length}. Max path char length: {_fileStorageSettings.MaxFullPathLength}");
            }
                
            return this;
        }

        public virtual IFileValidator ValidateSignature(string filename, byte[] fileData, byte[] allowedChars = null!)
        {
            if (string.IsNullOrWhiteSpace(filename))
                ThrowError("Invalid or empty filename.");

            if (fileData == null || !fileData.Any())
                ThrowError("Invalid or emtpy file.", filename);

            string extension = PathHelper.GetLowerExtension(filename);
            if (string.IsNullOrWhiteSpace(extension))
                ThrowError("Invalid or empty extension.", filename);

            // not truly validating extension here - just making sure it is a globally recognized extension before continuing
            if (!FileExtensions.Instance.TryGetValue(extension, out FileExtension? extensionInfo))
                ThrowError("Invalid extension.", filename, $"Extension '{WebUtility.HtmlEncode(extension)}' not found in global FileExtensions instance.");

            // * Reference: https://docs.microsoft.com/en-us/azure/security/develop/threat-modeling-tool-input-validation#controls-users

            // validate non-signature files (txt, csv, prn, ...)
            if (NonSigExtensions.Contains(extensionInfo!.Extension))
            {
                foreach (byte b in fileData!)
                {
                    // not larger than this guy
                    // ref - https://www.quora.com/What-is-the-meaning-of-0x7F
                    if (b > 0x7F)
                    {
                        if (allowedChars != null)
                        {
                            if (!allowedChars.Contains(b))
                                ThrowError("Invalid file.", filename, $"File contains invalid character '{b}' outside range of 0x7F.");
                        }
                        else
                            ThrowError("Invalid file.", filename, $"File contains invalid character '{b}' outside range of 0x7F.");
                    }
                }
            }
            else if (extensionInfo.Signature != null && extensionInfo.Signature.Any())
            {
                // if extension is registred with a signature to check against, verify with file data, otherwise let it pass
                bool validSignature = false;
                foreach (var b in extensionInfo.Signature)
                {
                    var curFileSig = new byte[b.Length];
                    Array.Copy(fileData!, curFileSig, b.Length);
                    if (curFileSig.SequenceEqual(b))
                    {
                        validSignature = true;
                        break;
                    }
                }

                if (!validSignature)
                    ThrowError("Invalid file.", filename, "Invalid signature.");
            }

            return this;
        }

        public virtual IFileValidator ValidateSize(long size, DocumentDirectory directory = null!)
        {
            if (size > 0 && _fileStorageSettings.MaxFileSize > 0 && size > _fileStorageSettings.MaxFileSize)
                ThrowError(new FileMaxSizeLimitValidationRule(size, _fileStorageSettings.MaxFileSize), reason: "Exceeded max file size for the application.");

            if (directory != null! && size > 0 && directory.MaxFileSize > 0 && size > directory.MaxFileSize)
                ThrowError(new FileMaxSizeLimitValidationRule(size, directory.MaxFileSize), reason: $"Exceeded max file size for the '{directory.Name}' directory.");

            return this;
        }
    }
}