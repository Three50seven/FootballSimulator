using Common.Core.Validation;
using System;

namespace Common.Core.Domain
{
    public class FileSource : Entity<Guid>
    {
        public FileSource(string path, byte[] source)
        {
            Guard.IsNotNull(path, nameof(path));

            Path = path.Trim();
            Source = source;
        }

        public FileSource(string path, byte[] source, Guid guid)
            : base (guid)
        {
            Guard.IsNotNull(path, nameof(path));

            Path = path.Trim();
            Source = source;
        }


        public string Path { get; private set; }
        public byte[] Source { get; private set; }
    }
}
