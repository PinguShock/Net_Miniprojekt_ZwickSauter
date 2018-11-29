using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    [Table("Auto")]
    public class Auto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Marke { get; set; }
        [Required]
        public int Tagestarif { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
        [InverseProperty("Auto")]
        public ICollection<Reservation> Reservationen { get; set; }
 
    }
}
