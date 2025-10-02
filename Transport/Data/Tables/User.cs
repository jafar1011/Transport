using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, MaxLength(11)]
        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string University { get; set; }

        [MaxLength(100)]
        public string College { get; set; }

        [MaxLength(100)]
        public string Department { get; set; }

        [MaxLength(20)]
        public string Stage { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }

        public Parent Parent { get; set; }
    }
}
