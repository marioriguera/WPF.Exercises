using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    public class UsersViewModels : BaseViewModel
    {
        public UsersViewModels(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
