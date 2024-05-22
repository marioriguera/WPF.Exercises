using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Navigation.Services;
using Wpf.Navigation.Services.Workers.Home;
using Wpf.Navigation.Views.Pages;
using Wpf.Navigation.Views.Windows;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Dependencies
{
    /// <summary>
    /// Contains extension methods for registering dependencies in the IServiceCollection.
    /// </summary>
    public static class Register
    {
        /// <summary>
        /// Configures the host options for graceful shutdown and exception handling.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the host options to.</param>
        /// <returns>The <see cref="IServiceCollection"/> with host options configured.</returns>
        public static IServiceCollection AddHostOptions(this IServiceCollection services)
        {
            services.Configure<HostOptions>(options =>
            {
                options.ShutdownTimeout = TimeSpan.FromSeconds(30); // Wait 30 seconds on gracefull shutdown.
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost; // stop host on hosted services exception.
            });

            return services;
        }

        /// <summary>
        /// Adds the application's dependencies to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the dependencies to.</param>
        /// <returns>The IServiceCollection with the registered dependencies.</returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
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

            return services;
        }

        /// <summary>
        /// Adds hosted services (background workers) to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the hosted services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> with hosted services added.</returns>
        public static IServiceCollection AddWorkers(this IServiceCollection services)
        {
            services.AddHostedService<HomeWorker>();
            return services;
        }
    }
}
