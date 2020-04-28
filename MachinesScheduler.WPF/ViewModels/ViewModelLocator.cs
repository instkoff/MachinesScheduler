using Microsoft.Extensions.DependencyInjection;

namespace MachinesScheduler.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();
    }
}
