using GymAccessBackend.Core.Models;

namespace GymAccessBackend.Core.Interfaces
{
    public interface IPurchaseLogic
    {
        /// <summary>
        /// Processes a payment request.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="cardHolder">The card holder's name.</param>
        /// <param name="email">The email address associated with the payment.</param>
        /// <param name="dayRequested">The date the payment is requested for.</param>
        Task<Result<string>> ProcessPurchaseAsync(string cardNumber, string cardHolder, string email, DateTime dayRequested);
    }
}
