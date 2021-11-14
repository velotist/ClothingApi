using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ClothingApi.Infrastructure
{
    public class AuthorizeEnumAttribute : AuthorizeAttribute
    {
        public AuthorizeEnumAttribute(params object[] roles)
        {
            if (roles.Any(role => role.GetType().BaseType != typeof(Enum)))
            {
                throw new ArgumentException(null, nameof(roles));
            }

            Roles = string.Join(",",
                roles.Select(role => Enum.GetName(role.GetType(), role)));
        }
    }
}
