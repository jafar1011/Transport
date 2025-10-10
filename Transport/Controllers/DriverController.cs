using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Transport.Data;
using Transport.Data.Tables;

namespace Transport.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        private readonly ILogger<DriverController> _logger;
        private readonly ApplicationDbContext _context;

        public DriverController(ILogger<DriverController> logger, ApplicationDbContext context)
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

            var existingInvite = await _context.Invites
                .FirstOrDefaultAsync(i => i.StudentId == student.StudentId && i.DriverId == driver.DriverId && i.Status == "Pending");

            if (existingInvite != null)
            {
                TempData["Error"] = "An invite is already pending for this student.";
                return RedirectToAction("Index");
            }

            var invite = new Invite
            {
                StudentId = student.StudentId,
                DriverId = driver.DriverId,
                Status = "Pending"
            };

            _context.Invites.Add(invite);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Invite sent to {student.Name}. Awaiting approval.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemovePassenger(int studentId)
        {
            if (!User.IsInRole("Driver"))
                return Forbid();

            var student = await _context.Students.FindAsync(studentId);
            if (student == null || student.CarId == null)
            {
                TempData["Error"] = "Passenger not found or not assigned.";
                return RedirectToAction("Index");
            }

            student.CarId = null;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{student.Name} removed successfully.";
            return RedirectToAction("Index");
        }
    }
}

