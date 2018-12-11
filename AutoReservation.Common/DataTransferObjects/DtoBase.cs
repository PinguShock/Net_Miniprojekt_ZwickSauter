using AutoReservation.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.Common.DataTransferObjects {
    public abstract class DtoBase<T> : IExtendedNotifyPropertyChanged, ICopy<T>, IValidatable {

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract T copy();

        public void OnPropertyChanged(string propertyName) {
            if (PropertyChanged == null) {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract string validate();
    }
}
