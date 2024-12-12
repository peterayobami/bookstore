using System.Text;
using Dna;
using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Book.Store.Api.Server
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

            return services;
        }

        public static IServiceCollection AddDatabaseSetup(this IServiceCollection services)
        {
            return services.AddScoped<DatabaseSetupMiddleware>();
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<BookService>();
            services.AddScoped<UserService>();

            return services;
        }

        public static IApplicationBuilder UseDatabaseSetup(this IApplicationBuilder app)
        {
            app.UseMiddleware<DatabaseSetupMiddleware>();

            return app;
        }

        /// <summary>
        /// Registers application identity to the DI
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
        {
            // Configure identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers the Identity Server configuration to DI container
        /// </summary>
        /// <param name="services">The instance of <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The instance of <see cref="IConfiguration"/></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityServerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServer(options =>
            {
                //options.UserInteraction.LoginUrl = WebRoutes.SignIn;
            })
                .AddConfigurationStore<ConfigurationDbContext>(options => options.ConfigureDbContext =
                    builder => builder.UseSqlServer(configuration.GetConnectionString("ConfigurationConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

            // Return services for further chaining
            return services;
        }

        public static IServiceCollection AddAuthentizationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration["Jwt:Authority"];

                    // Set validation parameters
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate issuer
                        ValidateIssuer = true,
                        // Validate audience
                        ValidateAudience = true,
                        // Validate expiration
                        ValidateLifetime = true,
                        // Validate signature
                        ValidateIssuerSigningKey = true,

                        // Set issuer
                        ValidIssuer = Framework.Construction.Configuration["Jwt:Issuer"],
                        // Set audience
                        ValidAudience = Framework.Construction.Configuration["Jwt:Audience"],

                        // Set signing key
                        IssuerSigningKey = new SymmetricSecurityKey(
                            // Get our secret key from configuration
                            Encoding.UTF8.GetBytes(Framework.Construction.Configuration["Jwt:SecretKey"])),
                    };
                });

            // Return services for further chaining
            return services;
        }

        /// <summary>
        /// Registers authorization configuration to DI container
        /// </summary>
        /// <param name="services">The instance of the <see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
        {
            // Add authorization and configure policies
            services.AddAuthorization(options =>
            {
                // Configure authorization policy for admin
                options.AddPolicy(AuthorizationPolicies.Admin, policy =>
                {
                    policy.RequireAuthenticatedUser();

                    policy.RequireClaim(JwtClaimTypes.Scope, UserScopes.Admin);
                });
            });

            // Return services for further chaining
            return services;
        }
    }
}