using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Service.Wcf;

namespace AutoDtoReservationDto.Service.Wcf
{
    public class AutoReservationService
    {
        private readonly AutoManager AutoManager = new AutoManager();
        private readonly KundeManager KundeManager = new KundeManager();
        private readonly ReservationManager ReservationManager = new ReservationManager();
        
        
        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public List<AutoDto> Autos => AutoManager.List.ConvertToDtos();

        public AutoDto GetAutoById(int id)
        {
            return AutoManager.GetById(id).ConvertToDto();
        }

        public void Cerate(AutoDto autoDto)
        {
            AutoManager.Create(autoDto.ConvertToEntity());
        }
        
        public void Update(AutoDto autoDto)
        {
            AutoManager.Update(autoDto.ConvertToEntity());
        }

        public void Remove(AutoDto autoDto)
        {
            AutoManager.Remove(autoDto.ConvertToEntity());
        }
        public List<KundeDto> Kunden => KundeManager.List.ConvertToDtos();

        public KundeDto GetKundeById(int id)
        {
            return KundeManager.GetById(id).ConvertToDto();
        }

        public void Cerate(KundeDto kundeDto)
        {
            KundeManager.Create(kundeDto.ConvertToEntity());
        }
        
        public void Update(KundeDto kundeDto)
        {
            KundeManager.Update(kundeDto.ConvertToEntity());
        }

        public void Remove(KundeDto kundeDto)
        {
            KundeManager.Remove(kundeDto.ConvertToEntity());
        }
        public List<ReservationDto> Reservationen => ReservationManager.List.ConvertToDtos();

        public ReservationDto GetReservationById(int id)
        {
            return ReservationManager.GetById(id).ConvertToDto();
        }

        public void Create(ReservationDto reservationDto)
        {
            ReservationManager.Create(reservationDto.ConvertToEntity());
        }
        
        public void Update(ReservationDto reservationDto)
        {
            ReservationManager.Update(reservationDto.ConvertToEntity());
        }

        public void Remove(ReservationDto reservationDto)
        {
            ReservationManager.Remove(reservationDto.ConvertToEntity());
        }
    }
}