namespace PhoneBook.Validation
{
    using FluentValidation;
    using PhoneBook.Settings;
    using PhoneBook.ViewModels;

    public class PhoneValidator : AbstractValidator<PhoneViewModel>
    {
        public PhoneValidator()
        {
            this.RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage(ValidationMessages.AddressNull)
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.Address.Length)
                    .LessThanOrEqualTo(DataSettings.AddressMaxLength)
                    .WithMessage(ValidationMessages.AddressNull);
                });

            this.RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage(ValidationMessages.PhoneNumberFormat)
                .DependentRules(() =>
                {
                    this.RuleFor(x => x.PhoneNumber)
                        .Matches(@"^([\(]{1}[0-9]{3}[\)]{1}[ ]{1}[0-9]{3}[\-]{1}[0-9]{4})$")
                        .WithMessage(ValidationMessages.PhoneNumberFormat);
                });
        }
    }
}
