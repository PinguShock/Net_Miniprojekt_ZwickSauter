using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class LuxusklasseAuto : Auto
    {
        [Required]
        public int Basistarif { get; set; }
    }
}