using System.Threading.Tasks;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using bradjolicoeur.web.Models;
using Microsoft.Extensions.Options;

namespace bradjolicoeur.web.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailConfig;
    private readonly IAmazonSimpleEmailServiceV2 _emailService;

    public MailService(IOptions<MailSettings> mailConfig, IAmazonSimpleEmailServiceV2 emailService)
    {
        _mailConfig = mailConfig.Value;
        _emailService = emailService;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var request = new SendEmailRequest
        {
            FromEmailAddress = _mailConfig.FromEmail,
            Destination = new Destination { ToAddresses = [toEmail] },
            Content = new EmailContent
            {
                Simple = new Message
                {
                    Subject = new Content { Data = subject },
                    Body = new Body { Html = new Content { Data = htmlBody } }
                }
            }
        };
        await _emailService.SendEmailAsync(request);
    }
}
