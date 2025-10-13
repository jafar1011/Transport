using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Areas { get; set; }

        [Range(0, 5)]
        [NotMapped]
        public float Rating { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }
        public string? IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser IdentityUser { get; set; }

    }
    }
