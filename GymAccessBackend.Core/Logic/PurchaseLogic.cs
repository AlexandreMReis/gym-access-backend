using GymAccessBackend.Core.Enums;
using GymAccessBackend.Core.Interfaces;
using GymAccessBackend.Core.Models;
using QRCoder;

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
                    return new Result<string>("Failed to save reservation.", isSuccess: false);
                }

                byte[] qrCodeBytes = await GenerateQrCodeBytesAsync(email);
                if (qrCodeBytes == null || qrCodeBytes.Length == 0)
                {
                    return new Result<string>("QR code generation failed.", isSuccess: false);
                }
                var qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                var success = await _paymentService.ProcessPaymentAsync(cardNumber, cardHolder, 15.0m);
                if (!success)
                {
                    return new Result<string>("Payment processing failed.", isSuccess: false);
                }

                success = await _reservationRepository.UpdateReservationStatusAndQrCodeAsync(
                    reservationId.GetValueOrDefault(), 
                    ReservationStatus.Confirmed, 
                    qrCodeBase64, 
                    "System"
                );
                if (!success)
                {
                    return new Result<string>($"Failed to update reservation status and qr code.", isSuccess: false);
                }

                const string qrCodeContentId = "reservation-qr-code";
                var htmlBody = $"<html><body><p>Your reservation for {dayRequested.ToShortDateString()} has been confirmed.</p><p>Your QR code:</p><img src='cid:{qrCodeContentId}' alt='QR Code' /></body></html>";
                success = await _emailService.SendEmailAsync(
                    email,
                    "Reservation Confirmation",
                    htmlBody,
                    qrCodeBytes,
                    qrCodeContentId);
                if (!success)
                {
                    return new Result<string>("Failed to save reservation on database.", isSuccess: false);
                }

                await _reservationRepository.SaveEmailSentByReservationIdAsync(reservationId.GetValueOrDefault());

                return new Result<string>(qrCodeBase64);
            }
            catch (Exception ex)
            {
                return new Result<string>($"An error occurred while processing the purchase: {ex.Message}", isSuccess: false);
            }
        }

        private async Task<byte[]> GenerateQrCodeBytesAsync(string email)
        {
            try
            {
                await Task.Yield();
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(email, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
            catch (Exception)
            {
                // TODO log error to file
                return Array.Empty<byte>();
            }
        }
    }
}
