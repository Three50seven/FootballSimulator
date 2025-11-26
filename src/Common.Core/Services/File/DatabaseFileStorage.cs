using Common.Core.Domain;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Files stored as file path and bytes in a database.
    /// </summary>
    public class DatabaseFileStorage : IFileStorage
    {
        public DatabaseFileStorage(IFileSourceRepository fileSourceRepository)
        {
            FileSourceRepository = fileSourceRepository;
        }

        public IFileSourceRepository FileSourceRepository { get; }


        public void Delete(string path)
        {
            throw new NotSupportedException("Delete method is not supported. Call DeleteAsync instead.");
        }

        public Task DeleteAsync(string path)
        {
            return FileSourceRepository.DeleteAsync(path);
        }

        public bool Exists(string path)
        {
            throw new NotSupportedException("Exists method is not supported. Call ExistsAsync instead.");
        }

        public Task<bool> ExistsAsync(string path)
        {
            return FileSourceRepository.ExistsAsync(path);
        }

        public Stream Load(string path)
        {
            throw new NotSupportedException("Load method is not supported. Call LoadAsync instead.");
        }

        public async Task<Stream> LoadAsync(string path)
        {
            var source = await FileSourceRepository.GetByPathAsync(path);
            if (source == null)
                return null;

            return new MemoryStream(source.Source, writable: false);
        }

        public string ReadContent(string path)
        {
            throw new NotSupportedException("ReadContent method is not supported. Call ReadContentAsync instead.");
        }

        public async Task<string> ReadContentAsync(string path)
        {
            var source = await FileSourceRepository.GetByPathAsync(path);
            if (source == null)
                throw new FileNotFoundException("File not found in database.", path);

            return Encoding.UTF8.GetString(source.Source, 0, source.Source.Length);
        }

        public FileReference Save(Stream stream, string path, bool overwrite = false)
        {
            throw new NotSupportedException("Save method is not supported. Call SaveAsync instead.");
        }

        public async Task<FileReference> SaveAsync(Stream stream, string path, bool overwrite = false)
        {
            await FileSourceRepository.AddOrUpdateAsync(new FileSource(path, stream.ToArray()));
            return new FileReference(path, stream.Length);
        }

        public FileReference Save(string content, string path, bool overwrite = false)
        {
            throw new NotSupportedException("Save method is not supported. Call SaveAsync instead.");
        }

        public async Task<FileReference> SaveAsync(string content, string path, bool overwrite = false)
        {
            var bytes = Encoding.Unicode.GetBytes(content);
            await FileSourceRepository.AddOrUpdateAsync(new FileSource(path, bytes));
            return new FileReference(path, bytes.Length);
        }
    }
}
