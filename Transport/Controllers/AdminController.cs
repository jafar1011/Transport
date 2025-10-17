using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Transport.Data;
using Transport.Data.Tables;
using Transport.Models;

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

        [HttpGet]
        public async Task<IActionResult> ManageUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Please enter a valid email.";
                return RedirectToAction("Dashboard");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Dashboard");
            }


            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Unknown";

            var model = new ManageUserViewModel
            {
                Email = user.Email,
                Role = role,
                Driver = await _context.Drivers
    .Include(d => d.Car)
    .FirstOrDefaultAsync(d => d.IdentityUserId == user.Id),
                Car = await _context.Cars
    .FirstOrDefaultAsync(c => c.Driver.IdentityUserId == user.Id),
                Student = await _context.Students.FirstOrDefaultAsync(s => s.IdentityUserId == user.Id),
                Parent = await _context.Parents.FirstOrDefaultAsync(p => p.IdentityUserId == user.Id)
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUser(ManageUserViewModel model)
        {
            // REMOVE ModelState validation since we only use parts of the model
            // based on the role

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index");
            }

            var role = model.Role;

            // === DRIVER === 
            if (role == "Driver")
            {
                // Validate only driver-specific fields
                if (string.IsNullOrEmpty(model.Driver?.Name))
                {
                    TempData["Error"] = "Driver name is required.";
                    return RedirectToAction("ManageUser", new { email = model.Email });
                }

                var driver = await _context.Drivers
                    .Include(d => d.Car)
                    .FirstOrDefaultAsync(d => d.IdentityUserId == user.Id);

                if (driver == null)
                {
                    // New driver
                    model.Driver.IdentityUserId = user.Id;

                    if (model.Car != null && !string.IsNullOrEmpty(model.Car.Model))
                    {
                        model.Car.IdentityUserId = user.Id;
                        model.Driver.Car = model.Car;
                    }

                    _context.Drivers.Add(model.Driver);
                }
                else
                {
                    // Update existing driver
                    driver.Name = model.Driver.Name;
                    driver.Phone = model.Driver.Phone;
                    driver.Areas = model.Driver.Areas;

                    if (driver.Car == null && model.Car != null && !string.IsNullOrEmpty(model.Car.Model))
                    {
                        model.Car.IdentityUserId = user.Id;
                        driver.Car = model.Car;
                        _context.Cars.Add(model.Car);
                    }
                    else if (driver.Car != null && model.Car != null)
                    {
                        driver.Car.Model = model.Car.Model;
                        driver.Car.Plate = model.Car.Plate;
                        driver.Car.PassengersTotal = model.Car.PassengersTotal;
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Driver and car info updated successfully.";
            }
            // === STUDENT ===
            else if (role == "Student")
            {
                // Validate only student-specific fields
                if (string.IsNullOrEmpty(model.Student?.Name))
                {
                    TempData["Error"] = "Student name is required.";
                    return RedirectToAction("ManageUser", new { email = model.Email });
                }

                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

                if (student == null)
                {
                    model.Student.IdentityUserId = user.Id;
                    _context.Students.Add(model.Student);
                }
                else
                {
                    student.Name = model.Student.Name;
                    student.Phone = model.Student.Phone;
                    student.Address = model.Student.Address;
                    student.University = model.Student.University;
                    student.College = model.Student.College;
                    student.Department = model.Student.Department;
                    student.Stage = model.Student.Stage;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Student info updated successfully.";
            }
            // === PARENT ===
            else if (role == "Parent")
            {
                // Validate only parent-specific fields
                if (string.IsNullOrEmpty(model.Parent?.Name))
                {
                    TempData["Error"] = "Parent name is required.";
                    return RedirectToAction("ManageUser", new { email = model.Email });
                }

                var parent = await _context.Parents
                    .FirstOrDefaultAsync(p => p.IdentityUserId == user.Id);

                if (parent == null)
                {
                    model.Parent.IdentityUserId = user.Id;
                    _context.Parents.Add(model.Parent);
                }
                else
                {
                    parent.Name = model.Parent.Name;
                    parent.Phone = model.Parent.Phone;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Parent info updated successfully.";
            }
            else
            {
                TempData["Error"] = "Invalid role type.";
            }

            return RedirectToAction("ManageUser", new { email = model.Email });
        }
    }
}