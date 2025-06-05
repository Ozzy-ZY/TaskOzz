using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskValidator()
    {
        RuleFor(t => t.TaskId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(t => t.Title)
            .NotNull()
            .MaximumLength(256);

        RuleFor(t => t.Description).NotNull()
            .MaximumLength(4096);
        RuleFor(t => t.Priority)
            .NotNull()
            .MaximumLength(64);

        RuleFor(t => t.Status)
            .NotNull()
            .MaximumLength(64);
    }
} 