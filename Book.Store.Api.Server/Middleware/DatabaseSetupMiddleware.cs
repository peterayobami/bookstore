
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book.Store.Api.Server
{
    public class DatabaseSetupMiddleware : IMiddleware
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<DatabaseSetupMiddleware> logger;

        public DatabaseSetupMiddleware(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DatabaseSetupMiddleware> logger)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await dbContext.Database.MigrateAsync();

                // Create Admin user

                if (!await dbContext.Users.AnyAsync())
                {
                    // Set the user's details
                    // NB: I decided to follow one of my examples and use scope instead of role.
                    var user = new ApplicationUser
                    {
                        Email = "johndoe@bookstore.com",
                        UserName = "johndoe",
                        FirstName = "John",
                        LastName = "Doe",
                        Scope = UserScopes.Admin
                    };
                    
                    // Create the user
                    var result = await userManager.CreateAsync(user, "myPassw0rd");

                    // Create admin role
                    var roleResult = await userManager.AddToRoleAsync(user, "Admin");

                    // If admin user was created successfully...
                    if (result.Succeeded)
                    {
                        // Fetch the user
                        var adminUser = await userManager.FindByNameAsync("peter");

                        // Create claims
                        await userManager.AddClaimsAsync(adminUser, new List<Claim>
                        {
                            new(ClaimTypes.Name, "johndoe"),
                            new(ClaimTypes.Email, "johndoe@bookstore.com"),
                            new(JwtClaimTypes.Scope, UserScopes.Admin)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            await next(context);
        }
    }
}