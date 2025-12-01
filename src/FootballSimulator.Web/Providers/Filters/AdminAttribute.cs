using Common.AspNetCore.Mvc;
using FootballSimulator.Core;

namespace FootballSimulator.Web.Providers.Filters
{
    public class AdminAttribute : AuthorizeBaseAttribute
    {
        public AdminAttribute()            
        {
            SetRole(RoleOption.Admin);
        }
    }
}
