using Wpf.Navigation.Models;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Services
{
    public class NavigationService : ObservableObjectModel, INavigationService
    {
        private BaseViewModel _currentViewModel;
        private readonly Func<Type, BaseViewModel> _viewModelFactory;

        public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            BaseViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentViewModel = viewModel;
        }
    }
}
