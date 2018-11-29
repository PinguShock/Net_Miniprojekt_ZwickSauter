using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AutoReservation.Dal.Entities
{
    [Table("Reservation")]
    public class Reservation
    {
        [Key]
        [Column("id")]
        public int ReservationsNr { get; set; }
        [Column("auto")]
        public int AutoId { get; set; }
        [ForeignKey("AutoId")]
        [InverseProperty("Reservationen")]
        public Auto Auto { get; set; }
        [Column("kunde")]
        public int KundeId { get; set; }
        [ForeignKey("KundeId")]
        [InverseProperty("Reservationen")]
        public Kunde Kunde { get; set; }
        [DataType(DataType.Date)]
        public DateTime Von { get; set; }
        [DataType(DataType.Date)]
        public DateTime Bis { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }
}
