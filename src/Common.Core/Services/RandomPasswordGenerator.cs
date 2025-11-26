using Common.Core.Domain;
using Common.Core.Interfaces;
using System;

namespace Common.Core.Services
{
	/// <summary>
	/// Generates random string of characters for use as a password.
	/// </summary>
    public class RandomPasswordGenerator : IPasswordGenerator
	{
		public static int MaxIdenticalConsecutiveChars = 2;
		public static int MaxAttempts = 1000;

		public string Generate(PasswordRequirements requirements = null)
		{
			if (requirements == null)
				requirements = new PasswordRequirements();

			int attempts = 0;
			string password;

			do
			{
				password = GenerateInternal(requirements);
				attempts++;
			}
			while (attempts < MaxAttempts && !requirements.IsValid(password));

			if (!requirements.IsValid(password))
				throw new InvalidOperationException($"Random password generation failed to generate a valid password in {MaxAttempts} attempts.");

			System.Diagnostics.Debug.WriteLine($"Random password generated in {attempts} attempts.");

			return password;
		}

		private static string GenerateInternal(PasswordRequirements requirements)
		{
			char[] passwordChars = new char[requirements.Length];
			int characterSetLength = requirements.CharacterSet.Length;

			var random = new Random();

			for (int characterPosition = 0; characterPosition < requirements.Length; characterPosition++)
			{
				passwordChars[characterPosition] = requirements.CharacterSet[random.Next(characterSetLength - 1)];

				// check identical chars - if failed, reset position
				if (characterPosition > MaxIdenticalConsecutiveChars
					&& passwordChars[characterPosition] == passwordChars[characterPosition - 1]
					&& passwordChars[characterPosition - 1] == passwordChars[characterPosition - 2])
				{
					characterPosition--;
				}
			}

			return string.Join(null, passwordChars);
		}
	}
}
