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
using AutoReservation.GUI.ViewModels;
using System.ServiceModel;
using AutoReservation.Common.Interfaces;

namespace KundeReservation.GUI.ViewModels
{
    public class KundeViewModel : ViewModelBase {
        private List<KundeDto> originalKunden = new List<KundeDto>();
        private ObservableCollection<KundeDto> _Kunden = new ObservableCollection<KundeDto>();
        
        public RelayCommand EditKundeCommand { get; set; }
        public RelayCommand AddKundeCommand { get; set; }
        public RelayCommand RemoveKundeCommand { get; set; }

        public KundeViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            IAutoReservationService target = channelFactory.CreateChannel();
            Kunden = new ObservableCollection<KundeDto>(target.Kunden());

            EditKundeCommand = new RelayCommand(() => this.EditKunde(), () => this.CanEdit);
            AddKundeCommand = new RelayCommand(() => this.AddKunde(), () => this.CanAdd);
            RemoveKundeCommand = new RelayCommand(() => this.RemoveKunde(), () => this.CanRemove);
        }

        public bool CanAdd = true;
        private void AddKunde() {
            EditKundeViewModel editKundeVM = new EditKundeViewModel();
        }
        public bool CanEdit = true;
        private void EditKunde() {
            EditKundeViewModel editKundeVM = new EditKundeViewModel();
        }
        public bool CanRemove = true;
        private void RemoveKunde() {
            
        }


        public ObservableCollection<KundeDto> Kunden {
            get { return _Kunden; }
            set { _Kunden = value; }
        }

        private KundeDto _selectedKunde;
        public KundeDto selectedKunde {
            get { return _selectedKunde; }
            set {
                if (_selectedKunde == value) {
                    return;
                }
                _selectedKunde = value;
                this.OnPropertyChanged(p => p.selectedKunde);
            }
        }


        
        /*
        #region AddKunde
        private CommandBaseClass _AddKundeCommand;
        public ICommand AddKundeCommand {
            get {
                return _AddKundeCommand ?? (_AddKundeCommand = new CommandBaseClass(param => New(), param => CanNew()));
            }
        }

        private void New() {
            Kunden.Add(new KundeDto());
        }

        private bool CanNew() {
            return serviceExist;
        }



        #endregion

        #region RemoveKunde
        private CommandBaseClass _RemoveKundeCommand;
        public ICommand RemoveKundeCommand {
            get {
                return _RemoveKundeCommand ?? (_RemoveKundeCommand = new CommandBaseClass(param => remove(), param => canRemove()));
            }
        }
        private void remove() {
            service.RemoveKunde(selectedKunde);
            load();
        }
        private bool canRemove() {
            return serviceExist && selectedKunde != null && selectedKunde.Id != default(int);
        }

        #endregion

        #region SaveKunde
        private CommandBaseClass _SaveKundeCommand;
        public ICommand SaveKundeCommand {
            get {
                return _SaveKundeCommand ?? (_SaveKundeCommand = new CommandBaseClass(param => save(), param => canSave()));
            }
        }
        public void save() {
            foreach (var k in Kunden) {
                if (k.Id == default(int)) {
                    service.CreateKunde(k);
                }
                else {
                    var original = originalKunden.FirstOrDefault(o => o.Id == k.Id);
                    service.UpdateKunde(k);
                }
            }
            load();
        }

        private bool canSave() {
            if (!serviceExist) {
                return false;
            }
            return validate(Kunden);
        }

        #endregion

        #region LoadKunde

        private CommandBaseClass _LoadKundeCommand;
        public ICommand LoadKundeCommand {
            get {
                return _LoadKundeCommand ?? (_LoadKundeCommand = new CommandBaseClass(param => load(), param => canLoadKunde()));
            }
        }
        protected override void load() {
            Kunden.Clear();
            originalKunden.Clear();
            foreach (var a in service.Kunden()) { // TODO: implementieren in KundeReservationService
                Kunden.Add(a);
                originalKunden.Add(a.copy());
            }
            selectedKunde = Kunden.FirstOrDefault();
        }
        private bool canLoadKunde() {
            return serviceExist;
        }
        #endregion
        */
    }
}
