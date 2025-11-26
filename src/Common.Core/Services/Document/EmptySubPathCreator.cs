using Common.Core.Domain;

namespace Common.Core
{
    /// <summary>
    /// Returns null value on Create.
    /// </summary>
    public class EmptySubPathCreator : ISubPathCreator
    {
        public string Create(DocumentDirectory directory, string fileName)
        {
            return null;
        }
    }
}
