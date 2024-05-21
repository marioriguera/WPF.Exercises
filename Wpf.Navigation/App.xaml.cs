using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Wpf.Navigation.Services;
using Wpf.Navigation.Views.Pages;
using Wpf.Navigation.Views.Windows;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register services
                    services.AddSingleton<INavigationService, NavigationService>();

                    services.AddScoped(provider => new MainWindow
                    {
                        DataContext = provider.GetRequiredService<MainWindowsViewModel>(),
                    });

                    // Register view models
                    services.AddSingleton<MainWindowsViewModel>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<SettingsViewModel>();
                    services.AddSingleton<UsersViewModel>();

                    // Register views
                    services.AddTransient<MainWindow>();
                    services.AddTransient<Home>();
                    services.AddTransient<Settings>();
                    services.AddTransient<Users>();

                    // Register functions
                    services.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _host.Services.GetRequiredService<MainWindowsViewModel>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
