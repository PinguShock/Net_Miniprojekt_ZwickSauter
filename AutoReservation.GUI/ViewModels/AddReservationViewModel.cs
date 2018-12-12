using AutoReservation.GUI.EditWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.GUI.ViewModels
{
    class AddReservationViewModel
    {

        public AddReservationViewModel() {
            AddReservationWindow editWindow = new AddReservationWindow();
            editWindow.DataContext = this;
            editWindow.Show();
        }
        
    }
}
