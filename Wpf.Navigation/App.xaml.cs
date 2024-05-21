using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Wpf.Navigation.Services;
using Wpf.Navigation.Views.Windows;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // Dependencies injection
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowsViewModel>(),
            });
            services.AddSingleton<MainWindowsViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<UsersViewModels>();
            services.AddSingleton<NavigationService>();

            services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindows = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindows.Show();
            base.OnStartup(e);
        }
    }
}
