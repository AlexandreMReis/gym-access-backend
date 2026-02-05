namespace GymAccessBackend.Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string htmlBody, byte[] inlineImageBytes = null, string inlineImageContentId = null);
    }
}
