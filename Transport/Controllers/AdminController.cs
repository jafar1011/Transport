using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Transport.Data;
using Transport.Data.Tables;

namespace Transport.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            ILogger<AdminController> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Verification()
        {
            var unverifiedDrivers = _context.Drivers
            .Include(d => d.IdentityUser)
            .Where(d => _context.Permissions
            .Any(p => p.IdentityUserId == d.IdentityUser.Id && p.Verification == "Pending"))
            .ToList();

            return View(unverifiedDrivers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveDriver(string id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
                return NotFound();

            permission.Verification = "Approved";
            permission.IsDisabled = false;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Driver approved successfully.";
            return RedirectToAction("Verification"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectDriver(string id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
                return NotFound();

            permission.Verification = "Rejected";
            permission.IsDisabled = true;
            await _context.SaveChangesAsync();

            TempData["Warning"] = "Driver rejected successfully.";
            return RedirectToAction("Verification");
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index", "Admin");
            }

            
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            
            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
                TempData["Success"] = $"User {email} is now an Admin.";
            else
                TempData["Error"] = "Failed to assign Admin role.";

            return RedirectToAction("Index", "Admin");
        }


        [HttpPost]
        public async Task<IActionResult> DisableUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Email is required.";
                return RedirectToAction("Index", "Admin");
            }

            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "Driver not found.";
                return RedirectToAction("Index", "Admin");
            }

           
            if (!await _userManager.IsInRoleAsync(user, "Driver"))
            {
                TempData["Error"] = "Only drivers can be disabled.";
                return RedirectToAction("Index", "Admin");
            }

          
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.IdentityUserId == user.Id);

            if (permission == null)
            {
                TempData["Error"] = "Permission record not found for this user.";
                return RedirectToAction("Index", "Admin");
            }

         
            permission.IsDisabled = true;
            permission.Verification = "Rejected";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Driver has been disabled successfully.";
            return RedirectToAction("Index", "Admin");
        }
    }
}