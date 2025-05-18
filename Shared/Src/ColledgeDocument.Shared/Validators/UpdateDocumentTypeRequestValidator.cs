namespace ColledgeDocument.Shared.Validators;

public class UpdateDocumentTypeRequestValidator : AbstractValidator<UpdateDocumentTypeRequest>
{
    public UpdateDocumentTypeRequestValidator()
    {
        RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Название типа справки не может быть пустым!")
          .MaximumLength(32).WithMessage("Название типа справки не может превышать 32 символа");
    }
}
