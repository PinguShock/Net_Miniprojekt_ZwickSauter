using System;
using System.Text;

namespace AutoReservation.Common.DataTransferObjects
{
    public class AutoDto : DtoBase<AutoDto>
    {

        public int Id
        {
            get;
            set;
        }
        public String Marke
        {
            get;
            set;
        }
        public int Tagestarif
        {
            get;
            set;
        }
        public int Basistarif
        {
            get;
            set;
        }
        public AutoKlasse AutoKlasse
        {
            get;
            set;
        }
        public Byte[] RowVersion
        {
            get;
            set;
        }

        public override string ToString()
            => $"{Id}; {Marke}; {Tagestarif}; {Basistarif}; {AutoKlasse}; {RowVersion}";

        public override string validate() {
            StringBuilder errorMessage = new StringBuilder();
            if (string.IsNullOrEmpty(Marke)) {
                errorMessage.AppendLine("Marke nicht gesetzt");
            }
            if (Tagestarif <= 0) {
                errorMessage.AppendLine("Tagestarif ungültig");
            }
            if (AutoKlasse == AutoKlasse.Luxusklasse && Basistarif <= 0) {
                errorMessage.AppendLine("Basistarif Luxusklasse ungültig");
            }
            if (errorMessage.Length == 0) {
                return null;   
            }
            return errorMessage.ToString();
        }
        public override AutoDto copy() {
            return new AutoDto {
                Id = Id,
                Marke = Marke,
                Tagestarif = Tagestarif,
                AutoKlasse = AutoKlasse,
                Basistarif = Basistarif
            };
        }
    }
}
