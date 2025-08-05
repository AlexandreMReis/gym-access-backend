using GymAccessBackend.Core.Enums;

namespace GymAccessBackend.Core.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public string Email { get; set; }
        public DateTime DayRequested { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string QrCodeToken { get; set; }
        public ReservationStatus Status { get; set; }
        public bool EmailSent { get; set; } = false;
    }
}
