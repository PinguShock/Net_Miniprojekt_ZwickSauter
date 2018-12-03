using System;
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

        public void Create(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext()) {
                if (isValid(reservation)) { 
                    context.Reservationen.Add(reservation);
                    context.SaveChanges();
                }
            }
        }
        
        public void Update(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                if (isValid(reservation)) {
                    context.Reservationen.Update(reservation);
                    context.SaveChanges();
                }
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

        private bool isValid(Reservation reservation) {
            System.TimeSpan diff = reservation.Bis.Subtract(reservation.Von);
            System.TimeSpan minDuration = DateTime.Now.AddHours(24).Subtract(DateTime.Now);
            if (diff < minDuration) {
                throw new InvalidDateRangeException("Duration less than 24h!!");
            }
            if (reservation.Von > reservation.Bis) {
                throw new InvalidDateRangeException("Startdate before Enddate!!");
            }
            
            foreach (Reservation r in List) {
                if (r.ReservationsNr != reservation.ReservationsNr 
                    && r.AutoId == reservation.AutoId 
                    && ((r.Bis > reservation.Von && r.Von < reservation.Bis) 
                        || (r.Von < reservation.Bis && r.Bis > reservation.Von))) {
                    throw new AutoUnavailableException("Car not available!!");
                }
            }
            return true;
        }
    }
}