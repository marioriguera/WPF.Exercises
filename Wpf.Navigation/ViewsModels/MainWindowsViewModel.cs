using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    public class MainWindowsViewModel : BaseViewModel
    {
        public MainWindowsViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
