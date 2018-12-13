using AutoReservation.Common.DataTransferObjects;
using System.Collections.ObjectModel;
using AutoReservation.Common.Interfaces;
using System.ServiceModel;
using System;
using System.Windows;
using System.Linq;
using AutoReservation.Common.DataTransferObjects.Faults;

namespace AutoReservation.GUI.ViewModels {
    public class AutoViewModel : ViewModelBase
    {   
        private AutoDto auto;

        public int Id { get; set; }
        public String Marke { get; set; }
        public int Tagestarif { get; set; }
        public int Basistarif { get; set; }
        public AutoKlasse Klasse { get; set; }

        public RelayCommand EditAutoCommand { get; set; }
        public RelayCommand AddAutoCommand { get; set; }
        public RelayCommand RemoveAutoCommand { get; set; }
        public RelayCommand ConfirmEditAutoCommand { get; set; }
        public RelayCommand DiscardAutoButtonCommand { get; set; }

        EditAutoViewModel editAutoVM;

        private IAutoReservationService target;

        private bool buttonVisibility = true;

        public AutoViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            Autos = new ObservableCollection<AutoDto>(target.Autos());

            EditAutoCommand = new RelayCommand(() => this.EditAuto(), () => buttonVisibility);
            AddAutoCommand = new RelayCommand(() => this.AddAuto(), () => buttonVisibility);
            RemoveAutoCommand = new RelayCommand(() => this.RemoveAuto(), () => buttonVisibility);
            ConfirmEditAutoCommand = new RelayCommand(() => this.ConfirmEdit(), () => true);
            DiscardAutoButtonCommand = new RelayCommand(() => this.DiscardAutod(), () => true);
        }


        private ObservableCollection<AutoDto> _Autos;
        public ObservableCollection<AutoDto> Autos {
            get { return _Autos; }
            set { _Autos = value; }
        }

        private AutoDto _selectedAuto;
        public AutoDto SelectedAuto {
            get { return _selectedAuto; }
            set { _selectedAuto = value; }
        }

        #region CommandMethods
        private void ConfirmEdit() {
            
            if (Id != 0) { // Ask only when a Auto is edited, not if a new Auto is generated.
                MessageBoxResult result = showSecureSaveMessage();
                if (result == MessageBoxResult.No) {
                    editAutoVM.closeWindow();
                    changeButtonState(true);
                    return;
                }
                else if (result == MessageBoxResult.Cancel) {
                    return;
                }
            }
            if (Marke == "") {
                showWarningMessage("Automarke muss angegeben werden!", "Fehler");
                return;
            } else if (Marke.Length > 20) {
                showWarningMessage("Automarke zu lang!", "Fehler");
                return;
            }

            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            if (Id == 0) { // New Auto
                auto = new AutoDto {
                    Marke = Marke,
                    Tagestarif = Tagestarif,
                    Basistarif = Basistarif,
                    AutoKlasse = Klasse
                };
                Autos.Add(auto);
                try {
                    target.CreateAuto(auto);
                } catch (FaultException f) {
                    showWarningMessage("Fehler: \n" + f.ToString(), "Fault");
                } catch (Exception e) {
                    showWarningMessage("Exception: \n" + e.ToString(), "Exception");
                }


            } else {    // Edit Auto
                foreach (var a in Autos) {
                    if (a.Id == Id) {
                        auto = a;
                    }
                }
                auto.Marke = Marke;
                auto.Tagestarif = Tagestarif;
                auto.Basistarif = Basistarif;
                auto.AutoKlasse = Klasse;
                Autos.Add(auto);
                try {
                    target.UpdateAuto(auto);
                } catch (FaultException<OptimisticConcurrencyFault>) {
                    showWarningMessage("Update fehlgeschlagen!", "Fault");
                } catch (FaultException f) {
                    showWarningMessage("Fehler: \n" + f.ToString(), "Fault");
                } catch (Exception e) {
                    showWarningMessage("Exception: \n" + e.ToString(), "Exception");
                }
            }

            Autos.Clear();
            foreach (var a in target.Autos()) {
                Autos.Add(a);
            }
            SelectedAuto = Autos.FirstOrDefault();

            resetAuto();
            editAutoVM.closeWindow();
            changeButtonState(true);
        }


        private void DiscardAutod() {
            editAutoVM.closeWindow();
            changeButtonState(true);
        }

        private void EditAuto() {
            changeButtonState(false);
            try {
                setAuto(SelectedAuto); // Fill the editWindow with the Selected Auto
                editAutoVM = new EditAutoViewModel();
                editAutoVM.setContext(this);
                Id = SelectedAuto.Id;
                return;
            }
            catch (NullReferenceException) {
                showWarningMessage("Kein Auto ausgewählt!", "Fehler");
            } catch (FaultException f) {
                showWarningMessage("Fehler: \n" + f.ToString(), "Fault");
            } catch (Exception e) {
                showWarningMessage("Exception: \n" + e.ToString(), "Exception");
            }
            changeButtonState(true);
        }

        private void AddAuto() {
            changeButtonState(false);
            resetAuto();
            editAutoVM = new EditAutoViewModel();
            editAutoVM.setContext(this);
        }

        private void RemoveAuto() {
            if (SelectedAuto == null) {
                showWarningMessage("Kein Auto ausgewählt!", "Fehler");
                return;
            }
            if (showSecureDeleteMessage()) {
                ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
                target = channelFactory.CreateChannel();

                try {
                    target.RemoveAuto(SelectedAuto);
                    Autos.Remove(SelectedAuto);
                } catch (FaultException f) {
                    showWarningMessage("Fehler: \n" + f.ToString(), "Fault");
                } catch (Exception e) {
                    showWarningMessage("Exception: \n" + e.ToString(), "Exception");
                }
            }
        }
        #endregion

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

        private void changeButtonState(bool state) {
            buttonVisibility = state;
            AddAutoCommand.RaiseCanExecuteChanged();
            EditAutoCommand.RaiseCanExecuteChanged();
            RemoveAutoCommand.RaiseCanExecuteChanged();
        }
        #endregion

    }
}
