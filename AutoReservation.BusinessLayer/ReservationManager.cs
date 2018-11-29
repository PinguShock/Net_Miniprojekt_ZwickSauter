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
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.First(a => a.ReservationsNr == id);
            }
        }

        public void Cerate(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Reservationen.Add(reservation);
                context.SaveChanges();
            }
        }
        
        public void Update(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Reservationen.Update(reservation);
                context.SaveChanges();
            }
        }

        public void Remove(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Reservationen.Remove(reservation);
                context.SaveChanges();
            }
        }
    }
}