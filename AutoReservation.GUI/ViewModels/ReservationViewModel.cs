﻿
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

        AddReservationViewModel editReservationVM;

        private IAutoReservationService target;

        private bool buttonVisibility = true;

        public ReservationViewModel() : base() {
            ChannelFactory<IAutoReservationService> channelFactory = new ChannelFactory<IAutoReservationService>("AutoReservationService");
            target = channelFactory.CreateChannel();
            Reservationen = new ObservableCollection<ReservationDto>(target.Reservationen());

            generateComboBoxLists();

            AddReservationCommand = new RelayCommand(() => this.AddReservation(), () => buttonVisibility);
            RemoveReservationCommand = new RelayCommand(() => this.RemoveReservation(), () => buttonVisibility);
            ConfirmAddReservationCommand = new RelayCommand(() => this.ConfirmAdd(), () => true);
            DiscardReservationButtonCommand = new RelayCommand(() => this.Discard(), () => true);

            StartTimer();
        }

        private string lastRefreshedTime = "Last update: ";
        public string LastRefreshedTime {
            get { return lastRefreshedTime; }
            set { lastRefreshedTime = value; }
        }

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

        private ReservationDto _selectedReservation;
        public ReservationDto SelectedReservation {
            get { return _selectedReservation; }
            set { _selectedReservation = value; }
        }

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

            Reservationen.Clear();
            foreach (var a in target.Reservationen()) {
                Reservationen.Add(a);
            }
            SelectedReservation = Reservationen.FirstOrDefault();

            resetReservation();
            editReservationVM.closeWindow();
            changeButtonState(true);
        }


        private void Discard() {
            editReservationVM.closeWindow();
            changeButtonState(true);
        }

        private void AddReservation() {
            changeButtonState(false);
            resetReservation();
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

                target.RemoveReservation(SelectedReservation);
                Reservationen.Remove(SelectedReservation);
            }
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

        private void resetReservation() {
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
            Autos = new ObservableCollection<AutoDto>(target.Autos());
            Kunden = new ObservableCollection<KundeDto>(target.Kunden());
            foreach (var a in Autos) {
                _AutoNames.Add(a.Marke);
            }
            foreach (var k in Kunden) {
                _KundenNames.Add(k.Vorname + " " + k.Nachname);
            }
        }

        private void StartTimer() {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.Tick += (sender, args) => {
                Reservationen.Clear();
                foreach (var a in target.Reservationen()) {
                    Reservationen.Add(a);
                }
                LastRefreshedTime = "Last update: " + DateTime.Now.ToLocalTime().ToString();
                Console.WriteLine("Refreshed!");
            };
            Timer.Start();
        }
        #endregion
    }
}
