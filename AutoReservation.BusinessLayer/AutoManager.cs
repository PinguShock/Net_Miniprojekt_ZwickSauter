using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using System.Collections.Generic;

namespace AutoReservation.BusinessLayer
{
    public class AutoManager
        : ManagerBase
    {
        
        public List<Auto> List {
            get {
                using (AutoReservationContext context = new AutoReservationContext()) {
                    return context.Auto
                }
            }
        }
        public void insertAuto(Auto toInsert) 
        {
            using (AutoReservationContext context = new AutoReservationContext()) {
                context.Auto.insert(toInsert);
                context.SaveChanges();
            }
        }


    }
}