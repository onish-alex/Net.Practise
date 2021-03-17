using FluentValidation;
using SimpleApi.DataTransfer.DeliveriesDto;

namespace SimpleApi.Validators
{
    public class DeliveryValidator : AbstractValidator<DeliveryDto>
    {
        public DeliveryValidator()
        {
            this.Include(new DeliveryCreateValidator());
        }
    }
}
