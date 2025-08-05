namespace GymAccessBackend.Core.Interfaces
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
