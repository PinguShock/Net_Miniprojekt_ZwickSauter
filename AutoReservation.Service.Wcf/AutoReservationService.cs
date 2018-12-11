using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.Common.Interfaces;
using AutoReservation.Dal.Entities;
using AutoReservation.Service.Wcf;

namespace AutoReservation.Service.Wcf
{
    public class AutoReservationService : IAutoReservationService
    {
        private readonly AutoManager AutoManager = new AutoManager();
        private readonly KundeManager KundeManager = new KundeManager();
        private readonly ReservationManager ReservationManager = new ReservationManager();
        
        
        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public List<AutoDto> Autos()
        {
            return AutoManager.List.ConvertToDtos();
        }

        public AutoDto GetAutoById(int id)
        {
            try
            {
                return AutoManager.GetById(id).ConvertToDto();
            }
            catch (InvalidOperationException)
            {
                InvalidOperationFault invalidOperationFault = new InvalidOperationFault
                {
                    Message = "Car not found!"
                };
                throw new FaultException<InvalidOperationFault>(invalidOperationFault);
            }
        }

        public void CreateAuto(AutoDto autoDto)
        {
            AutoManager.Create(autoDto.ConvertToEntity());
        }
        
        public void UpdateAuto(AutoDto autoDto)
        {
            try
            {
                AutoManager.Update(autoDto.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Auto> e)
            {
                OptimisticConcurrencyFault optimisticConcurrencyFault = new OptimisticConcurrencyFault
                {
                    Message = e.Message
                };
                throw new FaultException<OptimisticConcurrencyFault>(optimisticConcurrencyFault);
            }
        }

        public void RemoveAuto(AutoDto autoDto)
        {
            AutoManager.Remove(autoDto.ConvertToEntity());
        }

        public List<KundeDto> Kunden()
        {
            return KundeManager.List.ConvertToDtos(); 
        }

        public KundeDto GetKundeById(int id)
        {
            try{
            return KundeManager.GetById(id).ConvertToDto();
            }
            catch (InvalidOperationException)
            {
                InvalidOperationFault invalidOperationFault = new InvalidOperationFault
                {
                    Message = "Customer not found!"
                };
                throw new FaultException<InvalidOperationFault>(invalidOperationFault);
            }
        }

        public void CreateKunde(KundeDto kundeDto)
        {
            KundeManager.Create(kundeDto.ConvertToEntity());
        }
        
        public void UpdateKunde(KundeDto kundeDto)
        {
            try{
            KundeManager.Update(kundeDto.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Kunde> e)
            {
                OptimisticConcurrencyFault optimisticConcurrencyFault = new OptimisticConcurrencyFault
                {
                    Message = e.Message
                };
                throw new FaultException<OptimisticConcurrencyFault>(optimisticConcurrencyFault);
            }
        }

        public void RemoveKunde(KundeDto kundeDto)
        {
            KundeManager.Remove(kundeDto.ConvertToEntity());
        }

        public List<ReservationDto> Reservationen()
        { 
            return  ReservationManager.List.ConvertToDtos();
        }

        public ReservationDto GetReservationById(int id)
        {
            try{
            return ReservationManager.GetById(id).ConvertToDto();
            }
            catch (InvalidOperationException)
            {
                InvalidOperationFault invalidOperationFault = new InvalidOperationFault
                {
                    Message = "Reservation not found!"
                };
                throw new FaultException<InvalidOperationFault>(invalidOperationFault);
            }
        }

        public void CreateReservation(ReservationDto reservationDto)
        {
            try
            {
                ReservationManager.Create(reservationDto.ConvertToEntity());
            }
            catch (AutoUnavailableException e)
            {
                AutoUnavailableFault autoUnavailableFault = new AutoUnavailableFault
                {
                    Message = e.Message
                };
                throw new FaultException<AutoUnavailableFault>(autoUnavailableFault);
            }
            catch (InvalidCastException e)
            {
                InvalidOperationFault invalidOperationFault = new InvalidOperationFault
                {
                    Message = e.Message
                };
                throw new FaultException<InvalidOperationFault>(invalidOperationFault);
            }
        }
        
        public void UpdateReservation(ReservationDto reservationDto)
        {
            try{
            ReservationManager.Update(reservationDto.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Reservation> e)
            {
                OptimisticConcurrencyFault optimisticConcurrencyFault = new OptimisticConcurrencyFault
                {
                    Message = e.Message
                };
                throw new FaultException<OptimisticConcurrencyFault>(optimisticConcurrencyFault);
            }
        }

        public void RemoveReservation(ReservationDto reservationDto)
        {
            ReservationManager.Remove(reservationDto.ConvertToEntity());
        }

        public bool isAvailable(int autoId, DateTime von, DateTime bis, int resNr = 0)
        {
            return ReservationManager.isAvailable(autoId, von, bis, resNr);
        }
    }
}