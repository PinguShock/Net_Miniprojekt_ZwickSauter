using System;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private AutoManager target;
        private AutoManager Target => target ?? (target = new AutoManager());

        [Fact]
        public void UpdateAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void InsertAutoTest() {
            AutoManager autoManager = new AutoManager();
            Auto auto = new Auto();
            auto.Id = 5;
            auto.Marke = "VW";
            auto.Tagestarif = 50;
            autoManager.insertAuto(auto);
        }
    }
}
