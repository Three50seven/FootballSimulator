using Common.Core.Domain;

namespace Common.Core
{
    public interface ISubPathCreator
    {
        string Create(DocumentDirectory directory, string fileName);
    }
}
