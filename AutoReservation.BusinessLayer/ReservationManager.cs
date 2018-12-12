using System;
using System.Collections.Generic;
using System.Linq;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
                    List<Reservation> resList = context.Reservationen.ToList();
                    foreach (var res in resList)
                    {
                        res.Auto = context.Autos.First(a => a.Id == res.AutoId);
                        res.Kunde = context.Kunden.First(a => a.Id == res.KundeId);
                    }
                    return resList;
                }
            }
        }
        public Reservation GetById(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Reservation res =  context.Reservationen.First(a => a.ReservationsNr == id);
                res.Auto = context.Autos.First(a => a.Id == res.AutoId);
                res.Kunde = context.Kunden.First(a => a.Id == res.KundeId);
                return res;
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
                try{
                    if (isValid(reservation)) {
                        context.Reservationen.Update(reservation);
                        context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new OptimisticConcurrencyException<Reservation>("Data has been modified!");
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

            if (!isAvailable(reservation.AutoId, reservation.Von, reservation.Bis,reservation.ReservationsNr))
            {
                throw new AutoUnavailableException("Car not available!!");
            }
            return true;
        }

        public bool isAvailable(int autoId, DateTime von, DateTime bis, int resNr = 0)
        {
            foreach (Reservation r in List) {
                if (r.ReservationsNr != resNr 
                    && r.AutoId == autoId 
                    && ((r.Bis > von && r.Von < bis) 
                        || (r.Von < bis && r.Bis > von))) {
                    return false;
                }
            }

            return true;
        }
    }
}