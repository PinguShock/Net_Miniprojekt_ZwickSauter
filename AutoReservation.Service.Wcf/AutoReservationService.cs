using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.Service.Wcf;

namespace AutoDtoReservationDto.Service.Wcf
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
            WriteActualMethod();
            return AutoManager.List.ConvertToDtos();
        }

        public AutoDto GetAutoById(int id)
        {
            WriteActualMethod();
            return AutoManager.GetById(id).ConvertToDto();
        }

        public void Create(AutoDto autoDto)
        {
            WriteActualMethod();
            AutoManager.Create(autoDto.ConvertToEntity());
        }
        
        public void Update(AutoDto autoDto)
        {
            WriteActualMethod();
            AutoManager.Update(autoDto.ConvertToEntity());
        }

        public void Remove(AutoDto autoDto)
        {
            WriteActualMethod();
            AutoManager.Remove(autoDto.ConvertToEntity());
        }

        public List<KundeDto> Kunden()
        {
            WriteActualMethod();
            return KundeManager.List.ConvertToDtos(); 
        }

        public KundeDto GetKundeById(int id)
        {
            WriteActualMethod();
            return KundeManager.GetById(id).ConvertToDto();
        }

        public void Create(KundeDto kundeDto)
        {
            WriteActualMethod();
            KundeManager.Create(kundeDto.ConvertToEntity());
        }
        
        public void Update(KundeDto kundeDto)
        {
            WriteActualMethod();
            KundeManager.Update(kundeDto.ConvertToEntity());
        }

        public void Remove(KundeDto kundeDto)
        {
            WriteActualMethod();
            KundeManager.Remove(kundeDto.ConvertToEntity());
        }

        public List<ReservationDto> Reservationen()
        {
            WriteActualMethod();
            return  ReservationManager.List.ConvertToDtos();
        }

        public ReservationDto GetReservationById(int id)
        {
            WriteActualMethod();
            return ReservationManager.GetById(id).ConvertToDto();
        }

        public void Create(ReservationDto reservationDto)
        {
            WriteActualMethod();
            ReservationManager.Create(reservationDto.ConvertToEntity());
        }
        
        public void Update(ReservationDto reservationDto)
        {
            WriteActualMethod();
            ReservationManager.Update(reservationDto.ConvertToEntity());
        }

        public void Remove(ReservationDto reservationDto)
        {
            WriteActualMethod();
            ReservationManager.Remove(reservationDto.ConvertToEntity());
        }
    }
}