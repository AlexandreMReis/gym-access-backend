using GymAccessBackend.Core.Enums;
using GymAccessBackend.Core.Interfaces;
using GymAccessBackend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GymAccessBackend.Infrastructure.Repositories.SQLite
{
    public class SqlLiteReservationRepository : IReservationRepository
    {
        private SqlLiteReservationContext _db;
        public SqlLiteReservationRepository()
        {
            _db = new SqlLiteReservationContext();
        }
        public async Task<ReservationModel> GetReservationByCustomerEmailAsync(string email)
        {
            return await _db.Reservations.FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task<bool> SaveEmailSentByReservationIdAsync(int reservationId)
        {
            _db.Reservations
                .Where(r => r.Id == reservationId)
                .ExecuteUpdate(r => r
                    .SetProperty(reservation => reservation.EmailSent, true)
                    .SetProperty(reservation => reservation.UpdatedAt, DateTime.UtcNow));

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<int?> SaveReservationAsync(ReservationModel reservation)
        {
            reservation.Id = _db.Reservations.Count() + 1;
            reservation.CreatedAt = DateTime.UtcNow;

            _db.Reservations.Add(reservation);

            var result = await _db.SaveChangesAsync();

            return result > 0 ? reservation.Id : (int?)null;
        }

        public async Task<bool> UpdateReservationStatusAndQrCodeAsync(int reservationId, ReservationStatus status, string qrCode, string updatedBy)
        {
            _db.Reservations
                .Where(r => r.Id == reservationId)
                .ExecuteUpdate(r => r
                    .SetProperty(reservation => reservation.Status, status)
                    .SetProperty(reservation => reservation.QrCodeToken, qrCode)
                    .SetProperty(reservation => reservation.UpdatedBy, updatedBy)
                    .SetProperty(reservation => reservation.UpdatedAt, DateTime.UtcNow));

            return await _db.SaveChangesAsync() > 0;
        }
    }
}
