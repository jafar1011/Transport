using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport.Data.Tables
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int DriverId { get; set; }
        public float RatingValue { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }

    }
}
