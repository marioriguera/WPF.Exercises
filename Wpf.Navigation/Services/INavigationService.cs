using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Services
{
    /// <summary>
    /// Provides navigation functionality between different view models.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets the current view model.
        /// </summary>
        BaseViewModel CurrentViewModel { get; }

        /// <summary>
        /// Navigates to the specified view model type.
        /// </summary>
        /// <typeparam name="T">The type of the view model to navigate to.</typeparam>
        void NavigateTo<T>()
            where T : class;
    }
}
