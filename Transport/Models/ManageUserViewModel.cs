using Transport.Data.Tables;
namespace Transport.Models
{
    public class ManageUserViewModel
    {
        public string Email { get; set; }
        public string Role { get; set; }

        public Driver Driver { get; set; } = new Driver();
        public Car Car { get; set; } = new Car();
        public Student Student { get; set; } = new Student();
        public Parent Parent { get; set; } = new Parent();
    }
}
