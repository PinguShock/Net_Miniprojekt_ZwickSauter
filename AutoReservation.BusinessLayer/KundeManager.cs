using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;

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
            foreach (Kunde kunde in List)
            {
                if (kunde.Id == id)
                {
                    return kunde;
                }
            }
            return null;
        }

        public void Update(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Kunden.Update(kunde);
            }
        }
    }
}