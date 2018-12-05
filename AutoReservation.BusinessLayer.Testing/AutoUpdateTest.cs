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
            Auto auto = Target.List.First();
            auto.Tagestarif = 130;
            int autoid = auto.Id;
            Target.Update(auto);
            Assert.Equal(130,Target.GetById(autoid).Tagestarif);
        }
        [Fact]
        public void CreateAutoTest()
        {
            Auto auto = new Auto();
            auto.Marke = "Ferrari Cunque Cento";
            auto.Tagestarif = 230;
            Target.Create(auto);
            
            Assert.Equal(Target.List.First(a => a.Marke == auto.Marke).Tagestarif, auto.Tagestarif);
        }

        [Fact]
        public void RemoveAutoTest()
        {
            Auto auto = Target.List.Last();
            Target.Remove(auto);
            Assert.Throws<InvalidOperationException>(()=>Target.GetById(auto.Id));
        }
    }
}
