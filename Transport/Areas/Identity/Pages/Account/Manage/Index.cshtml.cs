// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Transport.Data;
using Transport.Data.Tables;

namespace Transport.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Role { get; set; }

        [BindProperty] public Driver Driver { get; set; }
        [BindProperty] public Car Car { get; set; }
        [BindProperty] public Student Student { get; set; }
        [BindProperty] public Parent Parent { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
           
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (Role == "Driver")
            {
                Driver = await _context.Drivers
                    .Include(d => d.Car)
                    .FirstOrDefaultAsync(d => d.IdentityUserId == user.Id)
                    ?? new Driver { IdentityUserId = user.Id, Car = new Car() };
                Car = Driver.Car ?? new Car();
            }
            else if (Role == "Student")
            {
                Student = await _context.Students
                    .Include(s => s.Car)
                    .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id)
                    ?? new Student { IdentityUserId = user.Id };
            }
            else if (Role == "Parent")
            {
                Parent = await _context.Parents
                    .Include(p => p.Student)
                    .FirstOrDefaultAsync(p => p.IdentityUserId == user.Id)
                    ?? new Parent { IdentityUserId = user.Id };
            }
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (Role == "Driver")
            {
               
                Driver.IdentityUserId = user.Id;
                if (Driver.DriverId == 0)
                    _context.Drivers.Add(Driver);
                else
                    _context.Drivers.Update(Driver);

                if (Car != null)
                {
                    Car.IdentityUserId = user.Id;
                    if (Car.CarId == 0)
                        _context.Cars.Add(Car);
                    else
                        _context.Cars.Update(Car);
                    Driver.Car = Car;
                }
            }
            else if (Role == "Student")
            {
                Student.IdentityUserId = user.Id;
                if (Student.StudentId == 0)
                    _context.Students.Add(Student);
                else
                    _context.Students.Update(Student);
            }
            else if (Role == "Parent")
            {
                Parent.IdentityUserId = user.Id;
                if (Parent.ParentId == 0)
                    _context.Parents.Add(Parent);
                else
                    _context.Parents.Update(Parent);
            }

            await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
