using FluentValidation;
using LojaVirtual.Entities.Models.Dto.Request;

namespace LojaVirtual.API.Models.Validators
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");
        }
    }
}