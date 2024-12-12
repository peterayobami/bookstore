using Microsoft.AspNetCore.Mvc;

namespace Book.Store.Api.Server
{
    /// <summary>
    /// Manages standard Web request for user operations
    /// </summary>
    public class UserController : BaseController<UserController>
    {
        #region Private Members

        private readonly UserService userService;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger{TCategory}"/></param>
        /// <param name="userService">The injected <see cref="UserService"/></param>
        /// <returns></returns>
        public UserController(ILogger<UserController> logger, UserService userService) : base(logger)
        {
            this.userService = userService;
        }

        #endregion

        /// <summary>
        /// Handles user login based on specified login credentials
        /// </summary>
        /// <param name="loginCredentials">The specified login credentials</param>
        /// <returns>An instance of <see cref="ActionResult"/> for this operation</returns>
        [HttpPost(EndpointRoutes.Login)]
        public async Task<ActionResult> LoginAsync([FromBody] LoginCredentials loginCredentials)
        {
            return await HandleOperationAsync(() => userService.LoginAsync(loginCredentials));
        }
    }
}