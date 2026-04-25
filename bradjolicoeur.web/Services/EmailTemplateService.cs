using System;
using System.Threading.Tasks;
using bradjolicoeur.core.blastcms;
using LazyCache;
using Markdig;
using Microsoft.Extensions.Configuration;
using Stubble.Core.Builders;

namespace bradjolicoeur.web.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IBlastCMSClient _blastCmsClient;
    private readonly IAppCache _cache;
    private readonly string _apiKey;
    private readonly MarkdownPipeline _markdownPipeline;

    public EmailTemplateService(IBlastCMSClient blastCmsClient, IAppCache cache, IConfiguration configuration)
    {
        _blastCmsClient = blastCmsClient;
        _cache = cache;
        _apiKey = configuration["BlastCMSContentKey"] ?? string.Empty;
        _markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    }

    public async Task<(string subject, string htmlBody)> RenderEmailTemplateAsync(Guid templateId, object model)
    {
        var template = await _cache.GetOrAddAsync(
            $"email-template-{templateId}",
            async () => await _blastCmsClient.GetEmailTemplateAsync(templateId, _apiKey)
        );

        if (template == null)
            throw new InvalidOperationException($"Email template with ID {templateId} not found.");

        var stubble = new StubbleBuilder().Build();

        var renderedSubject = await stubble.RenderAsync(template.Subject ?? string.Empty, model);
        var renderedBody = await stubble.RenderAsync(template.Body ?? string.Empty, model);
        var htmlBody = Markdown.ToHtml(renderedBody, _markdownPipeline);

        return (renderedSubject, htmlBody);
    }
}
