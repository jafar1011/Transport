using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class DriverPost
    {

        [Key]
        public int PostId { get; set; }

        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }

      
        [Required]
        public string CarName { get; set; }
        public int CarYear { get; set; }
        public bool AirCondition { get; set; }

        
        public List<DriverPostArea> Areas { get; set; } = new List<DriverPostArea>();

        public string? Note { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class DriverPostArea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AreaName { get; set; }

        [Required]
        public int DriverPostId { get; set; }
        [ForeignKey("DriverPostId")]
        public DriverPost DriverPost { get; set; }
    }
}
