using Wpf.Navigation.Commands;
using Wpf.Navigation.Models;
using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    public abstract class BaseViewModel : ObservableObjectModel
    {
        private INavigationService _navigation;

        public BaseViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;

            HomeNavigateCommand = new RelayCommand<object>(HomeNavigationCanExecute, HomeNavigationFunction);
            SettingsNavigateCommand = new RelayCommand<object>(SettingsNavigationCanExecute, SettingsNavigationFunction);
            UsersNavigateCommand = new RelayCommand<object>(UsersNavigationCanExecute, UsersNavigationFunction);
        }

        public RelayCommand<object> HomeNavigateCommand { get; init; }
        public RelayCommand<object> SettingsNavigateCommand { get; init; }
        public RelayCommand<object> UsersNavigateCommand { get; init; }

        private void UsersNavigationFunction(object obj)
        {
            Navigation.NavigateTo<UsersViewModels>();
        }

        private bool UsersNavigationCanExecute(object obj)
        {
            return true;
        }

        private void SettingsNavigationFunction(object obj)
        {
            Navigation.NavigateTo<SettingsViewModel>();
        }

        private bool SettingsNavigationCanExecute(object obj)
        {
            return true;
        }

        private bool HomeNavigationCanExecute(object obj)
        {
            return true;
        }

        private void HomeNavigationFunction(object obj)
        {
            Navigation.NavigateTo<HomeViewModel>();
        }

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                NotifyPropertyChanged(nameof(Navigation));
            }
        }
    }
}
