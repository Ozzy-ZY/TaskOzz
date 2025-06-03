using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class LoginValidator :AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(l=> l.Email)
            .EmailAddress()
            .MaximumLength(256)
            .WithMessage("Must be a Valid Email");
        
        RuleFor(l => l.Password)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,256}$")
            .WithMessage("Must Contain 8 Characters or more, One Uppercase, One Lowercase, One Number and One Special Character");
    }
}