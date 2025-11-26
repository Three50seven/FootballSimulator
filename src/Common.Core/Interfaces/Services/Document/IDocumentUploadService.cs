using Common.Core.Domain;
using Common.Core.DTOs;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IDocumentUploadService
    {
        DocumentSaveResult Save(DocumentUpload documentUpload);
        Task<DocumentSaveResult> SaveAsync(DocumentUpload documentUpload);
    }
}
