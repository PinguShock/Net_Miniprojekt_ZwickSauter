using System;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Auto
    {
        public virtual HashSet<Reservation> Reservation { get; set; }
        
        public int Id { get; set; }
        public string Marke { get; set; }
        public int Tagestarif { get; set; }
        public DateTime RowVersion { get; set; }
        
        public Auto()
        {
            Reservation = new HashSet<Reservation>();
        }
    }
}
