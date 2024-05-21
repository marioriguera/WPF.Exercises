using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Navigation.Models
{
    public class ObservableObjectModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Subscribe for property changed events.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Called by Set accessor of each property that needs to notify it's value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property it's value changed.</param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
