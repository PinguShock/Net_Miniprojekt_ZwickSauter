using System;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        public int ReservationsNr { get; set; }
        public int AutoId { get; set; }
        public int KundeId { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public DateTime RowVersion { get; set; }
    
        public Auto Auto { get; set; }
        public Kunde Kunde { get; set; }
    }
}
