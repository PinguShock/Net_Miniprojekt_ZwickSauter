using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoReservation.Common.Extensions;

namespace AutoReservation.GUI.ViewModels
{
    class ViewModelBase : IExtendedNotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void IExtendedNotifyPropertyChanged.OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
