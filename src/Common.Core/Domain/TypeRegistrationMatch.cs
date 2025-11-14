namespace Common.Core.Domain
{
    public class TypeRegistrationMatch : ValueObject<TypeRegistrationMatch>
    {
        public Type Declaration { get; private set; }

        public Type Implementation { get; private set; }

        public TypeRegistrationMatch(Type declaration, Type implementation)
        {
            Declaration = declaration;
            Implementation = implementation;
        }
    }
}
