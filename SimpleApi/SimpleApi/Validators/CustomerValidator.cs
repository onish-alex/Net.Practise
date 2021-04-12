using FluentValidation;
using SimpleApi.DataTransfer.CustomersDto;

namespace SimpleApi.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            this.Include(new CustomerCreateValidator());
        }
    }
}
