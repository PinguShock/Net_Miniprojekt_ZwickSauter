using System;
using System.Globalization;
using System.Linq;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoReservation.Service.Wcf.Testing
{
    public abstract class ServiceTestBase
        : TestBase
    {
        protected abstract IAutoReservationService Target { get; }

        #region Read all entities

        [Fact]
        public void GetAutosTest()
        {
            Assert.True(Target.Autos().Count>0);
        }

        [Fact]
        public void GetKundenTest()
        {
            Assert.True(Target.Kunden().Count>0);
        }

        [Fact]
        public void GetReservationenTest()
        {
            Assert.True(Target.Reservationen().Count>0);
        }

        #endregion

        #region Get by existing ID

        [Fact]
        public void GetAutoByIdTest()
        {
            AutoDto auto = Target.Autos().First();
            Assert.Equal(auto.Marke,Target.GetAutoById(auto.Id).Marke);
        }

        [Fact]
        public void GetKundeByIdTest()
        {
            KundeDto kunde = Target.Kunden().First();
            Assert.Equal(kunde.Vorname,Target.GetKundeById(kunde.Id).Vorname);
        }

        [Fact]
        public void GetReservationByNrTest()
        {
            ReservationDto reservation = Target.Reservationen().First();
            Assert.Equal(reservation.Von,Target.GetReservationById(reservation.ReservationsNr).Von);
        }

        #endregion

        #region Get by not existing ID

        [Fact]
        public void GetAutoByIdWithIllegalIdTest()
        {
            Assert.Throws<InvalidOperationException>(()=>Target.GetAutoById(99999));
        }

        [Fact]
        public void GetKundeByIdWithIllegalIdTest()
        {
            Assert.Throws<InvalidOperationException>(()=>Target.GetKundeById(99999));
        }

        [Fact]
        public void GetReservationByNrWithIllegalIdTest()
        {
            Assert.Throws<InvalidOperationException>(()=>Target.GetReservationById(99999));
        }

        #endregion

        #region Insert

        [Fact]
        public void InsertAutoTest()
        {
            AutoDto auto = new AutoDto
            {
                AutoKlasse = AutoKlasse.Luxusklasse,
                Basistarif = 200,
                Marke = "Lamborghini 230",
                Tagestarif = 45
            };
            Target.CreateAuto(auto);
            Assert.Equal(auto.Marke,Target.Autos().Last().Marke);
        }

        [Fact]
        public void InsertKundeTest()
        {
            KundeDto kunde = new KundeDto
            {
                Vorname = "Helene",
                Nachname = "Fischer",
                Geburtsdatum = DateTime.Parse("16.09.1956")
            };
            Target.CreateKunde(kunde);
            Assert.Equal(kunde.Vorname,Target.Kunden().Last().Vorname);
        }

        [Fact]
        public void InsertReservationTest()
        {
            ReservationDto res = new ReservationDto();

            res.Auto = Target.GetAutoById(1);
            res.Kunde = Target.GetKundeById(1);
            res.Von = DateTime.Now.AddHours(18);
            res.Bis = DateTime.Now.AddDays(2);
            
            Target.CreateReservation(res);
            Assert.Equal(res.Auto.Marke,Target.Reservationen().Last().Auto.Marke);
        }

        #endregion

        #region Delete  

        [Fact]
        public void DeleteAutoTest()
        {
            AutoDto auto = Target.Autos().Last();
            Target.RemoveAuto(auto);
            Assert.Throws<InvalidOperationException>(() => Target.GetAutoById(auto.Id));
        }

        [Fact]
        public void DeleteKundeTest()
        {
            KundeDto kunde = Target.Kunden().Last();
            Target.RemoveKunde(kunde);
            Assert.Throws<InvalidOperationException>(() => Target.GetKundeById(kunde.Id));
        }

        [Fact]
        public void DeleteReservationTest()
        {
            ReservationDto res = Target.Reservationen().Last();
            Target.RemoveReservation(res);
            Assert.Throws<InvalidOperationException>(() => Target.GetReservationById(res.ReservationsNr));
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateAutoTest()
        {
            AutoDto auto = Target.Autos().Last();
            auto.Marke = "Hummer Typ C";
            Target.UpdateAuto(auto);
            Assert.Equal(auto.Marke,Target.GetAutoById(auto.Id).Marke);
        }

        [Fact]
        public void UpdateKundeTest()
        {
            KundeDto kunde = Target.Kunden().Last();
            kunde.Nachname = "Stetter";
            Target.UpdateKunde(kunde);
            Assert.Equal(kunde.Nachname,Target.GetKundeById(kunde.Id).Nachname);
        }

        [Fact]
        public void UpdateReservationTest()
        {
            ReservationDto res = Target.Reservationen().Last();
            res.Bis = res.Bis.AddHours(1);
            Target.UpdateReservation(res);
            Assert.Equal(res.Bis,Target.GetReservationById(res.ReservationsNr).Bis);
        }

        #endregion

        #region Update with optimistic concurrency violation

        [Fact]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            AutoDto auto1 = Target.Autos().Last();
            AutoDto auto2 = Target.GetAutoById(auto1.Id);

            auto1.Tagestarif = 140;
            auto2.Tagestarif = 165;
            
            Target.UpdateAuto(auto2);
            Assert.Throws<OptimisticConcurrencyException<Auto>>(() => Target.UpdateAuto(auto1));
        }

        [Fact]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            KundeDto kunde1 = Target.Kunden().Last();
            KundeDto kunde2 = Target.GetKundeById(kunde1.Id);

            kunde1.Geburtsdatum = DateTime.Now.AddYears(-23);
            kunde2.Geburtsdatum = DateTime.Now.AddYears(-24);
            
            Target.UpdateKunde(kunde2);
            Assert.Throws<OptimisticConcurrencyException<Kunde>>(() => Target.UpdateKunde(kunde1));
        }

        [Fact]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            ReservationDto res1 = Target.Reservationen().Last();
            ReservationDto res2 = Target.GetReservationById(res1.ReservationsNr);

            res1.Bis = res1.Bis.AddHours(2);
            res2.Bis = res2.Bis.AddHours(3);
            
            Target.UpdateReservation(res1);
            Assert.Throws<OptimisticConcurrencyException<Reservation>>(() => Target.UpdateReservation(res1));
        }

        #endregion

        #region Insert / update invalid time range

        [Fact]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            ReservationDto res = new ReservationDto
            {
                Auto = Target.Autos().First(),
                Kunde = Target.Kunden().First(),
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(23)
            };
            Assert.Throws<InvalidDateRangeException>(() => Target.CreateReservation(res));

        }

        [Fact]
        public void InsertReservationWithAutoNotAvailableTest()
        {
            ReservationDto res1 = new ReservationDto
            {
                Auto = Target.Autos().First(),
                Kunde = Target.Kunden().First(),
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(25)
            };
            Target.CreateReservation(res1);
            ReservationDto res2 = new ReservationDto
            {
                Auto = Target.GetAutoById(res1.Auto.Id),
                Kunde = Target.Kunden().Last(),
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(43)
            };
            Assert.Throws<AutoUnavailableException>(() => Target.CreateReservation(res2));
        }

        [Fact]
        public void UpdateReservationWithInvalidDateRangeTest()
        {
            ReservationDto res = new ReservationDto
            {
                Auto = Target.Autos().Last(),
                Kunde = Target.Kunden().Last(),
                Von = DateTime.Now.AddHours(26),
                Bis = DateTime.Now.AddDays(3)
            };
            Target.CreateReservation(res);
            res.Bis = DateTime.Now.AddHours(17);
            Assert.Throws<InvalidDateRangeException>(()=>Target.UpdateReservation(res));
        }

        [Fact]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            ReservationDto res1 = new ReservationDto
            {
                Auto = Target.Autos().First(),
                Kunde = Target.Kunden().First(),
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(25)
            };
            Target.CreateReservation(res1);
            ReservationDto res2 = new ReservationDto
            {
                Auto = Target.GetAutoById(res1.Auto.Id),
                Kunde = Target.Kunden().Last(),
                Von = DateTime.Now.AddHours(26),
                Bis = DateTime.Now.AddDays(3)
            };
            Target.CreateReservation(res2);
            res1.Bis = DateTime.Now.AddHours(27);
            Assert.Throws<AutoUnavailableException>(()=>Target.UpdateReservation(res1));
        }

        #endregion

        #region Check Availability

        [Fact]
        public void CheckAvailabilityIsTrueTest()
        {
            
            Assert.True(Target.isAvailable(Target.Autos().First().Id,DateTime.Now,DateTime.Now.AddDays(2)));
        }

        [Fact]
        public void CheckAvailabilityIsFalseTest()
        {
            ReservationDto res = new ReservationDto
            {
                Auto = Target.Autos().First(),
                Kunde = Target.Kunden().Last(),
                Von = DateTime.Now.AddHours(3),
                Bis = DateTime.Now.AddDays(2)
            };
            Target.CreateReservation(res);
            Assert.False(Target.isAvailable(Target.Autos().First().Id,DateTime.Now,DateTime.Now.AddDays(2)));
        }

        #endregion
    }
}
