using AutoReservation.GUI.EditWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.GUI.ViewModels
{
    class EditKundeViewModel
    {
        public EditKundeViewModel() {
            EditKundeWindow editWindow = new EditKundeWindow();
            editWindow.DataContext = this;
            editWindow.Show();
        }
    }
}
