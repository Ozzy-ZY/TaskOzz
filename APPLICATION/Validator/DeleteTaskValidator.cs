using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class DeleteTaskValidator : AbstractValidator<DeleteTaskRequest>
{
    public DeleteTaskValidator()
    {
        RuleFor(t => t.TaskId)
            .NotEmpty()
            .GreaterThan(0);
    }
} 