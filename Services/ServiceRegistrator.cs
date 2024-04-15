using Microsoft.Extensions.DependencyInjection;


namespace WpfApp1.Services
{
    static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
           .AddTransient<IUserDialogService, WindowUserDialogService>()
        ;
    }
}
