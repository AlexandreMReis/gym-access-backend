using GymAccessBackend.Core.Interfaces;

namespace GymAccessBackend.Infrastructure.Services
{
    public class StubPaymentService : IPaymentService
    {
        public Task<bool> ProcessPaymentAsync(string cardNumber, string cardHolder, decimal value)
        {
            // Stubbed service for local development.
            return Task.FromResult(true);
        }
    }
}
