using Microsoft.AspNetCore.Authorization;

namespace API.Entity.SecrurityClass
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            AuthenticationSchemes = "Basic";
        }
    }
}
