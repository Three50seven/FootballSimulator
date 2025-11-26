using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Core.Domain
{
    public class PasswordRequirements : ValueObject<PasswordRequirements>
    {
        private const string _lowerChars = "abcdefghijklmnopqrstuvwxyz";
        private const string _upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _numericChars = "0123456789";
        private const string _specialChars = @"!#$%&*@\";

        public PasswordRequirements(
            bool includeLowercase = true,
            bool includeUppercase = true,
            bool includeNumbers = true,
            bool includeSpecial = true,
            int length = 12)
        {
            IncludeLowercase = includeLowercase;
            IncludeUppercase = includeUppercase;
            IncludeNumbers = includeNumbers;
            IncludeSpecial = includeSpecial;
            Length = length;

            if (length <= 2)
                throw new InvalidOperationException("Password length requirement must be greater than 2.");

            var characterSet = new StringBuilder();

            if (includeLowercase)
                characterSet.Append(_lowerChars);

            if (includeUppercase)
                characterSet.Append(_upperChars);

            if (includeNumbers)
                characterSet.Append(_numericChars);

            if (includeSpecial)
                characterSet.Append(_specialChars);

            CharacterSet = characterSet.ToString();
        }

        public bool IncludeLowercase { get; }
        public bool IncludeUppercase { get; }
        public bool IncludeNumbers { get; }
        public bool IncludeSpecial { get; }
        public int Length { get; }
        public string CharacterSet { get; }

        public bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            bool lowerCaseIsValid = !IncludeLowercase || (IncludeLowercase && Regex.IsMatch(password, @"[a-z]"));
            bool upperCaseIsValid = !IncludeUppercase || (IncludeUppercase && Regex.IsMatch(password, @"[A-Z]"));
            bool numericIsValid = !IncludeNumbers || (IncludeNumbers && Regex.IsMatch(password, @"[\d]"));
            bool symbolsAreValid = !IncludeSpecial || (IncludeSpecial && Regex.IsMatch(password, @"([!#$%&*@\\])+"));

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid;
        }
    }
}
