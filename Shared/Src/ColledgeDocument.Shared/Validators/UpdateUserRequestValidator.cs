namespace ColledgeDocument.Shared.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    private const string PhoneRegex = @"^\+?[0-9\s\-\(\)]{7,20}$";
    private const string NameRegex = @"^[\p{L}\-\s']+$";
    private const string UsernameRegex = @"^[a-zA-Z0-9_]+$";
    public UpdateUserRequestValidator()
    {
        // LastName
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна для заполнения!")
            .MaximumLength(32).WithMessage("Длина фамилии не должна превышать 32 символов!")
            .Matches(NameRegex).WithMessage("Фамилия содержит недопустимые символы!")
            .Must(BeValidNamePart).WithMessage("Фамилия не может состоять только из пробелов или дефисов!");

        // FirstName
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно для заполнения!")
            .MaximumLength(32).WithMessage($"Длина имени не должна превышать 32 символов!")
            .Matches(NameRegex).WithMessage("Имя содержит недопустимые символы!")
            .Must(BeValidNamePart).WithMessage("Имя не может состоять только из пробелов или дефисов!");

        // MiddleName
        RuleFor(x => x.MiddleName)
            .MaximumLength(32).WithMessage($"Длина отчества не должна превышать 32 символов!")
            .Matches(NameRegex).When(x => !string.IsNullOrEmpty(x.MiddleName))
            .WithMessage("Отчество содержит недопустимые символы!")
            .Must(BeValidNamePart).When(x => !string.IsNullOrEmpty(x.MiddleName))
            .WithMessage("Отчество не может состоять только из пробелов или дефисов!");

        // Phone
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Телефон обязателен для заполнения!")
            .MaximumLength(20).WithMessage("Длина телефона не должна превышать 20 символов!")
            .Matches(PhoneRegex).WithMessage("Неверный формат телефона!")
            .Must(BeDigitsOnly).When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Телефон должен содержать только цифры и допустимые символы!");

        // Username
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя обязательно для заполнения!")
            .MaximumLength(32).WithMessage("Длина логина не должна превышать 32 символов!")
            .Matches(UsernameRegex).WithMessage("Логин может содержать только латинские буквы, цифры и подчёркивания!")
            .Must(BeValidUsername).WithMessage("Логин не может состоять только из цифр!");

        // Password
        RuleFor(x => x.Password)
         ;

        // Password (conditional validation)
        When(x => x.IsChangePassword, () =>
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен при изменении пароля!")
                .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов!")
                .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву!")
                .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву!")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру!");
        });

        // RoleId
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Необходимо выбрать роль пользователя!");
    }
    private bool BeValidNamePart(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return true;
        return name.Trim().Length > 0 && name.Replace("-", "").Replace("'", "").Trim().Length > 0;
    }

    private bool BeDigitsOnly(string phone)
    {
        if (string.IsNullOrEmpty(phone)) return true;
        return phone.All(c => char.IsDigit(c) || c is '+' or '-' or '(' or ')' or ' ');
    }

    private bool BeValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return true;
        return !username.All(char.IsDigit);
    }
}
