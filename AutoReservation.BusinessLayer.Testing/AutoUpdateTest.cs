using System;
using System.Linq;
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
            Auto auto = Target.List[0];
            auto.Tagestarif = 130;
            int autoid = auto.Id;
            Target.Update(auto);
            Assert.Equal(130,Target.GetById(autoid).Tagestarif);
        }
    }
}
