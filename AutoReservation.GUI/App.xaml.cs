using AutoReservation.GUI.ViewModels;
using AutoReservation.GUI.Views;
using KundeReservation.GUI.ViewModels;
using ReservationReservation.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoReservation.GUI
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    /// 
        

    public partial class App : Application
    {

        public AutoViewModel AutoViewModel { get; set; }
        public KundeViewModel KundeViewModel { get; set; }
        public ReservationViewModel ReservationVewModel { get; set; }
        // KundeViewModel & ReservationViewModel ...

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);



            


        }



    }
}
