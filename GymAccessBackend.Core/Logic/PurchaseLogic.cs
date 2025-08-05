using GymAccessBackend.Core.Enums;
using GymAccessBackend.Core.Interfaces;
using GymAccessBackend.Core.Models;

namespace GymAccessBackend.Core.Logic
{
    public class PurchaseLogic : IPurchaseLogic
    {
    
        private readonly IReservationRepository _reservationRepository;
        private readonly IEmailService _emailService;
        private readonly IPaymentService _paymentService;

        public PurchaseLogic(IReservationRepository reservationRepository, IEmailService emailService, IPaymentService paymentService)
        {
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

        public async Task<Result<string>> ProcessPurchaseAsync(string cardNumber, string cardHolder, string email, DateTime dayRequested)
        {
            try
            {
                var reservationId = await _reservationRepository.SaveReservationAsync(new ReservationModel
                {
                    CardNumber = cardNumber,
                    CardHolder = cardHolder,
                    Email = email,
                    DayRequested = dayRequested,
                    CreatedBy = "System",
                    Status = ReservationStatus.Pending
                });
                if (reservationId == null)
                {
                    //TODO log error to file
                    return new Result<string>("Failed to save reservation.");
                }

                string qrCode = await GenerateQrCodeAsync(email);
                if (string.IsNullOrWhiteSpace(qrCode))
                {
                    //TODO log error to file
                    return new Result<string>("QR code generation failed.");
                }

                var success = await _paymentService.ProcessPaymentAsync(cardNumber, cardHolder, 15.0m);
                if (!success)
                {
                    //TODO log error to file
                    return new Result<string>("Payment processing failed.");
                }

                success = await _reservationRepository.UpdateReservationStatusAndQrCodeAsync(
                    reservationId.GetValueOrDefault(), 
                    ReservationStatus.Confirmed, 
                    qrCode, 
                    "System"
                );
                if (!success)
                {
                    //TODO log error to file
                    return new Result<string>($"Failed to update reservation status and qr code.");
                }

                success = await _emailService.SendEmailAsync(email, "Reservation Confirmation",
                    $"Your reservation for {dayRequested.ToShortDateString()} has been confirmed. Your QR code is: {qrCode}");
                if (!success)
                {
                    //TODO log error to file
                    return new Result<string>("Failed to save reservation on database.");
                }

                await _reservationRepository.SaveEmailSentByReservationIdAsync(reservationId.GetValueOrDefault());

                // For now, return a dummy success result to satisfy return requirements
                return new Result<string>(qrCode);
            }
            catch (Exception ex)
            {
                //TODO log error to file
                return new Result<string>($"An error occurred while processing the purchase: {ex.Message}");
            }
        }

        private async Task<string> GenerateQrCodeAsync(string email)
        {
            try
            {
                //TODO create qr code based on the email and utcnow ?
                await Task.Yield();
                throw new NotImplementedException("QR code generation logic is not implemented yet.");
            }
            catch (Exception)
            {
                //TODO log error to file
                return string.Empty;
            }
        }
    }
}
