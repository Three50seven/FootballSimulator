using Common.Core.Domain;
using Common.Core.Validation;

namespace Common.Core
{
    /// <summary>
    /// Validate and sanitize provided file paths and filenames for safe, secure storage.
    /// </summary>
    public interface IFileValidator
    {
        /// <summary>
        /// Validate the extension on the provided filename. 
        /// Throws <see cref="ValidationException"/> in the event of invalid extension.
        /// </summary>
        /// <param name="filename">Full path or filename.</param>
        /// <param name="directory">Optional directory on which to validate the extension for allowance.</param>
        /// <returns>Expected same instance of the validator to chain validation calls.</returns>
        /// <exception cref="ValidationException"></exception>
        IFileValidator ValidateExtension(string filename, DocumentDirectory directory = null!);

        /// <summary>
        /// Validate the provided size of a file against a max value.
        /// Throws <see cref="ValidationException"/> in the event of invalid size.
        /// </summary>
        /// <param name="size">Size of file.</param>
        /// <param name="directory">Optional directory on which to validate the size for max allowance.</param>
        /// <returns>Expected same instance of the validator to chain validation calls.</returns>
        /// <exception cref="ValidationException"></exception>
        IFileValidator ValidateSize(long size, DocumentDirectory directory = null!);

        /// <summary>
        /// Validate the signature of a file against any provided signature based on the extension.
        /// Throws <see cref="ValidationException"/> in the event of invalid signature.
        /// </summary>
        /// <param name="filename">Full path or filename.</param>
        /// <param name="fileData">File contents in bytes.</param>
        /// <param name="allowedChars">Optional btyes allowed as part of signature. Defaults to null.</param>
        /// <returns>Expected same instance of the validator to chain validation calls.</returns>
        /// <exception cref="ValidationException"></exception>
        IFileValidator ValidateSignature(string filename, byte[] fileData, byte[] allowedChars = null!);

        /// <summary>
        /// Validates a given path does not have any invalid characters. 
        /// Optionally check to allow or deny the path to be absolute.
        /// Throws <see cref="ValidationException"/> in the event of invalid signature.
        /// </summary>
        /// <param name="unsafePath">Full path, directory path, or filename.</param>
        /// <param name="allowAbsolute">Optional check to allow or deny the path to be absolute. Defaults to false.</param>
        /// <returns>Expected same instance of the validator to chain validation calls.</returns>
        /// <exception cref="ValidationException"></exception>
        IFileValidator ValidatePath(string unsafePath, bool allowAbsolute = false);

        /// <summary>
        /// Validate the contents of a file based on the extension.
        /// Throws <see cref="ValidationException"/> in the event of invalid signature.
        /// </summary>
        /// <param name="filename">Full path or filename.</param>
        /// <param name="fileData">File contents in bytes.</param>
        /// <returns>Expected same instance of the validator to chain validation calls.</returns>
        /// /// <exception cref="ValidationException"></exception>
        IFileValidator ValidateContents(string filename, byte[] fileData);

        /// <summary>
        /// Generate a safe, unique filename to use in place of the provided, unsafe filename.
        /// </summary>
        /// <param name="unsafeFileName">Full path or filename.</param>
        /// <returns>Unique, secure filename, path excluded.</returns>
        /// <exception cref="ValidationException"></exception>
        string GenerateSafeFileName(string unsafeFileName);

        /// <summary>
        /// Clean and santize an unsafe filename for secure storage.
        /// </summary>
        /// <param name="unsafeFileName">Full path or filename.</param>
        /// <param name="makeUnique">Optionally make the filename unique each time. Defaults to true.</param>
        /// <returns>Sanitized filename safe for storage.</returns>
        /// <exception cref="ValidationException"></exception>
        string SanitizeFileName(string unsafeFileName, bool makeUnique = true);
       
    }
}
