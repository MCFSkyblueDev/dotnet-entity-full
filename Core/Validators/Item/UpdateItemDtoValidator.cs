using Core.Dtos.Item;
using FluentValidation;

namespace Core.Validators.Item
{
    public class UpdateItemDtoValidator : AbstractValidator<UpdateItemDto>
    {
        public UpdateItemDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Item name cannot exceed 100 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            
        }
    }
}