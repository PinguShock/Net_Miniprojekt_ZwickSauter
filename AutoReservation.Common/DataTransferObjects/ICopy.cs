using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.Common.DataTransferObjects {
    public interface ICopy<out T>{
        T copy();
    }
}
