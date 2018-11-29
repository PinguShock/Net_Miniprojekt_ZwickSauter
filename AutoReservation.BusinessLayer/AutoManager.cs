using System.Collections.Generic;
using System.Linq;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;

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
                
            }
        }

        public void 
        
        public void Update(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Autos.Update(auto);
                context.SaveChanges();
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