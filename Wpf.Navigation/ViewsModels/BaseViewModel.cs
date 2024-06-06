using System.Windows;
using Wpf.Navigation.Commands;
using Wpf.Navigation.Models;
using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    /// <summary>
    /// Base class for all ViewModel classes in the application, providing common functionality.
    /// </summary>
    public abstract class BaseViewModel : ObservableObjectModel
    {
        private INavigationService? _navigation;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class with the specified navigation service.
        /// </summary>
        /// <param name="navigationService">The navigation service used for view model navigation.</param>
        public BaseViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;

            HomeNavigateCommand = new RelayCommand<object>(HomeNavigationCanExecute, HomeNavigationFunction);
            SettingsNavigateCommand = new RelayCommand<object>(SettingsNavigationCanExecute, SettingsNavigationFunction);
            UsersNavigateCommand = new RelayCommand<object>(UsersNavigationCanExecute, UsersNavigationFunction);
        }

        /// <summary>
        /// Gets or sets the navigation service used for view model navigation.
        /// </summary>
        public INavigationService? Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                NotifyPropertyChanged(nameof(Navigation));
            }
        }

        /// <summary>
        /// Gets the command to navigate to the Home view.
        /// </summary>
        public RelayCommand<object> HomeNavigateCommand { get; init; }

        /// <summary>
        /// Gets the command to navigate to the Settings view.
        /// </summary>
        public RelayCommand<object> SettingsNavigateCommand { get; init; }

        /// <summary>
        /// Gets the command to navigate to the Users view.
        /// </summary>
        public RelayCommand<object> UsersNavigateCommand { get; init; }

        /// <summary>
        /// Executes the specified action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        protected static void RunOnUiThread(Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }

        /// <summary>
        /// Navigates to the Users view model.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void UsersNavigationFunction(object obj)
        {
            Navigation?.NavigateTo<UsersViewModel>();
        }

        /// <summary>
        /// Determines whether the Users navigation command can execute.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        /// <returns><c>true</c> if the Users navigation command can execute; otherwise, <c>false</c>.</returns>
        private bool UsersNavigationCanExecute(object obj)
        {
            return true;
        }

        /// <summary>
        /// Navigates to the Settings view model.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void SettingsNavigationFunction(object obj)
        {
            Navigation?.NavigateTo<SettingsViewModel>();
        }

        /// <summary>
        /// Determines whether the Settings navigation command can execute.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        /// <returns><c>true</c> if the Settings navigation command can execute; otherwise, <c>false</c>.</returns>
        private bool SettingsNavigationCanExecute(object obj)
        {
            return true;
        }

        /// <summary>
        /// Determines whether the Home navigation command can execute.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        /// <returns><c>true</c> if the Home navigation command can execute; otherwise, <c>false</c>.</returns>
        private bool HomeNavigationCanExecute(object obj)
        {
            return true;
        }

        /// <summary>
        /// Navigates to the Home view model.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void HomeNavigationFunction(object obj)
        {
            Navigation?.NavigateTo<HomeViewModel>();
        }
    }
}
