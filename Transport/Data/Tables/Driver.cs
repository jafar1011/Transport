using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(200)]
        public string Areas { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }
    }
    }
