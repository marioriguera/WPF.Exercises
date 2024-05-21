using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }
    }
}
