using AutoReservation.GUI.Views;

namespace AutoReservation.GUI.ViewModels {
    public class EditAutoViewModel : ViewModelBase
    {
        private EditAutoWindow editWindow;

        public EditAutoViewModel() : base() {
            editWindow = new EditAutoWindow();
        }

        public void setContext(AutoViewModel context) {
            editWindow.DataContext = context;
            editWindow.Show();
        }

        public void closeWindow() {
            editWindow.Close();
        }
    }
}
