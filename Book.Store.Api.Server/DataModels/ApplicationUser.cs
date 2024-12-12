using Microsoft.AspNetCore.Identity;

namespace Book.Store.Api.Server
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Scope { get; set; }
    }
}