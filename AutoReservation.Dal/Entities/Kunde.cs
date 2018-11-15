using System;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
    
        public int Id { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public DateTime RowVersion { get; set; }
    
        public HashSet<Reservation> Reservation { get; set; }
        
        public Kunde()
        {
            Reservation = new HashSet<Reservation>();
        }
    }
}
