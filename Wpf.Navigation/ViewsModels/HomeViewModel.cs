using Wpf.Navigation.Services;

namespace Wpf.Navigation.ViewsModels
{
    /// <summary>
    /// ViewModel for the Home view.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        private string _message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        public HomeViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        /// <summary>
        /// Gets or sets the message to be displayed.
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyPropertyChanged(nameof(Message));
            }
        }

        /// <summary>
        /// Updates the message with the provided value.
        /// </summary>
        /// <param name="message">The new message.</param>
        internal void UpdateMessage(string message)
        {
            RunOnUiThread(() =>
            {
                Message = message;
            });
        }
    }
}
