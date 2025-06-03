using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class RegisterValidator:AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(r => r.Email)
            .EmailAddress()
            .MaximumLength(256)
            .WithMessage("Must be a Valid Email");
        
        RuleFor(r => r.UserName)
            .MinimumLength(3)
            .MaximumLength(256);
        
        RuleFor(r => r.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,256}$")
            .WithMessage("Must Contain 8 Characters or more, One Uppercase, One Lowercase, One Number and One Special Character");
    }
}