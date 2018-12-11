using System.Collections.Generic;
using System.Linq;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class AutoManager
        : ManagerBase
    {
        public List<Auto> List
        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Autos.ToList();
                }
            }
        }
        public Auto GetById(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Autos.First(a => a.Id == id);
            }
        }

        public void Create(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Autos.Add(auto);
                context.SaveChanges();
            }
        }
        
        public void Update(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Autos.Update(auto);
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new OptimisticConcurrencyException<Auto>("Data has been modified!");
                }
            }
        }

        public void Remove(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Autos.Remove(auto);
                context.SaveChanges();
            }
        }
    }
}