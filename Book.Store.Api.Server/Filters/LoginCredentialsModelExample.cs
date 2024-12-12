using Swashbuckle.AspNetCore.Filters;

namespace Book.Store.Api.Server
{
    /// <summary>
    /// Create book credential examples
    /// </summary>
    public class LoginCredentialsModelExample : IExamplesProvider<LoginCredentials>
    {
        /// <summary>
        /// Gets the sample value for object properties
        /// </summary>
        /// <returns></returns>
        public LoginCredentials GetExamples()
        {
            return new LoginCredentials
            {
                Email = "johndoe@bookstore.com",
                Password = "myPassw0rd"
            };
        }
    }
}