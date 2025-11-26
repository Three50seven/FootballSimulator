using Common.Core.Domain;
using Common.Core.DTOs;
using Common.Core.Services;
using Common.Core.Validation;
using System.Collections.Generic;
using System.IO;

namespace Common.Core
{
    public static class FileValidatorExtensions
    {
        /// <summary>
        /// Validate the signature of a file against any provided signature based on the extension.
        /// Throws <see cref="ValidationException"/> in the event of invalid signature.
        /// </summary>
        /// <param name="fileValidator"></param>
        /// <param name="filePath">Full path or filename.</param>
        /// <param name="fileData">File contents in stream. <see cref="StreamExtensions.ToArray(Stream, bool)"/> is called to check contents.</param>
        /// <param name="allowedChars">Optional btyes allowed as part of signature. Defaults to null.</param>
        /// <returns>Expected same instance of the validator to chain validation calls.</returns>
        /// <exception cref="ValidationException"></exception>
        public static IFileValidator ValidateSignature(this IFileValidator fileValidator, string filePath, Stream fileData, byte[] allowedChars = null)
        {
            return fileValidator.ValidateSignature(filePath, fileData?.ToArray(), allowedChars);
        }

        /// <summary>
        /// Secures and sanitizes a filename or full path. Returns the sanitized filename.
        /// Validates againsts extension, path, signature, and size. Also validates all applicable checks against the provided directory.
        /// Throws <see cref="ValidationException"/> in the event of the file being invalid.
        /// </summary>
        /// <param name="fileValidator"></param>
        /// <param name="filePath">Full path or filename</param>
        /// <param name="size">Size of file.</param>
        /// <param name="fileData">File contents in bytes.</param>
        /// <param name="directory">Optional directory where file is being stored.</param>
        /// <param name="makeUnique">Optionally make the filename unique each time. Defaults to true.</param>
        /// <param name="allowedChars">Optional btyes allowed as part of signature. Defaults to null.</param>
        /// <returns>Sanitized filename safe for storage.</returns>
        /// <exception cref="ValidationException"></exception>
        public static string SecureFile(
            this IFileValidator fileValidator, 
            string filePath, 
            long size, 
            byte[] fileData,
            DocumentDirectory directory = null,
            bool makeUnique = true,
            byte[] allowedChars = null)
        {
            return fileValidator.ValidateExtension(filePath, directory)
                                .ValidatePath(filePath)
                                .ValidateSignature(filePath, fileData, allowedChars)
                                .ValidateSize(size, directory)
                                .ValidateContents(filePath, fileData)
                                .SanitizeFileName(filePath, makeUnique);
        }

        /// <summary>
        /// Secures and sanitizes a filename or full path. Returns the sanitized filename.
        /// Validates againsts extension, path, signature, and size. Also validates all applicable checks against the provided directory.
        /// Throws <see cref="ValidationException"/> in the event of the file being invalid.
        /// </summary>
        /// <param name="fileValidator"></param>
        /// <param name="filePath">Full path or filename</param>
        /// <param name="size">Size of file.</param>
        /// <param name="fileData">File contents in stream. <see cref="StreamExtensions.ToArray(Stream, bool)"/> is called to check contents.</param>
        /// <param name="directory">Optional directory where file is being stored.</param>
        /// <param name="makeUnique">Optionally make the filename unique each time. Defaults to true.</param>
        /// <param name="allowedChars">Optional btyes allowed as part of signature. Defaults to null.</param>
        /// <returns>Sanitized filename safe for storage.</returns>
        /// <exception cref="ValidationException"></exception>
        public static string SecureFile(
            this IFileValidator fileValidator, 
            string filePath, 
            long size, 
            Stream fileData, 
            DocumentDirectory directory = null,
            bool makeUnique = true,
            byte[] allowedChars = null)
        {
            return SecureFile(fileValidator, filePath, size, fileData?.ToArray(), directory, makeUnique, allowedChars);
        }

        /// <summary>
        /// Secures and sanitizes a filename from <see cref="FileUpload.FileName"/>. Returns the sanitized filename.
        /// Validates againsts extension, path, signature, and size from <see cref="DocumentUpload"/>. Also validates all applicable checks against the provided directory.
        /// Throws <see cref="ValidationException"/> in the event of the file being invalid.
        /// </summary>
        /// <param name="fileValidator"></param>
        /// <param name="documentUpload">Required document upload information for the file being validated.</param>
        /// <param name="directory">Required directory where file is being stored.</param>
        /// <param name="makeUnique">Optionally make the filename unique each time. Defaults to true.</param>
        /// <param name="allowedChars">Optional btyes allowed as part of signature. Defaults to null.</param>
        /// <returns>Sanitized filename safe for storage.</returns>
        /// <exception cref="ValidationException"></exception>
        public static string SecureFile(
            this IFileValidator fileValidator, 
            DocumentUpload documentUpload, 
            DocumentDirectory directory, 
            bool makeUnique = true,
            byte[] allowedChars = null)
        {
            Guard.IsNotNull(documentUpload, nameof(documentUpload));
            Guard.IsNotNull(directory, nameof(directory));

            return SecureFile(fileValidator, documentUpload.FileName, documentUpload.Length, documentUpload.Stream, directory, makeUnique, allowedChars);
        }

        /// <summary>
        /// Validates a file against extension, path, size, signature, and contents.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="file"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IFileValidator ValidateFile(this IFileValidator validator, FileUpload file, DocumentDirectory directory)
        {
            Guard.IsNotNull(file, nameof(file));

            var bytes = file.Stream.ToArray();

            return validator.ValidateExtension(file.FileName, directory)
                            .ValidatePath(file.FileName)
                            .ValidateSize(file.Length, directory)
                            .ValidateSignature(file.FileName, bytes)
                            .ValidateContents(file.FileName, bytes);
        }

        /// <summary>
        /// Validates multiple files against extension, path, size, signature, and contents.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="files"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IFileValidator ValidateFiles(this IFileValidator validator, IEnumerable<FileUpload> files, DocumentDirectory directory)
        {
            Guard.IsNotNull(files, nameof(files));

            foreach (var file in files)
            {
                validator = ValidateFile(validator, file, directory);
            }

            return validator;
        }
    }
}
