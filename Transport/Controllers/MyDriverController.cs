using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transport.Data;

namespace Transport.Controllers
{
    [Authorize(Roles = "Student")] //students only
    public class MyDriverController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyDriverController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            var userId = _userManager.GetUserId(User);

            var student = _context.Students
         .FirstOrDefault(s => s.IdentityUser.Id == userId);
            
            ViewBag.Student = student;


            if (student != null && student.CarId != null)
            {
                var driver = _context.Drivers
                    .Include(d => d.Car)
                    .FirstOrDefault(d => d.CarId == student.CarId);

                ViewBag.Driver = driver;
            }
            
                return View();
        }
    }
}
