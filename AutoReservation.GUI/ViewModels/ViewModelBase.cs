using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Extensions;
using AutoReservation.Common.Interfaces;
using AutoReservation.GUI.Factory;

namespace AutoReservation.GUI.ViewModels
{
    public abstract class ViewModelBase : IExtendedNotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IServiceFactory factory;

        protected IAutoReservationService service { get; private set; }

        protected abstract void load();

        private string _errorMessage;
        public string errorMessage {
            get { return _errorMessage; }
            set {
                if (_errorMessage == value) {
                    return;
                }
                _errorMessage = value;
                this.OnPropertyChanged(p => p.errorMessage);
            }
        }

        protected ViewModelBase(IServiceFactory factory) {
            //ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            //target = channelFactory.CreateChannel();
            this.factory = factory;
        }

        protected ViewModelBase() {
        }

        public void init() {
            service = factory.GetService();
            load();
        }

        protected bool validate(IEnumerable<IValidatable> values) {
            var errorMessage = new StringBuilder();
            foreach(var v in values) {
                var error = v.validate();
                if (!string.IsNullOrEmpty(error)) {
                    errorMessage.AppendLine(v.ToString());
                    errorMessage.AppendLine(error);
                }
            }
            _errorMessage = errorMessage.ToString();
            return string.IsNullOrEmpty(_errorMessage);
        }
        

        void IExtendedNotifyPropertyChanged.OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        public bool serviceExist {
            get { return service != null; }
        }

        
    }
}
