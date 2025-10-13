using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        public async Task<IActionResult> DisableUser(string email)
        {
            if (!User.IsInRole("Admin"))
                return Forbid();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            user.IsDisabled = true; // mark as disabled
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                TempData["Success"] = $"User ({email}) has been disabled.";
            else
                TempData["Error"] = "Failed to disable user.";

            return RedirectToAction(nameof(Index));
        }
    }
}