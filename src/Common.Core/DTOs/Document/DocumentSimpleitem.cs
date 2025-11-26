using System;

namespace Common.Core.DTOs
{
    public class DocumentSimpleItem
    {
        protected DocumentSimpleItem() { }

        public DocumentSimpleItem(int id, string fullPath, string fileName) 
            : this (id, fullPath, fileName, fileName)
        {

        }

        public DocumentSimpleItem(int id, string fullPath, string fileName, string name)
            : this(id, Guid.Empty, fullPath, fileName, name)
        {

        }

        public DocumentSimpleItem(int id, Guid guid, string fullPath, string fileName)
            : this (id, guid, fullPath, fileName, fileName)
        {
            
        }

        public DocumentSimpleItem(int id, Guid guid, string fullPath, string fileName, string name)
        {
            Id = id;
            Guid = guid;
            FullPath = fullPath;
            FileName = fileName;
            Name = name;
        }

        public virtual int Id { get; protected set; }
        public virtual Guid Guid { get; protected set; }
        public virtual string FullPath { get; protected set; }
        public virtual string FileName { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual bool IsImage { get; protected set; }
    }
}
