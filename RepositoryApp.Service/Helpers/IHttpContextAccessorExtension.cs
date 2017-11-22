using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace RepositoryApp.Service.Helpers
{
    public static class IHttpContextAccessorExtension
    {
        public static Guid CurrentUser(this IHttpContextAccessor accessor)
        {
            try
            {
                return Guid.Parse(accessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Iat).Value);
            }
            catch (Exception e)
            {
                throw new Exception("Guid convertin " + e.Message);
            }
        }
    }
}