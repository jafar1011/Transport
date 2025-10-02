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

        [Required]
        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string University { get; set; }

        [Required]
        public string College { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Stage { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }

        public virtual Parent Parent { get; set; }
    }
}
