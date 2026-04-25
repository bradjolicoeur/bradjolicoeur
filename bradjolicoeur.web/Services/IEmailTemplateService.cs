using System;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Services;

public interface IEmailTemplateService
{
    Task<(string subject, string htmlBody)> RenderEmailTemplateAsync(Guid templateId, object model);
}
