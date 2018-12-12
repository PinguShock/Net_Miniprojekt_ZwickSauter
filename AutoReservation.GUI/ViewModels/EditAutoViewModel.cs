using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.GUI.Views;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

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
