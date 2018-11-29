using System.Collections.Generic;
using System.Linq;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public List<Reservation> List
        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Reservationen.ToList();
                }
            }
            
        }
        public Reservation GetById(int id)
        {
            foreach (Reservation reservation in List)
            {
                if (reservation.ReservationsNr == id)
                {
                    return reservation;
                }
            }
            return null;
        }

        public void Update(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Reservationen.Update(reservation);
            }
        }
    }
}