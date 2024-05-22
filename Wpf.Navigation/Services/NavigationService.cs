using Microsoft.Extensions.Logging;
using Wpf.Navigation.Models;
using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Services
{
    /// <summary>
    /// Represents a navigation service that manages the current view model.
    /// </summary>
    public class NavigationService : ObservableObjectModel, INavigationService
    {
        private readonly ILogger _logger;
        private readonly Func<Type, BaseViewModel> _viewModelFactory;
        private BaseViewModel? _currentViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="viewModelFactory">A factory method to create view models.</param>
        public NavigationService(Func<Type, BaseViewModel> viewModelFactory, ILogger<NavigationService> logger)
        {
            _viewModelFactory = viewModelFactory;
            _logger = logger;
            _logger.LogInformation($"Log in {nameof(NavigationService)} works.");
        }

        /// <summary>
        /// Gets or sets the current view model.
        /// </summary>
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel!;
            private set
            {
                _currentViewModel = value;
                NotifyPropertyChanged(nameof(CurrentViewModel));
            }
        }

        /// <summary>
        /// Navigates to the specified view model type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model to navigate to.</typeparam>
        public void NavigateTo<TViewModel>()
            where TViewModel : class
        {
            BaseViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentViewModel = viewModel;
        }
    }
}
