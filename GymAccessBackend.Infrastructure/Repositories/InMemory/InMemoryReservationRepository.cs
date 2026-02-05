using GymAccessBackend.Core.Enums;
using GymAccessBackend.Core.Interfaces;
using GymAccessBackend.Core.Models;
using System.Collections.Concurrent;

namespace GymAccessBackend.Infrastructure.Repositories.InMemory
{
    public class InMemoryReservationRepository : IReservationRepository
    {
        private static readonly ConcurrentDictionary<int, ReservationModel> Reservations = new();
        private static int _nextId = 1;

        public Task<ReservationModel> GetReservationByCustomerEmailAsync(string email)
        {
            var result = Reservations.Values.FirstOrDefault(r => r.Email == email);
            return Task.FromResult(result);
        }

        public Task<bool> SaveEmailSentByReservationIdAsync(int reservationId)
        {
            if (Reservations.TryGetValue(reservationId, out var reservation))
            {
                reservation.EmailSent = true;
                reservation.UpdatedAt = DateTime.UtcNow;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<int?> SaveReservationAsync(ReservationModel reservation)
        {
            if (reservation == null)
            {
                return Task.FromResult<int?>(null);
            }

            var id = Interlocked.Increment(ref _nextId);
            reservation.Id = id;
            reservation.CreatedAt = DateTime.UtcNow;

            if (Reservations.TryAdd(id, reservation))
            {
                return Task.FromResult<int?>(id);
            }
            return Task.FromResult<int?>(null);
        }

        public Task<bool> UpdateReservationStatusAndQrCodeAsync(
            int reservationId,
            ReservationStatus status,
            string qrCode,
            string updatedBy)
        {
            if (Reservations.TryGetValue(reservationId, out var existing))
            {
                existing.Status = status;
                existing.QrCodeToken = qrCode;
                existing.UpdatedBy = updatedBy;
                existing.UpdatedAt = DateTime.UtcNow;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
