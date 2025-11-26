using Microsoft.AspNetCore.Authorization;
using Common.Core;
using System;
using System.Linq;

namespace Common.AspNetCore.Mvc
{
    public abstract class AuthorizeBaseAttribute : AuthorizeAttribute
    {
        protected virtual void SetRole(Enum role)
        {
            SetRoles(role);
        }

        protected virtual void SetRoles(params Enum[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                Roles = string.Join(",", roles.Select(x => x.AsFriendlyName()));
            }
        }
    }
}
