using FluentValidation;
using SimpleApi.DataTransfer.DeliveriesDto;
using SimpleApi.Enumeration;

namespace SimpleApi.Validators
{
    public class DeliveryCreateValidator : AbstractValidator<DeliveryCreateDto>
    {
        public DeliveryCreateValidator()
        {
            this.RuleFor(x => x.Address)
                .NotEmpty();

            this.RuleFor(x => x.TypeDelivery)
                .IsInEnum();
        }
    }
}
