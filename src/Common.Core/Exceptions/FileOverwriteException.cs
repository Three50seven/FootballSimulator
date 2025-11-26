using System;

namespace Common.Core
{
    [Serializable]
    public class FileOverwriteException : Exception
    {
        public FileOverwriteException() { }
        public FileOverwriteException(string filePath) : base($"File overwrite denied. File already exists at '{filePath.Sanitize()}' and may not be overwritten.") { }
        public FileOverwriteException(string message, Exception inner) : base(message, inner) { }
    }
}
