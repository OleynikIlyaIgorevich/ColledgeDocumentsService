namespace ColledgeDocument.Shared.Validators;

public class CreateDocumentTypeRequestValidator : AbstractValidator<CreateDocumentTypeRequest>
{
    public CreateDocumentTypeRequestValidator()
    {
        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Название типа справки не может быть пустым!")
          .MaximumLength(32).WithMessage("Название типа справки не может превышать 32 символа");
    }
}
