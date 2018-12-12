using System;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());
        
        private KundeManager ktarget;
        private KundeManager KTarget => ktarget ?? (ktarget = new KundeManager());
        
        private AutoManager atarget;
        private AutoManager ATarget => atarget ?? (atarget = new AutoManager());

        [Fact]
        public void UpdateReservationTest()
        {
            Reservation reservation = Target.List.First();
            reservation.Bis = reservation.Bis.AddDays(4);
            int resNr = reservation.ReservationsNr;
            Target.Update(reservation);
            Assert.Equal(reservation.Bis,Target.GetById(resNr).Bis);
        }
        [Fact]
        public void CreateReservationTest()
        {
            Reservation reservation = new Reservation
            {
                KundeId = KTarget.List.Last().Id,
                AutoId = ATarget.List.Last().Id,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };
            Target.Create(reservation);
            
            Assert.Equal(Target.List.Last().Bis,reservation.Bis);
        }

        [Fact]
        public void RemoveReservationTest()
        {
            Reservation reservation = Target.List.Last();
            Target.Remove(reservation);
            Assert.Throws<InvalidOperationException>(()=>Target.GetById(reservation.ReservationsNr));
        }
    }
}
