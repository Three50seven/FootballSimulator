namespace Common.Core.Domain
{
    public interface IName
    {
        string FirstName { get; }

        string LastName { get; }

        string Initials { get; }

        string ToFormalString();
    }
}
