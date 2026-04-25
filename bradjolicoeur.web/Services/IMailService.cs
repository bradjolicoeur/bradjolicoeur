using System.Threading.Tasks;

namespace bradjolicoeur.web.Services;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlBody);
}
