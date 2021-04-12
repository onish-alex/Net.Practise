namespace PhoneBook.Validation
{
    using FluentValidation;
    using PhoneBook.ViewModels;

    public class CreatePhoneValidator : AbstractValidator<CreatePhoneViewModel>
    {
        public CreatePhoneValidator()
        {
            this.Include(new PhoneValidator());
        }
    }
}
