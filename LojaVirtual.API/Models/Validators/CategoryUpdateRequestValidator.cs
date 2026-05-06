using FluentValidation;
using LojaVirtual.Entities.Models.Dto.Request;

namespace LojaVirtual.API.Models.Validators
{
    public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
    {
        public CategoryUpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
