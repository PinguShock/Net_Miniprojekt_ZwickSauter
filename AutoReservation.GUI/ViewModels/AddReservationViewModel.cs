using AutoReservation.GUI.EditWindows;
using ReservationReservation.GUI.ViewModels;

namespace AutoReservation.GUI.ViewModels {
    class AddReservationViewModel
    {
        private AddReservationWindow addWindow;

        public AddReservationViewModel() : base() {
            addWindow = new AddReservationWindow();
        }

        public void setContext(ReservationViewModel context) {
            addWindow.DataContext = context;
            addWindow.Show();
        }

        public void closeWindow() {
            addWindow.Close();
        }
        
    }
}
