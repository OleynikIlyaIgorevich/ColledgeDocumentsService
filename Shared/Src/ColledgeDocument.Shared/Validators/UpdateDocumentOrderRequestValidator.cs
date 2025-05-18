namespace ColledgeDocument.Shared.Validators;

public class UpdateDocumentOrderRequestValidator : AbstractValidator<UpdateDocumentOrderRequest>
{
    public UpdateDocumentOrderRequestValidator()
    {
        RuleFor(x => x.DocumentTypeId)
           .GreaterThan(0).WithMessage("Тип документа должен быть выбран!");

        RuleFor(x => x.DepartamnetId)
            .GreaterThan(0).WithMessage("Отделение должно быть выбрано!");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Количество справок должно быть больше 0 штук!")
            .LessThan(11).WithMessage("Количество справок не можеть превышать 10 штук!");
    }
}
