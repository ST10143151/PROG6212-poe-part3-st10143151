using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CMCS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace CMCS.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ClaimsController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        /*public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Coordinator"))
            {
                var claims = await _context.Claims.ToListAsync();
                return View(claims);
            }
            else
            {
                var userClaims = await _context.Claims
                    .Where(c => c.Name == user.UserName)
                    .ToListAsync();
                return View(userClaims);
            }
        }*/

        public async Task<IActionResult> Dashboard()
        {
            var claims = await _context.Claims
        .Select(c => new Claim
        {
            Id = c.Id,
            Name = c.Name,
            HoursWorked = c.HoursWorked,
            HourlyRate = c.HourlyRate,
            TotalPayment = c.HoursWorked * c.HourlyRate,
            Status = c.Status
        })
        .ToListAsync();

            return View(claims);
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Admin and Coordinator view all claims
            if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Coordinator") || await _userManager.IsInRoleAsync(user, "Manager") || await _userManager.IsInRoleAsync(user, "HR"))
            {
                var claims = await _context.Claims
                    .Join(
                        _userManager.Users, // Join with Users
                        claim => claim.Name, // Match Claim.Name with User.UserName
                        user => user.UserName,
                        (claim, user) => new Claim
                        {
                            Id = claim.Id,
                            Name = claim.Name,
                            Description = claim.Description,
                            HoursWorked = claim.HoursWorked,
                            HourlyRate = user.HourlyRate, // Fetch HourlyRate
                            TotalPayment = claim.HoursWorked * user.HourlyRate, // Calculate TotalPayment
                            DocumentPath = claim.DocumentPath,
                            Status = claim.Status,
                            StartDate = claim.StartDate,
                            EndDate = claim.EndDate
                        })
                    .ToListAsync();

                return View(claims);
            }
            else
            {
                // Lecturer sees only their own claims
                var userClaims = await _context.Claims
                    .Join(
                        _userManager.Users,
                        claim => claim.Name,
                        user => user.UserName,
                        (claim, user) => new Claim
                        {
                            Id = claim.Id,
                            Name = claim.Name,
                            Description = claim.Description,
                            HoursWorked = claim.HoursWorked,
                            HourlyRate = user.HourlyRate, // Fetch HourlyRate
                            TotalPayment = claim.HoursWorked * user.HourlyRate, // Calculate TotalPayment
                            DocumentPath = claim.DocumentPath,
                            Status = claim.Status,
                            StartDate = claim.StartDate,
                            EndDate = claim.EndDate
                        })
                    .Where(c => c.Name == user.UserName) // Filter only lecturer's claims
                    .ToListAsync();

                return View(userClaims);
            }
        }





        /*[Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            return View();
        }*/

        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            var claim = new Claim
            {
                HourlyRate = _userManager.GetUserAsync(User).Result?.HourlyRate ?? 0 // Prefill hourly rate
            };

            return View(claim);
        }


        [HttpPost]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Create(Claim claim, IFormFile document)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            claim.Name = user.UserName;

            // Automatically set "Auto-Approved" if the claim meets validation criteria
            if (claim.IsValid())
            {
                claim.Status = "Auto-Approved";
            }
            else
            {
                claim.Status = "Pending"; // Default to "Pending" if not auto-approved
            }

            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            if (claim.EndDate < claim.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date cannot be earlier than start date.");
                return View(claim);
            }

            claim.TotalPayment = claim.HoursWorked * claim.HourlyRate;

            // Handle document upload
            if (document != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = $"{Guid.NewGuid()}_{document.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }

                claim.DocumentPath = uniqueFileName;
            }

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Coordinator,Admin,Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Approved"; // Set status explicitly to "Approved"
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Coordinator,Admin,Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Rejected"; // Set status explicitly to "Rejected"
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





        /*[HttpPost]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Create(Claim claim, IFormFile document)
        {
            // Retrieve the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Assign the user name to the claim
            claim.Name = user.UserName;
            claim.Status = "Pending";

            // Validate the model
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            // Validate date range
            if (claim.EndDate < claim.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date cannot be earlier than start date.");
                return View(claim);
            }

            // Ensure positive values for hours worked and hourly rate
            claim.HoursWorked = Math.Max(claim.HoursWorked, 0);
            claim.HourlyRate = Math.Max(claim.HourlyRate, 0); // Validate manual input for HourlyRate

            // Calculate total payment
            claim.TotalPayment = claim.HoursWorked * claim.HourlyRate;

            // Handle file upload for the document
            if (document != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = $"{Guid.NewGuid()}_{document.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }

                claim.DocumentPath = uniqueFileName;
            }

            // Save the claim to the database
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Redirect to the claims index page
            return RedirectToAction(nameof(Index));
        }*/


        /*[Authorize(Roles = "Coordinator,Admin,Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Approved";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Coordinator,Admin,Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = "Rejected";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }*/

        /*[Authorize(Roles = "Admin,Coordinator")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Coordinator")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }*/

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        [Authorize]
        public async Task<IActionResult> VerifyClaim(int id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            // Add custom verification logic if necessary
            if (claim.IsValid())
            {
                claim.Status = "Verified";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
