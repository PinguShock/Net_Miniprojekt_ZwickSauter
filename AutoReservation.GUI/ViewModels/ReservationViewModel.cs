
using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoReservation.GUI.ViewModels;
using AutoReservation.Common.DataTransferObjects;
using System.ServiceModel;
using AutoReservation.Common.Interfaces;
using System.Collections.Generic;
using AutoReservation.Common.DataTransferObjects.Faults;
using System.Windows.Threading;

namespace ReservationReservation.GUI.ViewModels {
    public class ReservationViewModel : ViewModelBase {

        private ReservationDto Reservation;

        public DispatcherTimer Timer { get; set; }

        public int ReservationsNr { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public string Kunde{ get; set; }
        public string Auto { get; set; }

        AutoDto auto = new AutoDto();
        KundeDto kunde = new KundeDto();

        public RelayCommand AddReservationCommand { get; set; }
        public RelayCommand RemoveReservationCommand { get; set; }
        public RelayCommand ConfirmAddReservationCommand { get; set; }
        public RelayCommand DiscardReservationButtonCommand { get; set; }
        public RelayCommand FilterReservationCommand { get; set; }

        AddReservationViewModel editReservationVM;

        private IAutoReservationService target;

        private bool buttonVisibility = true;
        private bool isFiltered = true;


        public ReservationViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            Reservationen = new ObservableCollection<ReservationDto>(target.Reservationen());

            generateComboBoxLists();

            AddReservationCommand = new RelayCommand(() => this.AddReservation(), () => buttonVisibility);
            RemoveReservationCommand = new RelayCommand(() => this.RemoveReservation(), () => buttonVisibility);
            ConfirmAddReservationCommand = new RelayCommand(() => this.ConfirmAdd(), () => true);
            DiscardReservationButtonCommand = new RelayCommand(() => this.Discard(), () => true);
            FilterReservationCommand = new RelayCommand(() => this.FilterReservations(), () => true);

            RefreshReservationen();
            StartTimer();
        }

        #region Propertys
        private ObservableCollection<ReservationDto> _Reservationen;
        public ObservableCollection<ReservationDto> Reservationen {
            get { return _Reservationen; }
            set { _Reservationen = value; }
        }

        private ObservableCollection<AutoDto> _Autos;
        public ObservableCollection<AutoDto> Autos {
            get { return _Autos; }
            set { _Autos = value; }
        }
        private ObservableCollection<KundeDto> _Kunden;
        public ObservableCollection<KundeDto> Kunden {
            get { return _Kunden; }
            set { _Kunden = value; }
        }

        private List<string> _AutoNames = new List<string>();
        public List<string> AutoNames {
            get { return _AutoNames; }
            set { _AutoNames = value; }
        }

        private List<string> _KundenNames = new List<string>();
        public List<string> KundeNames {
            get { return _KundenNames; }
            set { _KundenNames = value; }
        }

        private ReservationDto _selectedReservation;
        public ReservationDto SelectedReservation {
            get { return _selectedReservation; }
            set { _selectedReservation = value; }
        }
        #endregion

        #region CommandMethods
        private void ConfirmAdd() {
            if (Von > Bis || Von < DateTime.Today) {
                showWarningMessage("Alle Felder müssen korrekt ausgefüllt werden!" +
                                    "\nAuto und Kunde benötigt." +
                                    "\nDatum muss gültig sein.", "Fehler");
                return;
            }
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();

            foreach (var a in Autos) {
                if (Auto == a.Marke) {
                    auto = a;
                }
            }
            foreach (var k in Kunden) {
                if (Kunde == (k.Vorname + " " + k.Nachname)) {
                    kunde = k;
                }
            }

            if (ReservationsNr == 0) { // New Reservation
                Reservation = new ReservationDto {
                    Von = Von,
                    Bis = Bis,
                    Auto = auto,
                    Kunde = kunde
                };

                try {
                    Reservationen.Add(Reservation);
                    target.CreateReservation(Reservation);
                } catch (FaultException<AutoUnavailableFault>) {
                    showWarningMessage("Auto nicht verfügbar!", "Fehler");
                    return;
                } catch (FaultException<InvalidDateRangeFault>) {
                    showWarningMessage("Datum ungültig!", "Fehler");
                    return;
                } catch (FaultException f) {
                    showWarningMessage("Fault: " + f.ToString(), "Fault");
                    return;
                } catch (Exception e) {
                    showWarningMessage("Exception: " + e.ToString(), "Exception");
                    return;
                }
            }

            RefreshReservationen();
            SelectedReservation = Reservationen.FirstOrDefault();

            resetBindedValues();
            editReservationVM.closeWindow();
            changeButtonState(true);
        }


        private void Discard() {
            editReservationVM.closeWindow();
            changeButtonState(true);
        }

        private void AddReservation() {
            generateComboBoxLists();
            changeButtonState(false);
            resetBindedValues();
            editReservationVM = new AddReservationViewModel();
            editReservationVM.setContext(this);
        }

        private void RemoveReservation() {
            if (SelectedReservation == null) {
                showWarningMessage("Keine Reservation ausgewählt!", "Fehler");
                return;
            }
            if (showSecureDeleteMessage()) {
                ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
                target = channelFactory.CreateChannel();

                try {
                    target.RemoveReservation(SelectedReservation);
                    Reservationen.Remove(SelectedReservation);
                } catch (FaultException f) {
                    showWarningMessage("Fault: " + f.ToString(), "Fault");
                    return;
                } catch (Exception e) {
                    showWarningMessage("Exception: " + e.ToString(), "Exception");
                    return;
                }
                
            }
        }

        private void FilterReservations() {
            if (isFiltered) {
                isFiltered = false;
            } else {
                isFiltered = true;
            }
            RefreshReservationen();
        }
        #endregion

        #region HelperMethods

        private void setReservation(ReservationDto Reservation) {
            ReservationsNr = Reservation.ReservationsNr;
            Von = Reservation.Von;
            Bis = Reservation.Bis;
            auto = Reservation.Auto;
            kunde = Reservation.Kunde;
        }

        private void resetBindedValues() {
            ReservationsNr = 0;
            Von = DateTime.Now;
            Bis = DateTime.Now;
            Auto = null;
            Kunde = null;
        }

        private void changeButtonState(bool state) {
            buttonVisibility = state;
            AddReservationCommand.RaiseCanExecuteChanged();
            RemoveReservationCommand.RaiseCanExecuteChanged();
        }

        private void generateComboBoxLists() {
            AutoNames.Clear();
            KundeNames.Clear();

            Autos = new ObservableCollection<AutoDto>(target.Autos());
            Kunden = new ObservableCollection<KundeDto>(target.Kunden());
            foreach (var a in Autos) {
                _AutoNames.Add(a.Marke);
            }
            foreach (var k in Kunden) {
                _KundenNames.Add(k.Vorname + " " + k.Nachname);
            }
        }

        private void RefreshReservationen() {
            if (!isFiltered) {
                Reservationen.Clear();
                foreach (var a in target.Reservationen()) {
                    Reservationen.Add(a);
                }
            } else {
                Reservationen.Clear();
                foreach (var a in target.Reservationen()) {
                    if (a.Von < DateTime.Now && a.Bis > DateTime.Now) {
                        Reservationen.Add(a);
                    }
                }
            }
        }

        private void StartTimer() {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(10);
            Timer.Tick += (sender, args) => {
                RefreshReservationen();
            };
            Timer.Start();
        }
        #endregion
    }
}
