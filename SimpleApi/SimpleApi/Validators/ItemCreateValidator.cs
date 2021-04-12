using FluentValidation;
using SimpleApi.DataTransfer.ItemsDto;

namespace SimpleApi.Validators
{
    public class ItemCreateValidator : AbstractValidator<ItemCreateDto>
    {
        public ItemCreateValidator()
        {
            this.RuleFor(x => x.Name)
                .NotEmpty();

            this.RuleFor(x => x.Description)
                .NotEmpty();

            this.RuleFor(x => x.Cost)
                .GreaterThan(0);

            this.RuleFor(x => x.Manufacturer)
                .NotEmpty();

            this.RuleFor(x => x.Nds)
                .NotEmpty();

            this.RuleFor(x => x.Refrigerate)
                .NotNull();
        }
    }
}
