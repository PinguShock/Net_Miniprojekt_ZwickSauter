using System;
using System.Diagnostics;
using AutoReservation.BusinessLayer;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Service.Wcf
{
    public class AutoReservationService
    {
        private AutoManager autosManager;
        private KundeManager kundeManager;
        private ReservationManager reservationManager;
        public AutoReservationService()
        {
            autosManager = new AutoManager();
            kundeManager = new KundeManager();
            reservationManager = new ReservationManager();
            
        }
        
        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public void printAutos()
        {
            foreach (Auto auto in autosManager.List)
            {
                Console.WriteLine(auto.Marke);
            }
        }
    }
}