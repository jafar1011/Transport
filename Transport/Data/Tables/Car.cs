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

        public ICollection<User> Users { get; set; } = new List<User>();

        public int PassengersRemaining => PassengersTotal - (Users?.Count ?? 0);
    }
}
