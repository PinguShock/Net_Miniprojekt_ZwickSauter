using AutoReservation.GUI.EditWindows;
using KundeReservation.GUI.ViewModels;

namespace AutoReservation.GUI.ViewModels {
    class EditKundeViewModel
    {
        private EditKundeWindow editWindow;

        public EditKundeViewModel() : base() {
            editWindow = new EditKundeWindow();
        }

        public void setContext(KundeViewModel context) {
            editWindow.DataContext = context;
            editWindow.Show();
        }

        public void closeWindow() {
            editWindow.Close();
        }
    }
}
