using System;

namespace Common.Core
{
    [Serializable]
    public class DataObjectNotFoundException : Exception
    {
        private static string FormatMessage(object objectName) => $"The data object {objectName} was not found.";

        public DataObjectNotFoundException() { }
        public DataObjectNotFoundException(string objectName) : base(FormatMessage(objectName)) { }

        public DataObjectNotFoundException(string objectName, int id) : base($"{FormatMessage(objectName)} Supplied Id: {id}") { }

        public DataObjectNotFoundException(string objectName, int? id) : base($"{FormatMessage(objectName)} Supplied Id: {id}") { }

        public DataObjectNotFoundException(string objectName, Guid guid) : base($"{FormatMessage(objectName)} Supplied Guid: {guid}") { }

        public DataObjectNotFoundException(string objectName, Guid? guid) : base($"{FormatMessage(objectName)} Supplied Guid: {guid}") { }

        public DataObjectNotFoundException(string objectName, string message) : base($"{FormatMessage(objectName)} Message: {message}") { }

        public DataObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
