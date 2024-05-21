using Wpf.Navigation.ViewsModels;

namespace Wpf.Navigation.Services
{
    public interface INavigationService
    {
        BaseViewModel CurrentViewModel { get; }
        void NavigateTo<T>() where T : class;
    }
}
