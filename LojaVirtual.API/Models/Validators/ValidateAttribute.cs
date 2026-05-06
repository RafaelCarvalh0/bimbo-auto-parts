using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LojaVirtual.API.Models.Validators
{
    public class ValidateAttribute<T> : ActionFilterAttribute where T : class
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
            var dto = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (dto is not null)
            {
                var result = await validator.ValidateAsync(dto);
                if (!result.IsValid)
                {
                    context.Result = new BadRequestObjectResult(
                        result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    );
                    return;
                }
            }

            await next();
        }
    }
}
