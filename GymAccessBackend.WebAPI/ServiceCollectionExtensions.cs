using GymAccessBackend.Core.Interfaces;
using GymAccessBackend.Core.Logic;
using GymAccessBackend.Infrastructure.Repositories.InMemory;
using GymAccessBackend.Infrastructure.Services;

namespace GymAccessBackend.WebAPI
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection) 
        {
            serviceCollection.AddScoped<IPurchaseLogic, PurchaseLogic>();

            return serviceCollection;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            // Add any infrastructure services here, e.g., database, email, payment external services, etc.
            serviceCollection.AddScoped<IReservationRepository, InMemoryReservationRepository>();
            serviceCollection.AddScoped<IEmailService, StubEmailService>();
            serviceCollection.AddScoped<IPaymentService, StubPaymentService>();

            return serviceCollection;
        }
    }
}
