using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public List<Kunde> List
        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Kunden.ToList();
                }
            }
        }
        public Kunde GetById(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Kunden.First(a => a.Id == id);
            }
        }

        public void Create(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Kunden.Add(kunde);
                context.SaveChanges();
            }
        }
        
        public void Update(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Kunden.Update(kunde);
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new OptimisticConcurrencyException<Kunde>("Data has been modified!");
                }
            }
        }

        public void Remove(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Kunden.Remove(kunde);
                context.SaveChanges();
            }
        }
    }
}