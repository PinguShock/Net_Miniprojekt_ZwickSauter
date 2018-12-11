using System;
using System.Text;

namespace AutoReservation.Common.DataTransferObjects
{
    public class KundeDto : DtoBase<KundeDto>
    {

        public int Id { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public Byte[] RowVersion { get; set; }
        
        public override string ToString()
            => $"{Id}; {Nachname}; {Vorname}; {Geburtsdatum}; {RowVersion}";

        public override string validate() {
            StringBuilder errorMessage = new StringBuilder();
            if (string.IsNullOrEmpty(Nachname)) {
                errorMessage.AppendLine("Nachname benötigt");
            }
            if (string.IsNullOrEmpty(Vorname)) {
                errorMessage.AppendLine("Vorname benötigt");
            }
            if (Geburtsdatum == DateTime.MinValue) {
                errorMessage.AppendLine("Geburtsdatum benötigt");
            }
            if (errorMessage.Length == 0) {
                return null;
            }
            return errorMessage.ToString();
        }
        public override KundeDto copy() {
            return new KundeDto {
                Id = Id,
                Nachname = Nachname,
                Vorname = Vorname,
                Geburtsdatum = Geburtsdatum
            };
        }
    }
}
