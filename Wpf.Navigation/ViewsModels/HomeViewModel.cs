using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string _message;

        public HomeViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyPropertyChanged(nameof(Message));
            }
        }

        internal void UpdateMessage(string messaje)
        {
            RunOnUiThread(() =>
            {
                Message = messaje;
            });
        }
    }
}
