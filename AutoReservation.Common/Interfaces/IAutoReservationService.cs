using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract]
    public interface IAutoReservationService
    {
        [OperationContract]
         List<AutoDto> Autos();

        [OperationContract]
        [FaultContract(typeof(InvalidOperationFault))]
         AutoDto GetAutoById(int id);

        [OperationContract]
         void CreateAuto(AutoDto autoDto);

        [OperationContract]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
         void UpdateAuto(AutoDto autoDto);

        [OperationContract]
         void RemoveAuto(AutoDto autoDto);
        
        [OperationContract]
         List<KundeDto> Kunden();

         [OperationContract]
         [FaultContract(typeof(InvalidOperationFault))]
         KundeDto GetKundeById(int id);

        [OperationContract]
         void CreateKunde(KundeDto kundeDto);

         [OperationContract]
         [FaultContract(typeof(OptimisticConcurrencyFault))]
         void UpdateKunde(KundeDto kundeDto);

         [OperationContract]
         void RemoveKunde(KundeDto kundeDto);
        
         [OperationContract]
         List<ReservationDto> Reservationen();

         [OperationContract]
         [FaultContract(typeof(InvalidOperationFault))]
         ReservationDto GetReservationById(int id);

         [OperationContract]
         [FaultContract(typeof(AutoUnavailableFault))]
         [FaultContract(typeof(InvalidDateRangeFault))]
         void CreateReservation(ReservationDto reservationDto);

         [OperationContract]
         [FaultContract(typeof(OptimisticConcurrencyFault))]
         [FaultContract(typeof(AutoUnavailableFault))]
         [FaultContract(typeof(InvalidDateRangeFault))]
         void UpdateReservation(ReservationDto reservationDto);

         [OperationContract]
         void RemoveReservation(ReservationDto reservationDto);

         [OperationContract]
         bool isAvailable(int autoId, DateTime von, DateTime bis, int resNr = 0);
    }
}
