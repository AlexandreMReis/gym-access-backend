namespace GymAccessBackend.WebAPI.Responses
{
    public class PurchaseResponse
    {
        /// <summary>
        /// Gets or sets the email address associated with the payment.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the date the gym train is reuested.
        /// </summary>
        public DateTime DayRequested { get; set; }

        /// <summary>
        /// Gets or sets the QR code token for the reservation.
        /// </summary>
        public string QrCodeToken { get; set; }
    }
}
