namespace ColledgeDocument.Shared.Validators;

public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
{
    private const int MinPasswordLength = 8;

    public UpdatePasswordRequestValidator()
    {
        // Current Password
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Текущий пароль обязателен для заполнения!")
            .MinimumLength(MinPasswordLength).WithMessage($"Длина текущего пароля должна быть не менее {MinPasswordLength} символов!");

        // New Password
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Новый пароль обязателен для заполнения!")
            .MinimumLength(MinPasswordLength).WithMessage($"Новый пароль должен содержать минимум {MinPasswordLength} символов")
            .Matches("[A-Z]").WithMessage("Новый пароль должен содержать хотя бы одну заглавную букву!")
            .Matches("[a-z]").WithMessage("Новый пароль должен содержать хотя бы одну строчную букву!")
            .Matches("[0-9]").WithMessage("Новый пароль должен содержать хотя бы одну цифру!")
            .NotEqual(x => x.CurrentPassword).WithMessage("Новый пароль должен отличаться от текущего!");

        // Repeat New Password
        RuleFor(x => x.RepeatNewPassword)
            .NotEmpty().WithMessage("Повтор пароля обязателен для заполнения!")
            .Equal(x => x.NewPassword).WithMessage("Пароли не совпадают!");
    }
}
