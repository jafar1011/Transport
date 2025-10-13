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
                driver = await _context.Drivers
                    .Include(d => d.Car)
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


            foreach (var post in posts)
            {
                if (post.Driver != null)
                {
                    var avgRating = await _context.Ratings
                        .Where(r => r.DriverId == post.Driver.DriverId)
                        .Select(r => (float?)r.RatingValue)
                        .AverageAsync();

                    post.Driver.Rating = avgRating ?? 0;
                }
            }

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
                DriverId = driver.DriverId,
                Name = driver.Name,
                Phone = driver.Phone,
                CarName = driver.Car?.Model ?? "Unknown",
                CarYear = model.CarYear,
                AirCondition = model.AirCondition,
                Note = model.Note,
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
        [Authorize(Roles = "Driver,Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");

            
            int? driverId = null;
            if (!isAdmin)
            {
                var driver = await _context.Drivers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.IdentityUserId == userId);

                if (driver == null)
                    return Forbid();
                driverId = driver.DriverId;
            }

            DriverPost post;
            if (isAdmin)
            {
                post = await _context.DriverPosts
                    .FirstOrDefaultAsync(p => p.PostId == id);
            }
            else
            {
                post = await _context.DriverPosts
                    .FirstOrDefaultAsync(p => p.PostId == id && p.DriverId == driverId);
            }

            if (post == null)
                return Forbid();

            _context.DriverPosts.Remove(post);
            await _context.SaveChangesAsync();

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
