using Common.Core.Services;
using System.IO;

namespace Common.Core.Domain
{
    public class FileReference : ValueObject<FileReference>
    {
        protected FileReference() { }

        public FileReference(string path, long size)
            : this(PathHelper.GetDirectoryPath(path), path, new FileDetail(Path.GetFileName(path), size))
        {

        }

        public FileReference(string directory, string filename, string fullPath) 
            : this(directory, fullPath, new FileDetail(filename))
        {

        }

        public FileReference(string directory, string fullPath, FileDetail fileDetail)
        {
            Directory = directory;
            FullPath = fullPath;
            File = fileDetail;
        }

        public string Directory { get; private set; }
        public string FullPath { get; private set; }
        public FileDetail File { get; private set; }

        public override string ToString()
        {
            return FullPath;
        }
    }
}
