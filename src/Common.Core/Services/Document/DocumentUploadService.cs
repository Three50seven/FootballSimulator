using Common.Core.Domain;
using Common.Core.DTOs;
using Common.Core.Validation;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    public class DocumentUploadService : IDocumentUploadService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IFileValidator _fileValidator;
        private readonly ISubPathCreator _subPathCreator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandRepository<Document> _documentRepository;
        private readonly IQueryRepository<DocumentDirectory> _directoryRepository;

        public DocumentUploadService(
            IFileStorage fileStorage, 
            IFileValidator fileValidator,
            ISubPathCreator subPathCreator,
            IUnitOfWork unitOfWork, 
            ICommandRepository<Document> documentRepository,
            IQueryRepository<DocumentDirectory> directoryRepository)
        {
            _fileStorage = fileStorage;
            _fileValidator = fileValidator;
            _subPathCreator = subPathCreator;
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
            _directoryRepository = directoryRepository;
        }

        public virtual DocumentSaveResult Save(DocumentUpload documentUpload)
        {
            Guard.IsNotNull(documentUpload, nameof(documentUpload));

            try
            {
                var directory = _directoryRepository.GetById(documentUpload.DirectoryId);
                if (directory == null)
                    throw new DataObjectNotFoundException(nameof(DocumentDirectory), documentUpload.DirectoryId);

                string sanitizedFilename = _fileValidator.SecureFile(documentUpload, directory);
                string subpath = _subPathCreator.Create(directory, sanitizedFilename).SetNullToEmpty();

                var fileInfo = _fileStorage.Save(
                    documentUpload.Stream, 
                    PathHelper.Combine(directory.Path, subpath, sanitizedFilename));

                var document = new Document(fileInfo.File, directory, subpath, name: PathHelper.EncodeFileName(documentUpload.FileName));

                _documentRepository.AddOrUpdate(document);
                _unitOfWork.Save(); //perform save to generate document Id value

                return DocumentSaveResult.Success(new DocumentSaveResultItem(document));
            }
            catch (ValidationException vex)
            {
                return DocumentSaveResult.Fail(vex.BrokenRules);
            }
        }

        public virtual async Task<DocumentSaveResult> SaveAsync(DocumentUpload documentUpload)
        {
            Guard.IsNotNull(documentUpload, nameof(documentUpload));

            try
            {
                var directory = await _directoryRepository.GetByIdAsync(documentUpload.DirectoryId);
                if (directory == null)
                    throw new DataObjectNotFoundException(nameof(DocumentDirectory), documentUpload.DirectoryId);

                string sanitizedFilename = _fileValidator.SecureFile(documentUpload, directory);
                string subpath = _subPathCreator.Create(directory, sanitizedFilename).SetNullToEmpty();

                var fileInfo = await _fileStorage.SaveAsync(
                   documentUpload.Stream,
                   PathHelper.Combine(directory.Path, subpath, sanitizedFilename));

                var document = new Document(fileInfo.File, directory, subpath, name: PathHelper.EncodeFileName(documentUpload.FileName));

                await _documentRepository.AddOrUpdateAsync(document);
                await _unitOfWork.SaveAsync(); //perform save to generate document Id value

                return DocumentSaveResult.Success(new DocumentSaveResultItem(document));
            }
            catch (ValidationException vex)
            {
                return DocumentSaveResult.Fail(vex.BrokenRules);
            }
        }
    }
}
