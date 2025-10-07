using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Transport.Data;
using Transport.Data.Tables;
using Transport.Models;

namespace Transport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        
          public async Task<IActionResult> Index(string searchTerm)
        {
            Driver driver = null;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Driver") && userId != null)
            {
                driver = await _context.Drivers.Include(d => d.Car)
                                               .FirstOrDefaultAsync(d => d.IdentityUserId == userId);
            }

            var postsQuery = _context.DriverPosts
     .Include(p => p.Driver)
     .Include(p => p.Areas)
     .AsQueryable();
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                postsQuery = postsQuery.Where(p => p.Areas.Any(a => a.AreaName.Contains(searchTerm)));
            }


            var posts = await postsQuery.ToListAsync();

            ViewData["searchTerm"] = searchTerm;
            ViewBag.Driver = driver;
            ViewBag.Posts = posts;

            return View(new DriverPost());
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(DriverPost model, string[] Areas)
        {
            if (!User.IsInRole("Driver"))
                return Forbid();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var driver = await _context.Drivers
                .Include(d => d.Car)
                .FirstOrDefaultAsync(d => d.IdentityUserId == userId);

            if (driver == null)
            {
                TempData["StatusMessage"] = "Driver profile not found!";
                return RedirectToAction("Index");
            }

            var post = new DriverPost
            {
                IdentityUserId = userId,
                Name = driver.Name,
                Phone = driver.Phone,
                CarName = driver.Car?.Model ?? "Unknown",
                CarYear = model.CarYear,
                AirCondition = model.AirCondition,
                CreatedAt = DateTime.UtcNow
            };

            if (Areas != null)
            {
                foreach (var area in Areas)
                    post.Areas.Add(new DriverPostArea { AreaName = area });
            }

            _context.DriverPosts.Add(post);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = "Post added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]
        public IActionResult DeletePost(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = _context.DriverPosts.FirstOrDefault(p => p.PostId == id && p.IdentityUserId == userId);
            if (post == null)
            {
                return Unauthorized(); 
            }

            _context.DriverPosts.Remove(post);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Post deleted successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
