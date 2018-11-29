using System;

namespace AutoReservation.Common.DataTransferObjects
{
    public class AutoDto
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
    }
}
