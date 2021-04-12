namespace PhoneBook.Validation
{
    using FluentValidation;
    using PhoneBook.ViewModels;

    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            this.RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage(ValidationMessages.UserLoginNull);

            this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.UserPasswordNull);
        }
    }
}
