using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    public class UsersViewModel : BaseViewModel
    {
        public UsersViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
