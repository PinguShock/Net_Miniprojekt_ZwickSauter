using System;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private KundeManager target;
        private KundeManager Target => target ?? (target = new KundeManager());

        [Fact]
        public void UpdateKundeTest()
        {
            Kunde kunde = Target.List.First();
            kunde.Vorname = "Franz Josef";
            int kundeid = kunde.Id;
            Target.Update(kunde);
            Assert.Equal(kunde.Vorname,Target.GetById(kundeid).Vorname);
        }
        [Fact]
        public void CreateKundeTest()
        {
            Kunde kunde = new Kunde();
            kunde.Vorname = "Max";
            kunde.Nachname = "Mustermann";
            kunde.Geburtsdatum = DateTime.Today.AddYears(-20);
            Target.Cerate(kunde);
            
            Assert.Equal(Target.List.First(a => a.Vorname == kunde.Vorname&&a.Nachname==kunde.Nachname).Geburtsdatum, kunde.Geburtsdatum);
        }

        [Fact]
        public void RemoveKundeTest()
        {
            Kunde kunde = Target.List.Last();
            Target.Remove(kunde);
            Assert.Throws<InvalidOperationException>(()=>Target.GetById(kunde.Id));
        }
    }
}
