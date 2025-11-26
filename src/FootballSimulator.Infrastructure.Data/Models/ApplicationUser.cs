using FootballSimulator.Core.Domain;
using Microsoft.AspNetCore.Identity;

namespace FootballSimulator.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
