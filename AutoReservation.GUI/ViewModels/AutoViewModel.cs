using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoReservation.Common.Interfaces;
using System.ServiceModel;
using System;
using System.Windows.Threading;
using AutoReservation.GUI.Views;

namespace AutoReservation.GUI.ViewModels {
    public class AutoViewModel : ViewModelBase
    {
        private List<AutoDto> originalAutos = new List<AutoDto>();
        private ObservableCollection<AutoDto> _Autos;

        private AutoDto auto;

        public int Id { get; set; }
        public String Marke { get; set; }
        public int Tagestarif { get; set; }
        public int Basistarif { get; set; }
        public AutoKlasse Klasse { get; set; }

        public RelayCommand EditCarCommand { get; set; }
        public RelayCommand AddCarCommand { get; set; }
        public RelayCommand RemoveCarCommand { get; set; }
        public RelayCommand ConfirmEditAutoCommand { get; set; }
        public RelayCommand DiscardButtonCommand { get; set; }

        EditAutoViewModel editAutoVM;

        private IAutoReservationService target;

        public DispatcherTimer Timer { get; set; }

        public AutoViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();
            Autos = new ObservableCollection<AutoDto>(target.Autos());

            EditCarCommand = new RelayCommand(() => this.EditCar(), () => true);
            AddCarCommand = new RelayCommand(() => this.AddCar(), () => true);
            RemoveCarCommand = new RelayCommand(() => this.RemoveCar(), () => true);
            ConfirmEditAutoCommand = new RelayCommand(() => this.ConfirmEdit(), () => true);
            DiscardButtonCommand = new RelayCommand(() => this.Discard(), () => true);
        }

        private void ConfirmEdit() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            auto = new AutoDto {
                Marke = Marke,
                Tagestarif = Tagestarif,
                Basistarif = Basistarif,
                AutoKlasse = Klasse
            };

            if (Id == 0) {
                Autos.Add(auto);
                target.CreateAuto(auto);
            } else {
                auto.Id = Id;
                Autos.Add(auto);
                target.UpdateAuto(auto);
            }
            resetAuto();
            editAutoVM.closeWindow();
        }
        

        private void Discard() {
            editAutoVM.closeWindow();
        }

        private void EditCar() {
            if (SelectedAuto != null) {
                setAuto(SelectedAuto); // Fill the editWindow with the Selected Auto
            }
            editAutoVM = new EditAutoViewModel();
            editAutoVM.setContext(this);
            this.Id = 20; // TODO: ID von Tabelle auswählen!!!!!!!!!!!!
        }
        
        private void AddCar() {
            editAutoVM = new EditAutoViewModel();
            editAutoVM.setContext(this);
        }
        
        private void RemoveCar() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            target.RemoveAuto(SelectedAuto);
            Autos.Remove(SelectedAuto);
        }

        public ObservableCollection<AutoDto> Autos {
            get { return _Autos; }
            set { _Autos = value; }
        }

        private AutoDto _selectedAuto;
        public AutoDto SelectedAuto {
            get { return _selectedAuto; }
            set { _selectedAuto = value; }
        }

        #region HelperMethods

        private void setAuto(AutoDto auto) {
            Id = auto.Id;
            Marke = auto.Marke;
            Tagestarif = auto.Tagestarif;
            Basistarif = auto.Basistarif;
            Klasse = auto.AutoKlasse;
        }

        private void resetAuto() {
            Id = 0;
            Marke = "";
            Tagestarif = 0;
            Basistarif = 0;
            Klasse = AutoKlasse.Standard;
        }
        #endregion


        /*
        #region AddCar
        private CommandBaseClass _AddCarCommand;
        public ICommand AddCarCommand {
            get {
                return _AddCarCommand ?? (_AddCarCommand = new CommandBaseClass(param => New(), param => CanNew()));
            }
        }

        private void New() {
            Autos.Add(new AutoDto());
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
            service.RemoveAuto(selectedAuto);
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
            foreach (var a in Autos) {
                if (a.Id == default(int)) {
                    service.CreateAuto(a);
                } else {
                    var original = originalAutos.FirstOrDefault(o => o.Id == a.Id);
                    service.UpdateAuto(a);
                }
            }
            load();
        }

        private bool canSave() {
            if (!serviceExist) {
                return false;
            }
            return validate(Autos);
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
            Autos.Clear();
            originalAutos.Clear();
            foreach (var a in service.Autos()) { 
                Autos.Add(a);
                originalAutos.Add(a.copy());
            }
            selectedAuto = Autos.FirstOrDefault();
        }
        private bool canLoadAuto() {
            return serviceExist;
        }
        #endregion
    */
    }
}
