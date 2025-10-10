using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Transport.Data;
using Transport.Data.Tables;

namespace Transport.Controllers
{
    [Authorize(Roles = "Student")] //students only
    public class StudentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
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

                
                if (driver != null)
                {
                    var avgRating = _context.Ratings
                        .Where(r => r.DriverId == driver.DriverId)
                        .Select(r => (float?)r.RatingValue)
                        .Average();

                    driver.Rating = avgRating ?? 0;
                }

                ViewBag.Driver = driver;
            }

            return View();
        }

        public async Task<IActionResult> Invites()
        {
            var userId = _userManager.GetUserId(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.IdentityUserId == userId);

            if (student == null)
                return NotFound();

            var invites = await _context.Invites
                .Include(i => i.Driver)
                    .ThenInclude(d => d.Car)  
                .Include(i => i.Parent)    
                .Where(i => i.StudentId == student.StudentId && i.Status == "Pending")
                .ToListAsync();

            return View(invites);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptInvite(int inviteId)
        {
            var invite = await _context.Invites
        .Include(i => i.Student)
        .Include(i => i.Driver)
        .ThenInclude(d => d.Car)
        .Include(i => i.Parent)
        .FirstOrDefaultAsync(i => i.InviteId == inviteId);

            if (invite == null)
                return NotFound();

            invite.Status = "Accepted";
            if (invite.DriverId != null && invite.ParentId == null) 
            { 
            invite.Student.CarId = invite.Driver.CarId;
            }
            else if ((invite.DriverId == null && invite.ParentId != null))
            {
                invite.Parent.StudentId = invite.Student.StudentId;
            }

                await _context.SaveChangesAsync();
            TempData["Success"] = "Invite accepted successfully!";
            return RedirectToAction("Invites");
        }

        [HttpPost]
        public async Task<IActionResult> RejectInvite(int inviteId)
        {
            var invite = await _context.Invites.FindAsync(inviteId);
            if (invite == null)
                return NotFound();

            invite.Status = "Rejected";
            await _context.SaveChangesAsync();
            TempData["Info"] = "Invite rejected.";
            return RedirectToAction("Invites");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateDriver(int driverId, float ratingValue)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var existing = await _context.Ratings
                .FirstOrDefaultAsync(r => r.DriverId == driverId && r.UserId == userId);

            if (existing != null)
            {
                existing.RatingValue = ratingValue;
            }
            else
            {
                _context.Ratings.Add(new Rating
                {
                    DriverId = driverId,
                    UserId = userId,
                    RatingValue = ratingValue
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
