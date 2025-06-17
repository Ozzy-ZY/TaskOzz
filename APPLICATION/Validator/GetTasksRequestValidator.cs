using APPLICATION.DTOs;
using FluentValidation;

namespace APPLICATION.Validator;

public class GetTasksRequestValidator:AbstractValidator<GetTasksRequest>
{
    public GetTasksRequestValidator()
    {
        RuleFor(r => r.FilterKey)
            .MaximumLength(20)
            .WithMessage("Filter Key must be less than 20 characters");
        RuleFor(r => r.SortKey).MaximumLength(20).WithMessage("Sort Key must be less than 20 characters");
        RuleFor(r => r.SortOrder).MaximumLength(10).WithMessage("type asc or desc only");
    }
}