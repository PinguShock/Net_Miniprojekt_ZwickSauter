using System.Collections.Generic;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.Common.Interfaces
{
    public interface IAutoReservationService
    {
         List<AutoDto> Autos();

         AutoDto GetAutoById(int id);

         void Create(AutoDto autoDto);

         void Update(AutoDto autoDto);

         void Remove(AutoDto autoDto);
         List<KundeDto> Kunden();

         KundeDto GetKundeById(int id);

         void Create(KundeDto kundeDto);

         void Update(KundeDto kundeDto);

         void Remove(KundeDto kundeDto);
         List<ReservationDto> Reservationen();

         ReservationDto GetReservationById(int id);

         void Create(ReservationDto reservationDto);

         void Update(ReservationDto reservationDto);

         void Remove(ReservationDto reservationDto);
    }
}
