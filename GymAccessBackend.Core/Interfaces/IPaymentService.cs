namespace GymAccessBackend.Core.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Processes a payment request.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="cardHolder">The card holder's name.</param>
        /// <param name="value">The value of the reservation in euros.</param>
        /// <returns>A task that represents the asynchronous operation, containing the reservation model.</returns>
        Task<bool> ProcessPaymentAsync(string cardNumber, string cardHolder, decimal value);
    }
}
