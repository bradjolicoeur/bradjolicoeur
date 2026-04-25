namespace bradjolicoeur.web.Models;

public class MailSettings
{
    public string FromEmail { get; set; } = string.Empty;
    public string NotificationEmail { get; set; } = string.Empty;
    public string ContactTemplateId { get; set; } = string.Empty;
    public string ConfirmationTemplateId { get; set; } = string.Empty;
}
