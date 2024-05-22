using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Wpf.Navigation.Dependencies;
using Wpf.Navigation.Views.Windows;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.Configure<HostOptions>(options =>
                    {
                        options.ShutdownTimeout = TimeSpan.FromSeconds(30); // Wait 30 seconds on gracefull shutdown.
                        options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost; // stop host on hosted services exception.
                    });

                    Register.AddDependencies(services);
                    Register.AddWorkers(services);
                }).Build();

            _host.RunAsync();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _host.Services.GetRequiredService<MainWindowsViewModel>();
            mainWindow.Show();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
