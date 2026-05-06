using FluentValidation;
using LojaVirtual.Entities.Models.Dto.Request;

namespace LojaVirtual.API.Models.Validators
{
    public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
