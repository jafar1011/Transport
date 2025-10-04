using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class Parent
    {
        [Key]
        public int ParentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }


        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}
