using GymAccessBackend.Core.Enums;
using GymAccessBackend.Core.Models;

namespace GymAccessBackend.Core.Interfaces
{
    public interface IReservationRepository
    {
        /// <summary>
        /// Saves a reservation to the database.
        /// </summary>
        /// <param name="reservation">The reservation to save.</param>
        /// <returns>A task that represents the asynchronous operation, returning the resercation if created; otherwise, null.</returns>
        Task<int?> SaveReservationAsync(ReservationModel reservation);

        /// <summary>
        /// Marks the reservation as having its email sent, based on the reservation ID.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the update was successful; otherwise, false.</returns>
        Task<bool> SaveEmailSentByReservationIdAsync(int reservationId);

        /// <summary>
        /// Updates the status of a reservation.
        /// </summary>
        /// <param name="reservationId">The unique identifier of the reservation.</param>
        /// <param name="status">The new status to set for the reservation.</param>
        /// <param name="qrCode">The new qrCode to set for the reservation.</param>
        /// <param name="updatedBy">The user or system that performed the update.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateReservationStatusAndQrCodeAsync(int reservationId, ReservationStatus status, string qrCode, string updatedBy);

        /// <summary>
        /// Retrieves a reservation by its customer email.
        /// </summary>
        /// <param name="email">The email of the customer.</param>
        /// <returns>A task that represents the asynchronous operation, containing the reservation model.</returns>
        Task<ReservationModel> GetReservationByCustomerEmailAsync(string email);
    }
}
