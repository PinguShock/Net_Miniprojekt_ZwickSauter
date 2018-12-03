using System;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        /* DateRange Availability:
         * OK-Szenarien:
         *  Reservation 24h
         *  Reservation Von vor Bis
         * 
         * NOK-Szenarien: 
         *  Reservation < 24h
         *  Reservation Bis vor Von 2h
         *  Reservation Bis vor Von 24h
         */

        [Fact]
        public void ScenarioOkay01Test()
        {
            // Reservation 24h
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };

            Target.Create(reservation1);
            Assert.NotEqual(0, reservation1.ReservationsNr);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            // Reservation Von vor Bis
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };

            Target.Create(reservation1);
            Assert.NotEqual(0, reservation1.ReservationsNr);
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            // Reservation < 24h
            Reservation reservation = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(12)
            };

            Exception ex = Assert.Throws<InvalidDateRangeException>(
                () => Target.Create(reservation));

            Assert.Equal("Duration less than 24h!!", ex.Message);
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            // Reservation Bis vor Von 2h
            Reservation reservation = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now.AddHours(2),
                Bis = DateTime.Now
            };

            Exception ex = Assert.Throws<InvalidDateRangeException>(
                () => Target.Create(reservation));

            Assert.Equal("Duration less than 24h!!", ex.Message);
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            // Reservation Bis vor Von 24h
            Reservation reservation = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now.AddHours(24),
                Bis = DateTime.Now
            };

            Exception ex = Assert.Throws<InvalidDateRangeException>(
                () => Target.Create(reservation));

            Assert.Equal("Duration less than 24h!!", ex.Message);
        }
    }
}
