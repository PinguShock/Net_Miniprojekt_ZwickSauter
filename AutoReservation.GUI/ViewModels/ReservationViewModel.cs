
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoReservation.GUI.ViewModels;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.GUI.Factory;
using AutoReservation.Common.Extensions;

namespace ReservationReservation.GUI.ViewModels
{
    public class ReservationViewModel : ViewModelBase {
        private List<ReservationDto> originalReservationen = new List<ReservationDto>();
        private ObservableCollection<ReservationDto> _Reservationen = new ObservableCollection<ReservationDto>();

        public ReservationViewModel(IServiceFactory factory) : base(factory) { }
        public ReservationViewModel() : base() {
        }

        public ObservableCollection<ReservationDto> Reservationen {
            get { return _Reservationen; }
        }

        private ReservationDto _selectedReservation;
        public ReservationDto selectedReservation {
            get { return _selectedReservation; }
            set {
                if (_selectedReservation == value) {
                    return;
                }
                _selectedReservation = value;
                this.OnPropertyChanged(p => p.selectedReservation);
            }
        }

        #region AddReservation
        private CommandBaseClass _AddReservationCommand;
        public ICommand AddReservationCommand {
            get {
                return _AddReservationCommand ?? (_AddReservationCommand = new CommandBaseClass(param => New(), param => CanNew()));
            }
        }

        private void New() {
            Reservationen.Add(new ReservationDto());
        }

        private bool CanNew() {
            return serviceExist;
        }



        #endregion

        #region RemoveReservation
        private CommandBaseClass _RemoveReservationCommand;
        public ICommand RemoveReservationCommand {
            get {
                return _RemoveReservationCommand ?? (_RemoveReservationCommand = new CommandBaseClass(param => remove(), param => canRemove()));
            }
        }
        private void remove() {
            service.Remove(selectedReservation);
            load();
        }
        private bool canRemove() {
            return serviceExist && selectedReservation != null && selectedReservation.ReservationsNr != default(int);
        }

        #endregion

        #region Save
        private CommandBaseClass _SaveReservationCommand;
        public ICommand SaveReservationCommand {
            get {
                return _SaveReservationCommand ?? (_SaveReservationCommand = new CommandBaseClass(param => save(), param => canSave()));
            }
        }
        public void save() {
            foreach (var r in Reservationen) {
                if (r.ReservationsNr == default(int)) {
                    service.Create(r);
                }
                else {
                    var original = originalReservationen.FirstOrDefault(o => o.ReservationsNr == r.ReservationsNr);
                    service.Update(r);
                }
            }
            load();
        }

        private bool canSave() {
            if (!serviceExist) {
                return false;
            }
            return validate(Reservationen);
        }

        #endregion

        #region Load

        private CommandBaseClass _LoadReservationsCommand;
        public ICommand LoadReservationsCommand {
            get {
                return _LoadReservationsCommand ?? (_LoadReservationsCommand = new CommandBaseClass(param => load(), param => canLoadReservation()));
            }
        }
        protected override void load() {
            Reservationen.Clear();
            originalReservationen.Clear();
            /*foreach (var a in service.GetAllReservationen()) { // TODO: implementieren in ReservationReservationenervice
                Reservationen.Add(a);
                originalReservationen.Add(a.copy());
            }*/
            selectedReservation = Reservationen.FirstOrDefault();
        }
        private bool canLoadReservation() {
            return serviceExist;
        }
        #endregion
    }
}
