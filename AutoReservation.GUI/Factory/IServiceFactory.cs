using AutoReservation.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.GUI.Factory {
    public interface IServiceFactory {
        IAutoReservationService GetService();
    }
}
