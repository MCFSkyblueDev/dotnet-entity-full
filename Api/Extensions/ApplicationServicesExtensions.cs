using Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


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

     
            var coreAssembly = Assembly.Load("Core");
            services.AddValidatorsFromAssembly(coreAssembly!);
            //* Apply all Mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}