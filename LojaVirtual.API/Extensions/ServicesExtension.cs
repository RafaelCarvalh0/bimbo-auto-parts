using FluentValidation;
using LojaVirtual.API.Models.Validators;
using LojaVirtual.Data.DatabaseContext;
using LojaVirtual.Data.Repositories;
using LojaVirtual.Entities.Models.Dto.Request;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.API.Extensions
{
    public static class ServicesExtension
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection ApplyServiceConfiguration(IConfiguration configuration)
            {
                // Database Context
                services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("MySql"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("MySql"))
                ));

                // Repositories
                services.AddScoped<ICategoryRepository, CategoryRepository>();
                services.AddScoped<IProductRepository, ProductRepository>();

                // Validators
                services.AddScoped<IValidator<CategoryCreateRequest>, CategoryCreateRequestValidator>();
                services.AddScoped<IValidator<CategoryUpdateRequest>, CategoryUpdateRequestValidator>();
                services.AddScoped<IValidator<ProductCreateRequest>, ProductCreateRequestValidator>();
                services.AddScoped<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();

                // API
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();

                return services;
            }
        }
    }
}
