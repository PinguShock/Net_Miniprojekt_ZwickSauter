using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.GUI.ViewModels;

namespace KundeReservation.GUI.ViewModels {
    public class KundeViewModel : ViewModelBase {
        private KundeDto Kunde;

        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime Geburtsdatum { get; set; }

        public RelayCommand EditKundeCommand { get; set; }
        public RelayCommand AddKundeCommand { get; set; }
        public RelayCommand RemoveKundeCommand { get; set; }
        public RelayCommand ConfirmEditKundeCommand { get; set; }
        public RelayCommand DiscardKundeButtonCommand { get; set; }

        EditKundeViewModel editKundeVM;

        private IAutoReservationService target;

        private bool buttonVisibility = true;

        public KundeViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();
            Kunden = new ObservableCollection<KundeDto>(target.Kunden());

            EditKundeCommand = new RelayCommand(() => this.EditKunde(), () => buttonVisibility);
            AddKundeCommand = new RelayCommand(() => this.AddKunde(), () => buttonVisibility);
            RemoveKundeCommand = new RelayCommand(() => this.RemoveKunde(), () => buttonVisibility);
            ConfirmEditKundeCommand = new RelayCommand(() => this.ConfirmEdit(), () => true);
            DiscardKundeButtonCommand = new RelayCommand(() => this.Discard(), () => true);
        }

        private ObservableCollection<KundeDto> _Kunden;
        public ObservableCollection<KundeDto> Kunden {
            get { return _Kunden; }
            set { _Kunden = value; }
        }

        private KundeDto _selectedKunde;
        public KundeDto SelectedKunde {
            get { return _selectedKunde; }
            set { _selectedKunde = value; }
        }

        #region CommandMethods
        private void ConfirmEdit() {
            if (Id != 0) { // Ask only when a Customer is edited, not if a new Customer is generated.
                MessageBoxResult result = showSecureSaveMessage();
                if (result == MessageBoxResult.No) {
                    editKundeVM.closeWindow();
                    changeButtonState(true);
                    return;
                }
                else if (result == MessageBoxResult.Cancel) {
                    return;
                }
            }
            if (Vorname == "" || Nachname == "" || Geburtsdatum > DateTime.Now) {
                showWarningMessage( "Alle Felder müssen korrekt ausgefüllt werden!" +
                                    "\nVor- und Nachname sowie Geburtstdatum benötigt." +
                                    "\nGeburtsdatum muss in der Vergangenheit liegen.", "Fehler");
                return;
            }

            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            if (Id == 0) { // New Kunde
                Kunde = new KundeDto {
                    Vorname = Vorname,
                    Nachname = Nachname,
                    Geburtsdatum = Geburtsdatum
                };
                Kunden.Add(Kunde);
                target.CreateKunde(Kunde);
            } else {    // Edit Kunde
                Kunde = target.GetKundeById(this.Id);
                Kunde.Vorname = Vorname;
                Kunde.Nachname = Nachname;
                Kunde.Geburtsdatum = Geburtsdatum;
                Kunden.Add(Kunde);
                try {
                    target.UpdateKunde(Kunde);
                }
                catch (OptimisticConcurrencyException<KundeDto>) {
                    showWarningMessage("Update fehlgeschlagen!\nOptimistic Concurrency Exception.", "Update Fehlgeschlagen");
                }
            }

            Kunden.Clear();
            foreach (var a in target.Kunden()) {
                Kunden.Add(a);
            }
            SelectedKunde = Kunden.FirstOrDefault();

            resetKunde();
            editKundeVM.closeWindow();
            changeButtonState(true);
        }


        private void Discard() {
            editKundeVM.closeWindow();
            changeButtonState(true);
        }

        private void EditKunde() {
            changeButtonState(false);
            try {
                setKunde(SelectedKunde); // Fill the editWindow with the Selected Kunde
                editKundeVM = new EditKundeViewModel();
                editKundeVM.setContext(this);
                Id = SelectedKunde.Id;
                return;
            }
            catch (NullReferenceException) {
                showWarningMessage("Kein Kunde ausgewählt!", "Fehler");
            }
            catch (Exception e) {
                Console.WriteLine("Exception catched in KundeViewModel:" + e.ToString());
            }
            changeButtonState(true);
        }

        private void AddKunde() {
            changeButtonState(false);
            resetKunde();
            editKundeVM = new EditKundeViewModel();
            editKundeVM.setContext(this);
        }

        private void RemoveKunde() {
            if (SelectedKunde == null) {
                showWarningMessage("Kein Kunde ausgewählt!", "Fehler");
                return;
            }
            if (showSecureDeleteMessage()) {
                ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
                target = channelFactory.CreateChannel();

                target.RemoveKunde(SelectedKunde);
                Kunden.Remove(SelectedKunde);
            }
        }
        #endregion

        #region HelperMethods

        private void setKunde(KundeDto Kunde) {
            Id = Kunde.Id;
            Vorname = Kunde.Vorname;
            Nachname = Kunde.Nachname;
            Geburtsdatum = Kunde.Geburtsdatum;
        }

        private void resetKunde() {
            Id = 0;
            Vorname = "";
            Nachname = "";
            Geburtsdatum = DateTime.Now;
        }

        private void changeButtonState(bool state) {
            buttonVisibility = state;
            AddKundeCommand.RaiseCanExecuteChanged();
            EditKundeCommand.RaiseCanExecuteChanged();
            RemoveKundeCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
