using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class ChangePasswordValidator:AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(p=>p.NewPassword)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,256}$")
            .WithMessage("Must Contain 8 Characters or more, One Uppercase, One Lowercase, One Number and One Special Character");
    }
}