using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1.ViewModels
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViews(this IServiceCollection services) => services
           .AddSingleton<MainWindowViewModel>()
           .AddTransient<QuestionEditViewModel>()
        ;
    }
}