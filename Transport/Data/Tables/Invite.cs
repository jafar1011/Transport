namespace Transport.Data.Tables
{
    public class Invite
    {
        public int InviteId { get; set; }

        //target
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        //sender
        public int? DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public int? ParentId { get; set; }
        public virtual Parent Parent { get; set; }

        public string Status { get; set; } = "Pending"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
