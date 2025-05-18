namespace ColledgeDocument.Shared.Validators;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название отделения не может быть пустым!")
            .MaximumLength(32).WithMessage("Название отделения не может превышать 32 символа");
    }
}
