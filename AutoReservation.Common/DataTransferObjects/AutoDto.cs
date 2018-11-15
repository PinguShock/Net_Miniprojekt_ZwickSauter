using System;

namespace AutoReservation.Common.DataTransferObjects
{
    public class AutoDto
    {

        public int Id
        {
            get { return Id;}
            set { Id = value; }
        }
        public String Marke
        {
            get { return Marke;}
            set { Marke = value; }
        }
        public int Tagestarif
        {
            get { return Tagestarif;}
            set { Tagestarif = value; }
        }
        public int Basistarif
        {
            get { return Basistarif;}
            set { Basistarif = value; }
        }
        public AutoKlasse AutoKlasse
        {
            get { return AutoKlasse;}
            set { AutoKlasse = value; }
        }
        public DateTime RowVersion
        {
            get { return RowVersion;}
            set { RowVersion = value; }
        }
        
        public override string ToString()
            => $"{Id}; {Marke}; {Tagestarif}; {Basistarif}; {AutoKlasse}; {RowVersion}";
    }
}
