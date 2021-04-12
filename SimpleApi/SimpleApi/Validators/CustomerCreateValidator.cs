using FluentValidation;
using SimpleApi.DataTransfer.CustomersDto;

namespace SimpleApi.Validators
{
    public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateValidator()
        {
            this.RuleFor(x => x.PhoneNumber)
                .NotEmpty();

            this.RuleFor(x => x.Email)
                .NotEmpty();

            this.RuleFor(x => x.FullName)
                .NotEmpty();
        }
    }
}
