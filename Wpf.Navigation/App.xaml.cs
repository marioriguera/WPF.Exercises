using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;
using Wpf.Navigation.Dependencies;
using Wpf.Navigation.Services.Configurations;
using Wpf.Navigation.Views.Windows;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Host field to share inside of this class.
        /// </summary>
        private readonly IHost? _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            AppConfigurationsService.UpdateLogger();
            Log.Fatal($"WPF Exercises initializing.");

            try
            {
                _host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        Register.AddHostOptions(services);
                        Register.AddDependencies(services);
                        Register.AddWorkers(services);
                    })
                    .UseSerilog()
                    .Build();

                Log.Fatal($"WPF Exercises going to start.");
                _host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Stopped program because of exception : {ex.Message}");
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _host!.Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _host.Services.GetRequiredService<MainWindowsViewModel>();
            mainWindow.Show();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            await _host!.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
