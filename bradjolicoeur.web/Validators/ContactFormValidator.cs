using bradjolicoeur.web.ViewModels;
using FluentValidation;

namespace bradjolicoeur.web.Validators;

public class ContactFormValidator : AbstractValidator<ContactFormModel>
{
    public ContactFormValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
    }
}
