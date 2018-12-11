using AutoReservation.Common.DataTransferObjects;
using AutoReservation.GUI.Factory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoReservation.Common.Extensions;
using AutoReservation.Common.Interfaces;
using System.ServiceModel;

namespace AutoReservation.GUI.ViewModels
{
    public class AutoViewModel : ViewModelBase
    {
        private List<AutoDto> originalAutos = new List<AutoDto>();
        private ObservableCollection<AutoDto> _autos;

        public AutoViewModel(IServiceFactory factory) : base(factory) { }
        public AutoViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            IAutoReservationService target = channelFactory.CreateChannel();
            autos = new ObservableCollection<AutoDto>(target.Autos());
        }

        public ObservableCollection<AutoDto> autos {
            get { return _autos; }
            set { _autos = value; }
        }

        private AutoDto _selectedAuto;
        public AutoDto selectedAuto {
            get { return _selectedAuto; }
            set {
                if (_selectedAuto == value) {
                    return;
                }
                _selectedAuto = value;
                this.OnPropertyChanged(p => p.selectedAuto);
            }
        }

        #region AddCar
        private CommandBaseClass _AddCarCommand;
        public ICommand AddCarCommand {
            get {
                return _AddCarCommand ?? (_AddCarCommand = new CommandBaseClass(param => New(), param => CanNew()));
            }
        }

        private void New() {
            autos.Add(new AutoDto());
        }

        private bool CanNew() {
            return serviceExist;
        }



        #endregion

        #region RemoveCar
        private CommandBaseClass _RemoveCarCommand;
        public ICommand RemoveCarCommand {
            get {
                return _RemoveCarCommand ?? (_RemoveCarCommand = new CommandBaseClass(param => remove(), param => canRemove()));
            }
        }
        private void remove() {
            service.Remove(selectedAuto);
            load();
        }
        private bool canRemove() {
            return serviceExist && selectedAuto != null && selectedAuto.Id != default(int);
        }

        #endregion

        #region Save
        private CommandBaseClass _SaveCommand;
        public ICommand SaveCommand {
            get {
                return _SaveCommand ?? (_SaveCommand = new CommandBaseClass(param => save(), param => canSave()));
            }
        }
        public void save() {
            foreach (var a in autos) {
                if (a.Id == default(int)) {
                    service.Create(a);
                } else {
                    var original = originalAutos.FirstOrDefault(o => o.Id == a.Id);
                    service.Update(a);
                }
            }
            load();
        }

        private bool canSave() {
            if (!serviceExist) {
                return false;
            }
            return validate(autos);
        }

        #endregion

        #region Load

        private CommandBaseClass _LoadCarsCommand;
        public ICommand LoadCarsCommand {
            get {
                return _LoadCarsCommand ?? (_LoadCarsCommand = new CommandBaseClass(param => load(), param => canLoadAuto()));
            }
        }
        protected override void load() {
            autos.Clear();
            originalAutos.Clear();
            foreach (var a in service.Autos()) { 
                autos.Add(a);
                originalAutos.Add(a.copy());
            }
            selectedAuto = autos.FirstOrDefault();
        }
        private bool canLoadAuto() {
            return serviceExist;
        }
        #endregion
    }
}
