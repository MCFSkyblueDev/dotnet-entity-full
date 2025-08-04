using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Security;
using Infrastructure.Indentity;
using Microsoft.AspNetCore.Antiforgery;


namespace Api.Extensions
{
    public static class SecurityExtensions
    {
        public static IServiceCollection AddSecurities(this IServiceCollection services, IConfiguration configuration)
        {
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
                // 👇 Cho phép lấy token từ cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
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

            // Antiforgery (CSRF Protection)
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN";
                options.Cookie.HttpOnly = false; // Cho phép JS đọc
                options.HeaderName = "X-CSRF-TOKEN";
            });

            // CORS 
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins(["http://localhost:4200"])
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });



            return services;
        }


        public static void UseCsrfTokenCookie(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

                if (HttpMethods.IsGet(context.Request.Method))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("X-CSRF-TOKEN", tokens.RequestToken!,
                        new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });
                }

                await next();
            });
        }
    }
}