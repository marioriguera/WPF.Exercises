using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wpf.Navigation.Models
{
    /// <summary>
    /// Represents a model that notifies clients that a property value has changed.
    /// </summary>
    public class ObservableObjectModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies subscribers that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
