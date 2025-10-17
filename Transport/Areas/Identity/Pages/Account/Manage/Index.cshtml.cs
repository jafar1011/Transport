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
                // Fetch existing driver and car from DB
                var existingDriver = await _context.Drivers
                    .Include(d => d.Car)
                    .FirstOrDefaultAsync(d => d.IdentityUserId == user.Id);

                if (existingDriver == null)
                {
                    // New driver
                    Driver.IdentityUserId = user.Id;
                    if (Car != null) Car.IdentityUserId = user.Id;
                    Driver.Car = Car;
                    _context.Drivers.Add(Driver);
                }
                else
                {
                    // Update existing driver
                    existingDriver.Name = Driver.Name;
                    existingDriver.Phone = Driver.Phone;
                    existingDriver.Areas = Driver.Areas;

                    if (existingDriver.Car == null && Car != null)
                    {
                        Car.IdentityUserId = user.Id;
                        existingDriver.Car = Car;
                        _context.Cars.Add(Car);
                    }
                    else if (existingDriver.Car != null && Car != null)
                    {
                        existingDriver.Car.Model = Car.Model;
                        existingDriver.Car.Plate = Car.Plate;
                        existingDriver.Car.PassengersTotal = Car.PassengersTotal;
                    }
                }
            }
            else if (Role == "Student")
            {
                // Fetch existing student
                var existingStudent = await _context.Students
                    .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

                if (existingStudent == null)
                {
                    Student.IdentityUserId = user.Id;
                    _context.Students.Add(Student);
                }
                else
                {
                    existingStudent.Name = Student.Name;
                    existingStudent.Phone = Student.Phone;
                    existingStudent.Address = Student.Address;
                    existingStudent.University = Student.University;
                    existingStudent.College = Student.College;
                    existingStudent.Department = Student.Department;
                    existingStudent.Stage = Student.Stage;
                }
            }
            else if (Role == "Parent")
            {
                // Fetch existing parent
                var existingParent = await _context.Parents
                    .FirstOrDefaultAsync(p => p.IdentityUserId == user.Id);

                if (existingParent == null)
                {
                    Parent.IdentityUserId = user.Id;
                    _context.Parents.Add(Parent);
                }
                else
                {
                    existingParent.Name = Parent.Name;
                    existingParent.Phone = Parent.Phone;
                }
            }

            // Save all changes
            await _context.SaveChangesAsync();

            // Update phone number if changed
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
