using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Transport.Data;
using Transport.Data.Tables;

namespace Transport.Controllers
{
    [Authorize(Roles = "Driver")]
    public class PassengersController : Controller
    {
        private readonly ILogger<PassengersController> _logger;
        private readonly ApplicationDbContext _context;

        public PassengersController(ILogger<PassengersController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var driver = await _context.Drivers
                .Include(d => d.Car)
                .FirstOrDefaultAsync(d => d.IdentityUserId == userId);

            if (driver == null)
            {
                TempData["Error"] = "Driver not found.";
                return View(Enumerable.Empty<Student>());
            }

            if (driver.Car == null)
            {
                TempData["Error"] = "Your car is not registered yet.";
                return View(Enumerable.Empty<Student>());
            }

            var passengers = await _context.Students
                .Where(s => s.CarId == driver.CarId)
                .ToListAsync();

            return View(passengers);
        }

        [HttpPost]
        public async Task<IActionResult> AddPassenger(string phone)
        {
            if (!User.IsInRole("Driver"))
                return Forbid();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            var driver = await _context.Drivers
                .Include(d => d.Car)
                .FirstOrDefaultAsync(d => d.IdentityUserId == userId);

            if (driver == null)
            {
                TempData["Error"] = "Driver not found.";
                return RedirectToAction("Index");
            }

            
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Phone == phone);

            if (student == null)
            {
                TempData["Error"] = "No student found with that phone number.";
                return RedirectToAction("Index");
            }

            
            if (student.CarId != null)
            {
                TempData["Error"] = "This student is already assigned to another car.";
                return RedirectToAction("Index");
            }

            
            var currentCount = await _context.Students.CountAsync(s => s.CarId == driver.CarId);
            if (currentCount >= driver.Car.PassengersTotal)
            {
                TempData["Error"] = "No remaining seats in your car.";
                return RedirectToAction("Index");
            }

            //
            student.CarId = driver.CarId;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{student.Name} added as passenger.";
            return RedirectToAction("Index");
        }
    }
}

