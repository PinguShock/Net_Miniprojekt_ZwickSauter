using System;
using System.Globalization;
using System.Linq;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.TestEnvironment;
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
            Target.Create(auto);
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
            Target.Create(kunde);
            Assert.Equal(kunde.Vorname,Target.Kunden().Last().Vorname);
        }

        [Fact]
        public void InsertReservationTest()
        {
            ReservationDto res = new ReservationDto
            {
                Auto = Target.Autos().Last(),
                Kunde = Target.Kunden().Last(),
                Von = DateTime.Now.AddHours(18),
                Bis = DateTime.Now.AddHours(24)
            };
            Target.Create(res);
            Assert.Equal(res.Auto.Marke,Target.Reservationen().Last().Auto.Marke);
        }

        #endregion

        #region Delete  

        [Fact]
        public void DeleteAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void DeleteKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void DeleteReservationTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Update with optimistic concurrency violation

        [Fact]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Insert / update invalid time range

        [Fact]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void InsertReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Check Availability

        [Fact]
        public void CheckAvailabilityIsTrueTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void CheckAvailabilityIsFalseTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion
    }
}
