using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Plate { get; set; }

        [Required]
        public int PassengersTotal { get; set; }

        public virtual Driver Driver { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public int PassengersRemaining => PassengersTotal - (Students?.Count ?? 0);

        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser IdentityUser { get; set; }
    }
}
