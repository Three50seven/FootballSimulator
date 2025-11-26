using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class FileStorageExtensions
    {
        /// <summary>
        /// Load stream from a file, copied to <see cref="MemoryStream"/> and returned as bytes.
        /// </summary>
        /// <param name="fileStorage">Valid file storage.</param>
        /// <param name="path">Path to the file. Loaded from <see cref="IFileStorage.Load(string)"/></param>
        /// <returns></returns>
        public static byte[] LoadBytes(this IFileStorage fileStorage, string path)
        {
            if (fileStorage == null)
                return null;

            using (var stream = fileStorage.Load(path))
            {
                if (stream == null)
                    return null;

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Load stream from a file asynchronously, copied to <see cref="MemoryStream"/> and returned as bytes.
        /// </summary>
        /// <param name="fileStorage">Valid file storage.</param>
        /// <param name="path">Path to the file. Loaded from <see cref="IFileStorage.LoadAsync(string)"/></param>
        /// <returns></returns>
        public static async Task<byte[]> LoadBytesAsync(this IFileStorage fileStorage, string path)
        {
            if (fileStorage == null)
                return null;

            using (var stream = await fileStorage.LoadAsync(path))
            {
                if (stream == null)
                    return null;

                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Build unique file name if file with same name exists at the same location.
        /// The filename is incremented by 1 at the end of the filename for each existing file found.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetNextFileName(
            this IFileStorage fileStorage,
            string fileName)
        {
            string extension = Path.GetExtension(fileName);

            int i = 0;
            while (fileStorage.Exists(fileName))
            {
                if (i == 0)
                    fileName = fileName.Replace(extension, $"_{++i}{extension}");
                else
                    fileName = fileName.Replace($"_{i}{extension}", $"_{++i}{extension}");
            }

            return fileName;
        }


        /// <summary>
        /// Build unique file name if file with same name exists at the same location.
        /// The filename is incremented by 1 at the end of the filename for each existing file found.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> GetNextFileNameAsync(
            this IFileStorage fileStorage,
            string fileName)
        {
            string extension = Path.GetExtension(fileName);

            int i = 0;
            while (await fileStorage.ExistsAsync(fileName))
            {
                if (i == 0)
                    fileName = fileName.Replace(extension, $"_{++i}{extension}");
                else
                    fileName = fileName.Replace($"_{i}{extension}", $"_{++i}{extension}");
            }

            return fileName;
        }

        /// <summary>
        /// Copy file from source path to destination path with optional overwrite.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static async Task<FileReference> CopyAsync(
            this IFileStorage fileStorage,
            string sourcePath,
            string destinationPath,
            bool overwrite = false)
        {
            FileReference result = null;

            if (!overwrite)
                destinationPath = await fileStorage.GetNextFileNameAsync(destinationPath);

            using (var fileToCopy = await fileStorage.LoadAsync(sourcePath))
            {
                result = await fileStorage.SaveAsync(fileToCopy, destinationPath, overwrite);
                fileToCopy.Close();
            }

            return result;
        }

        /// <summary>
        /// Copy file from source path to destination path with optional overwrite.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static FileReference Copy(
            this IFileStorage fileStorage,
            string sourcePath,
            string destinationPath,
            bool overwrite = false)
        {
            FileReference result = null;

            if (!overwrite)
                destinationPath = fileStorage.GetNextFileName(destinationPath);

            using (var fileToCopy = fileStorage.Load(sourcePath))
            {
                result = fileStorage.Save(fileToCopy, destinationPath, overwrite);
                fileToCopy.Close();
            }

            return result;
        }

        /// <summary>
        /// Copies file from source path to destination path and removes file at source path.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static async Task<FileReference> MoveAsync(
            this IFileStorage fileStorage,
            string sourcePath,
            string destinationPath,
            bool overwrite = false)
        {
            var result = await fileStorage.CopyAsync(sourcePath, destinationPath, overwrite);
            bool sourceIsSameAsDestination = string.Compare(sourcePath, destinationPath, StringComparison.InvariantCultureIgnoreCase) == 0;

            // delete only if the file has been copied to a new destination
            if (result != null && !sourceIsSameAsDestination)
                await fileStorage.DeleteAsync(sourcePath);

            return result;
        }

        /// <summary>
        /// Copies file from source path to destination path and removes file at source path.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static FileReference Move(
            this IFileStorage fileStorage,
            string sourcePath,
            string destinationPath,
            bool overwrite = false)
        {
            var result = fileStorage.Copy(sourcePath, destinationPath, overwrite);
            bool sourceIsSameAsDestination = string.Compare(sourcePath, destinationPath, StringComparison.InvariantCultureIgnoreCase) == 0;

            // delete only if the file has been copied to a new destination
            if (result != null && !sourceIsSameAsDestination)
                fileStorage.Delete(sourcePath);

            return result;
        }

        /// <summary>
        /// Read a local file stored on disk and save file to the destination relative path.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="localFilePath"></param>
        /// <param name="destinationRelativePath"></param>
        /// <param name="overwrite"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static FileReference PublishLocalFile(
            this IFileStorage fileStorage,
            string localFilePath,
            string destinationRelativePath,
            bool overwrite = false,
            int buffer = 4096)
        {
            Guard.IsNotNull(localFilePath, nameof(localFilePath));
            Guard.IsNotNull(destinationRelativePath, nameof(destinationRelativePath));

            if (!File.Exists(localFilePath))
                throw new FileNotFoundException("Local file not found on publish attempt.", localFilePath);

            using (var fileStream = new FileStream(localFilePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None,
                bufferSize: buffer,
                useAsync: false))
            {
                return fileStorage.Save(fileStream, destinationRelativePath, overwrite);
            }
        }

        /// <summary>
        /// Read a local file stored on disk and save file to the destination relative path.
        /// </summary>
        /// <param name="fileStorage"></param>
        /// <param name="localFilePath"></param>
        /// <param name="destinationRelativePath"></param>
        /// <param name="overwrite"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<FileReference> PublishLocalFileAsync(
          this IFileStorage fileStorage,
          string localFilePath,
          string destinationRelativePath,
          bool overwrite = false,
          int buffer = 4096)
        {
            Guard.IsNotNull(localFilePath, nameof(localFilePath));
            Guard.IsNotNull(destinationRelativePath, nameof(destinationRelativePath));

            if (!File.Exists(localFilePath))
                throw new FileNotFoundException("Local file not found on publish attempt.", localFilePath);

            using (var fileStream = new FileStream(localFilePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None,
                bufferSize: buffer,
                useAsync: true))
            {
                return await fileStorage.SaveAsync(fileStream, destinationRelativePath, overwrite);
            }
        }
    }
}
