using System;
using System.Text;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Common.DataTransferObjects
{
    public class ReservationDto : DtoBase<ReservationDto>
    {
        public int ReservationsNr { get; set; }
        public DateTime Von { get; set; }
        public DateTime Bis { get; set; }
        public Byte[] RowVersion { get; set; }
    
        public AutoDto Auto { get; set; }
        public KundeDto Kunde { get; set; }

        

        public override string ToString()
            => $"{ReservationsNr}; {Von}; {Bis}; {Auto}; {Kunde}";

        public override string validate() {
            StringBuilder errorMessage = new StringBuilder();
            if (Von > Bis || Bis == DateTime.MinValue || Von == DateTime.MinValue) {
                errorMessage.AppendLine("Datum nicht gültig!");
            }
            if (Auto == null) {
                errorMessage.AppendLine("Kein Auto gewählt");
            } else {
                string autoError = Auto.validate();
                if (!string.IsNullOrEmpty(autoError)) {
                    errorMessage.AppendLine(autoError);
                }
            }
            if (Kunde == null) {
                errorMessage.AppendLine("Kein Kunde gewählt");
            } else {
                string kundeError = Kunde.validate();
                if (!string.IsNullOrEmpty(kundeError)) {
                    errorMessage.AppendLine(kundeError);
                }
            }


            if (errorMessage.Length == 0) {
                return null;
            }
            return errorMessage.ToString();
        }
        public override ReservationDto copy() {
            return new ReservationDto {
                ReservationsNr = ReservationsNr,
                Von = Von,
                Bis = Bis,
                Auto = Auto.copy(),
                Kunde = Kunde.copy()
            };
        }
    }
}
