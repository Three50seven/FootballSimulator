using Common.Core;
using System.ComponentModel.DataAnnotations;

namespace FootballSimulator.Core.DTOs
{
    public class UserSearchFilter
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? UserName { get; set; }
        [MaxLength(256)]
        public string? Email { get; set; }

        public void Clean()
        {
            FirstName = FirstName?.SetEmptyToNull()?.ToLower();
            LastName = LastName?.SetEmptyToNull()?.ToLower();
            UserName = UserName?.SetEmptyToNull()?.ToLower();
            Email = Email?.SetEmptyToNull()?.ToLower();
        }
    }
}
