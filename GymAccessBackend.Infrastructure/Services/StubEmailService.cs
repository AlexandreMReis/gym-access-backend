using GymAccessBackend.Core.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

namespace GymAccessBackend.Infrastructure.Services
{
    public class StubEmailService : IEmailService
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body, byte[] inlineImageBytes = null, string inlineImageContentId = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Gym Access", "noreply@gymaccess.com"));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };

                if (inlineImageBytes != null && inlineImageBytes.Length > 0)
                {
                    var contentId = inlineImageContentId ?? MimeUtils.GenerateMessageId();
                    var image = builder.LinkedResources.Add("qrcode.png", inlineImageBytes);
                    image.ContentId = contentId;
                    image.ContentType.MediaType = "image";
                    image.ContentType.MediaSubtype = "png";
                    image.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                    if (inlineImageContentId == null)
                    {
                        builder.HtmlBody += $"<br/><img src='cid:{contentId}' alt='QR Code' />";
                    }
                }

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    // Gmail SMTP settings (replace with your credentials)
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("alexmmreis@gmail.com", "replace_with_app_password");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch
            {
                // TODO: Log error
                return false;
            }
        }
    }
}
