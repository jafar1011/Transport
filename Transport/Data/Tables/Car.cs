using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required, MaxLength(50)]
        public string Model { get; set; }

        [Required, MaxLength(20)]
        public string Plate { get; set; }

        [Range(1, 20)]
        public int PassengersTotal { get; set; }

        public Driver Driver { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        [NotMapped]
        public int PassengersRemaining => PassengersTotal - (Users?.Count ?? 0);
    }
}
