using System;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Common.DataTransferObjects
{
    public class ReservationDto
    {
        public int ReservationsNr { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public Byte[] RowVersion { get; set; }
    
        public AutoDto Auto { get; set; }
        public KundeDto Kunde { get; set; }
        
        public override string ToString()
            => $"{ReservationsNr}; {Von}; {Bis}; {Auto}; {Kunde}";
    }
}
