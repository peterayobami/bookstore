using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Book.Store.Api.Server
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AuthorizeAdminAttribute()
        {
            // Add the JWT bearer authentication scheme
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;

            // Authorization policy is admin
            Policy = AuthorizationPolicies.Admin;
        }

        #endregion
    }
}