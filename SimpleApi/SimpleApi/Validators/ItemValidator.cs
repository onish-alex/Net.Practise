using FluentValidation;
using SimpleApi.DataTransfer.ItemsDto;

namespace SimpleApi.Validators
{
    public class ItemValidator : AbstractValidator<ItemDto>
    {
        public ItemValidator()
        {
            this.Include(new ItemCreateValidator());
        }
    }
}
