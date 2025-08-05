namespace GymAccessBackend.WebAPI.Request
{
    public class PurchaseRequest
    {
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public string Email { get; set; }
        public DateTime? DayRequested { get; set; }
    }
}
