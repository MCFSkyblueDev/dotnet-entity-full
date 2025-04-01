using Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Security;
using Infrastructure.Indentity;

namespace Api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationAutoValidation();
            // services.Services.AddFluentValidationClientsideAdapters();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            //Add Config
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            //Add Jwt
            services.AddAuthentication((options) =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((options) =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["AppSettings:Jwt:Issuer"]!,
                    // ValidIssuer = services.Configuration.GetSection("Jwt").GetValue<string>("Issuer"),
                    ValidAudience = configuration["AppSettings:Jwt:Audience"]!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            }).AddCookie(options =>
            {
                options.LoginPath = "/auth/login";  // Redirect if unauthorized
                options.LogoutPath = "/auth/logout";
                // options.AccessDeniedPath = "/auth/access-denied";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Secure cookies in production
                options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF attacks
            });

            var coreAssembly = Assembly.Load("Core");
            services.AddValidatorsFromAssembly(coreAssembly!);
            //* Apply all Mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}