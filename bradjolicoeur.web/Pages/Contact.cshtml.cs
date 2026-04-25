using System;
using System.Threading.Tasks;
using bradjolicoeur.web.Models;
using bradjolicoeur.web.Services;
using bradjolicoeur.web.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace bradjolicoeur.web.Pages;

public class ContactModel : PageModel
{
    private readonly IMailService _mailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IValidator<ContactFormModel> _validator;
    private readonly MailSettings _mailSettings;

    public ContactModel(
        IMailService mailService,
        IEmailTemplateService emailTemplateService,
        IValidator<ContactFormModel> validator,
        IOptions<MailSettings> mailSettings)
    {
        _mailService = mailService;
        _emailTemplateService = emailTemplateService;
        _validator = validator;
        _mailSettings = mailSettings.Value;
    }

    [BindProperty]
    public ContactFormModel Input { get; set; } = new();

    public bool Submitted { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var validation = await _validator.ValidateAsync(Input);
        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError($"Input.{error.PropertyName}", error.ErrorMessage);
            return Page();
        }

        var templateId = Guid.Parse(_mailSettings.ContactTemplateId);
        var (subject, htmlBody) = await _emailTemplateService.RenderEmailTemplateAsync(templateId, new
        {
            Input.Name,
            Input.Email,
            Input.Message
        });

        await _mailService.SendEmailAsync(_mailSettings.NotificationEmail, subject, htmlBody);

        var confirmationTemplateId = Guid.Parse(_mailSettings.ConfirmationTemplateId);
        var (confirmationSubject, confirmationHtmlBody) = await _emailTemplateService.RenderEmailTemplateAsync(confirmationTemplateId, new
        {
            Input.Name,
            Input.Email,
            Input.Message
        });
        await _mailService.SendEmailAsync(Input.Email, confirmationSubject, confirmationHtmlBody);

        Submitted = true;
        ModelState.Clear();
        Input = new ContactFormModel();
        return Page();
    }
}
