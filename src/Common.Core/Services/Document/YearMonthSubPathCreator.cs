using Common.Core.Domain;
using System;

namespace Common.Core
{
    /// <summary>
    /// Returns the current date's year and month as "yyyy/MM".
    /// Intended for storage of many content files where files should be scattered across sub directories.
    /// </summary>
    public class YearMonthSubPathCreator : ISubPathCreator
    {
        public string Create(DocumentDirectory directory, string fileName)
        {
            return DateTime.Today.ToString("yyyy/MM");
        }
    }
}
