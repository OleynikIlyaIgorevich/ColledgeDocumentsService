namespace ColledgeDocument.Shared.Validators;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    private const string UsernameRegex = @"^[a-zA-Z0-9_]+$";

    public TokenRequestValidator()
    {

        // Username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя обязательно для заполнения!")
            .MaximumLength(32).WithMessage("Длина логина не должна превышать 32 символов!")
            .Matches(UsernameRegex).WithMessage("Логин может содержать только латинские буквы, цифры и подчёркивания!")
            .Must(BeValidUsername).WithMessage("Логин не может состоять только из цифр!");

        // Password
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения!")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов!")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву!")
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву!")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру!");

    }

    private bool BeValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return true;
        return !username.All(char.IsDigit);
    }

}
