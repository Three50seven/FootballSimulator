using Common.Core.Domain;

namespace Common.Core.Interfaces
{
    public interface IPasswordGenerator
    {
        string Generate(PasswordRequirements requirements = null);
    }
}
