using Common.Core.Domain;
using Common.Core.Validation;
using System.IO;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class DocumentUploadServiceExtensions
    {
        /// <summary>
        /// Save Document file and database record from set of file data. Bytes from <paramref name="fileData"/> 
        /// are read into <see cref="MemoryStream"/> and applied to <see cref="IDocumentUploadService.SaveAsync(DocumentUpload)"/>.
        /// </summary>
        /// <param name="documentUploadService">Valid document service.</param>
        /// <param name="fileData">Required file data.</param>
        /// <param name="directoryId">Directory for the document.</param>
        /// <returns></returns>
        public static async Task<DocumentSaveResult> SaveAsync(this IDocumentUploadService documentUploadService, FileData fileData, int directoryId)
        {
            Guard.IsNotNull(documentUploadService, nameof(documentUploadService));
            Guard.IsNotNull(fileData, nameof(fileData));

            DocumentSaveResult documentSaveResult = null;

            using (var stream = new MemoryStream(fileData.Data))
            {
                documentSaveResult = await documentUploadService.SaveAsync(new DocumentUpload(
                    stream,
                    fileData.FileName,
                    directoryId,
                    length: fileData.Data.Length));
            }

            return documentSaveResult;
        }

        /// <summary>
        /// Save Document file and database record from set of file data. Bytes from <paramref name="fileData"/> 
        /// are read into <see cref="MemoryStream"/> and applied to <see cref="IDocumentUploadService.Save(DocumentUpload)"/>.
        /// </summary>
        /// <param name="documentUploadService">Valid document service.</param>
        /// <param name="fileData">Required file data.</param>
        /// <param name="directoryId">Directory for the document.</param>
        /// <returns></returns>
        public static DocumentSaveResult Save(this IDocumentUploadService documentUploadService, FileData fileData, int directoryId)
        {
            Guard.IsNotNull(documentUploadService, nameof(documentUploadService));
            Guard.IsNotNull(fileData, nameof(fileData));

            DocumentSaveResult documentSaveResult = null;

            using (var stream = new MemoryStream(fileData.Data))
            {
                documentSaveResult = documentUploadService.Save(new DocumentUpload(
                    stream,
                    fileData.FileName,
                    directoryId,
                    length: fileData.Data.Length));
            }

            return documentSaveResult;
        }
    }
}
