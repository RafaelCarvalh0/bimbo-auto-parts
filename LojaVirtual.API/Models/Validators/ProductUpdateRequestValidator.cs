using FluentValidation;
using LojaVirtual.Entities.Models.Dto.Request;

namespace LojaVirtual.API.Models.Validators
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .When(x => x.Price != null);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.")
                .When(x => x.CategoryId != null);
        }
    }
}