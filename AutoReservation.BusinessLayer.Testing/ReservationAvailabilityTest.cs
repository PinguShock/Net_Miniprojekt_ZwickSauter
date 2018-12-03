using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        public ReservationAvailabilityTest()
        {
            // Prepare reservation
            //Reservation reservation = Target.GetById(1);
            //reservation.Von = DateTime.Today;
            //reservation.Bis = DateTime.Today.AddDays(1);
            //Target.Update(reservation);
        }

        /* Requirement Availability:
         * OK-Szenarien:
         *  Gleiches Auto, nahtlos
         *  Ungleiches Auto, überlappend
         * 
         * NOK-Szenarien: 
         *  Gleiches Auto, überlappend, res1 vor res2
         *  Gleiches Auto, überlappend, res2 vor res1
         *  Gleiches Auto, überlappend, res1 = res2
         */

        [Fact]
        public void ScenarioOkay01Test()
        {
            // Gleiches Auto, nahtlos
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };

            Reservation reservation2 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now.AddHours(24),
                Bis = DateTime.Now.AddHours(48)
            };
            Target.Create(reservation1);
            Target.Create(reservation2);

            Assert.NotEqual(0, reservation2.ReservationsNr);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            // Ungleiches Auto, überlappend
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };

            Reservation reservation2 = new Reservation {
                KundeId = 1,
                AutoId = 2,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };
            Target.Create(reservation1);
            Target.Create(reservation2);

            Assert.NotEqual(0, reservation2.ReservationsNr);
        }

        //[Fact]
        //public void ScenarioOkay03Test()
        //{
        //    throw new NotImplementedException("Test not implemented.");
        //}

        //[Fact]
        //public void ScenarioOkay04Test()
        //{
        //    throw new NotImplementedException("Test not implemented.");
        //}

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            // Gleiches Auto, überlappend, res1 vor res2
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };
            Target.Create(reservation1);

            Reservation reservation2 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now.AddHours(2),
                Bis = DateTime.Now.AddHours(26)
            };

            Exception ex = Assert.Throws<AutoUnavailableException>(
                () => Target.Create(reservation2));
            Assert.Equal("Car not available!!", ex.Message);
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            // Gleiches Auto, überlappend, res2 vor res1
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now.AddHours(2),
                Bis = DateTime.Now.AddHours(26)
            };
            Target.Create(reservation1);

            Reservation reservation2 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };

            Exception ex = Assert.Throws<AutoUnavailableException>(
                () => Target.Create(reservation2));
            Assert.Equal("Car not available!!", ex.Message);
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            // Gleiches Auto, überlappend, res1 = res2
            Reservation reservation1 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };
            Target.Create(reservation1);

            Reservation reservation2 = new Reservation {
                KundeId = 1,
                AutoId = 1,
                Von = DateTime.Now,
                Bis = DateTime.Now.AddHours(24)
            };

            Exception ex = Assert.Throws<AutoUnavailableException>(
                () => Target.Create(reservation2));
            Assert.Equal("Car not available!!", ex.Message);
        }

        //[Fact]
        //public void ScenarioNotOkay04Test()
        //{
        //    throw new NotImplementedException("Test not implemented.");
        //}

        //[Fact]
        //public void ScenarioNotOkay05Test()
        //{
        //    throw new NotImplementedException("Test not implemented.");
        //}
    }
}
