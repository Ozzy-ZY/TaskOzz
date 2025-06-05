using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class AddTaskValidator:AbstractValidator<AddTaskRequest>
{
    public AddTaskValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .MaximumLength(256);
        
        RuleFor(t => t.Description)
            .NotEmpty()
            .MaximumLength(4096);

        RuleFor(t => t.Priority)
            .NotEmpty().MaximumLength(64);
        
    }
}