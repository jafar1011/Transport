using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Transport.Data;
using Transport.Data.Tables;

namespace Transport.Controllers
{
    [Authorize(Roles = "Parent")]
    public class ParentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ParentController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var parent = await _context.Parents
                .Include(p => p.Student)
                .FirstOrDefaultAsync(p => p.IdentityUserId == userId);

            return View(parent);
        }


        public async Task<IActionResult> LinkStudent(string phone)
        {
            if (!User.IsInRole("Parent"))
                return Forbid();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var parent = await _context.Parents
                .FirstOrDefaultAsync(d => d.IdentityUserId == userId);

            if (parent == null)
            {
                TempData["Error"] = "Parent not found.";
                return RedirectToAction("Index");
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Phone == phone);

            if (student == null)
            {
                TempData["Error"] = "No student found with that phone number.";
                return RedirectToAction("Index");
            }


            var existingInvite = await _context.Invites
                .FirstOrDefaultAsync(i => i.StudentId == student.StudentId && i.ParentId == parent.ParentId && i.Status == "Pending");

            if (existingInvite != null)
            {
                TempData["Error"] = "An invite is already pending for this student.";
                return RedirectToAction("Index");
            }

            var invite = new Invite
            {
                StudentId = student.StudentId,
                ParentId = parent.ParentId,
                Status = "Pending"
            };

            _context.Invites.Add(invite);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Invite sent to {student.Name}. Awaiting approval.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UnlinkStudent(int studentId)
        {
            if (!User.IsInRole("Parent"))
                return Forbid();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parent = await _context.Parents
                .FirstOrDefaultAsync(p => p.IdentityUserId == userId);

            if (parent == null)
            { 
                return NotFound("Parent not found.");
            }
            else
            {
                parent.StudentId = null;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Student unlinked successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
