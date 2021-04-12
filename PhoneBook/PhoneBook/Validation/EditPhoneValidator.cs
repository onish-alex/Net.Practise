namespace PhoneBook.Validation
{
    using FluentValidation;
    using PhoneBook.ViewModels;

    public class EditPhoneValidator : AbstractValidator<EditPhoneViewModel>
    {
        public EditPhoneValidator()
        {
            this.Include(new PhoneValidator());
        }
    }
}
