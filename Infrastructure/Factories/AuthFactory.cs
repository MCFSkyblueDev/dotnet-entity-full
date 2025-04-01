using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Core.Interfaces.Services;
using Infrastructure.Indentity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Factories
{
    public class AuthFactory(IServiceProvider serviceProvider) : IAuthFactory
    {

        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public IAuthService CreateAuthService(string provider)
        {
            return provider.Trim().ToLowerInvariant() switch
            {
                "local" => _serviceProvider.GetRequiredService<LocalAuthService>(),
                _ => throw new ArgumentException("Invalid authentication provider")
            };
        }
    }
}