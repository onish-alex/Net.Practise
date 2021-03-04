namespace PhoneBook.Validation
{
    using FluentValidation;
    using PhoneBook.Settings;
    using PhoneBook.ViewModels;

    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            this.RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage(ValidationMessages.UserLoginNull)
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Login.Length)
                        .LessThanOrEqualTo(DataSettings.UserLoginMaxLength)
                        .WithMessage(ValidationMessages.UserLoginLength)
                        .GreaterThanOrEqualTo(DataSettings.UserLoginMinLength)
                        .WithMessage(ValidationMessages.UserLoginLength);
                });

            this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.UserPasswordNull)
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Password.Length)
                        .LessThanOrEqualTo(DataSettings.UserPasswordMaxLength)
                        .WithMessage(ValidationMessages.UserPasswordLength)
                        .GreaterThanOrEqualTo(DataSettings.UserPasswordMinLength)
                        .WithMessage(ValidationMessages.UserPasswordLength);

                    this.RuleFor(x => x.ConfirmPassword)
                       .NotEmpty()
                       .WithMessage(ValidationMessages.UserConfirmPasswordNotMatch)
                       .Equal(x => x.Password)
                       .WithMessage(ValidationMessages.UserConfirmPasswordNotMatch);
                });
        }
    }
}
