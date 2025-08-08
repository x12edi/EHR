using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EHR.Application;
using EHR.Infrastructure;

namespace EHR.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // register Application layer types (services, validators, mappers)
            // e.g., services.AddScoped<IPatientService, PatientService>();
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infrastructure-specific DI (repositories, file storage adapters, identity)
            // e.g., services.AddScoped<IPatientRepository, PatientRepository>();
            return services;
        }
    }
}
