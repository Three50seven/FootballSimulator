using System;

namespace Common.Core.Domain
{
    public class TypeRegistrationMatch : ValueObject<TypeRegistrationMatch>
    {
        public TypeRegistrationMatch(Type declaration, Type implementation)
        {
            Declaration = declaration;
            Implementation = implementation;
        }

        public Type Declaration { get; private set; }
        public Type Implementation { get; private set; }
    }
}
