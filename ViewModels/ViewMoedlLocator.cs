using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public QuestionEditViewModel QuestionEditViewModel => App.Services.GetRequiredService<QuestionEditViewModel>();
    }
}
