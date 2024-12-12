using Microsoft.AspNetCore.Identity;

namespace Book.Store.Api.Server
{
    public class UserService
    {
        private readonly ILogger<UserService> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ILogger<UserService> logger, UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task<OperationResult> LoginAsync(LoginCredentials loginCredentials)
        {
            try
            {
                // The error message when login fails
                var errorMessage = "Invalid username or password";

                // Set the error response for failed login
                var errorResponse = new OperationResult
                {
                    ErrorTitle = "UNAUTHORIZED",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    ErrorMessage = errorMessage
                };

                // If email was not specified...
                if (loginCredentials?.Email == null || string.IsNullOrWhiteSpace(loginCredentials.Email))
                    // Return error response to user
                    return errorResponse;

                // Validate if the user credentials are correct...

                // Get the user
                var user = await userManager.FindByEmailAsync(loginCredentials.Email);

                // If user could not be found...
                if (user == null)
                    // Return error response
                    return errorResponse;

                // Get if password is valid
                var isValidPassword = await userManager.CheckPasswordAsync(user, loginCredentials.Password);

                // If the password was wrong...
                if (!isValidPassword)
                    // Return error response to user
                    return errorResponse;

                // Get username
                var username = user.UserName;

                // Return result
                return new OperationResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Result = new
                    {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.UserName,
                        Token = user.GenerateJwtToken()
                    }
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                // Return error result
                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}