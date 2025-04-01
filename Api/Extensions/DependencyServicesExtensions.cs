using Core.Interfaces;
using Infrastructure.Security;
using Infrastructure.Indentity;
using Core.Interfaces.Services;
using Infrastructure.Repositories;
using Core.Interfaces.Factories;
using Infrastructure.Factories;

namespace Api.Extensions
{
    public static class DependencyServicesExtensions
    {
        public static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            //Add scope
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register the BCryptPasswordHasher as a singleton
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            // Register the JwtTokenService
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Register the UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register AuthService
            services.AddScoped<LocalAuthService>();

            // Register Factory
            services.AddScoped<IAuthFactory, AuthFactory>();

            return services;
        }
    }
}