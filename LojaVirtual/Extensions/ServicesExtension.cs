using LojaVirtual.Services.Application;

namespace LojaVirtual.Extensions
{
    public static class ServicesExtension
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection ApplyServiceConfiguration(IConfiguration configuration)
            {
                services.AddControllersWithViews();

                services.AddHttpClient();
                services.AddTransient<IApplicationFactory, ApplicationFactory>();

                return services;
            }
        }
    }
}
