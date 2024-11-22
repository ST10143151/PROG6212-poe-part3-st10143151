// Code Attribution:

//Allen, D. and Foster, A., 2022. Pro ASP.NET Core Identity: Under the Hood of Authentication and Authorization 
//for ASP.NET Core Applications. New York: Apress.
//Esposito, D., 2021. Modern Web Development with ASP.NET Core 5: An end-to-end guide to becoming a professional 
//full-stack web developer. Birmingham: Packt Publishing.
//Johnson, M., 2019. Improving efficiency through digital claims management. International Journal of Systems and 
//Applications, 7(4), pp.102-115.
//Microsoft, 2023. ASP.NET Core MVC Overview. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0 [Accessed 18 October 2024].
//Microsoft, 2024. Entity Framework Core Documentation. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/ef/core/ [Accessed 18 November 2024].
//Microsoft, 2024. Introduction to Identity on ASP.NET Core. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0 [Accessed 18 November 2024].
//Microsoft, 2024. Configure ASP.NET Core Identity. Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-9.0 [Accessed 18 November 2024].
//Microsoft, 2024. EF Core Tools Reference (.NET CLI). Microsoft Docs. Available at: 
//https://learn.microsoft.com/en-us/ef/core/cli/dotnet/ [Accessed 18 November 2024].
//Troelsen, A. and Japikse, P., 2021. Pro C# 9 with .NET 5: Foundational Principles and Practices in Programming.
//Full code attribution in Readme.md

using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CMCS.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HRController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        // Generate Report for Approved Claims
        public IActionResult GenerateReport()
        {
            var approvedClaims = _context.Claims.Where(c => c.Status == "Approved").ToList();
            var report = new ClaimReportGenerator(approvedClaims).GeneratePdf();
            return File(report, "application/pdf", "ClaimSummary.pdf");
        }

        // Update Lecturer Information

        [HttpPost]
        public async Task<IActionResult> UpdateLecturer(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return NotFound();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            await _userManager.UpdateAsync(existingUser);

            return RedirectToAction(nameof(Index)); // Adjust to redirect to the relevant action
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ManageLecturers()
        {
            var lecturerRole = await _userManager.GetUsersInRoleAsync("Lecturer");
            return View(lecturerRole);
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> EditLecturer(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lecturer = await _userManager.FindByIdAsync(id);
            if (lecturer == null) return NotFound();

            return View(lecturer);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> EditLecturer(User model)
        {
            if (!ModelState.IsValid) return View(model);

            var lecturer = await _userManager.FindByIdAsync(model.Id);
            if (lecturer == null) return NotFound();

            lecturer.Name = model.Name;
            lecturer.Email = model.Email;
            lecturer.Department = model.Department;
            lecturer.Office = model.Office;

            var result = await _userManager.UpdateAsync(lecturer);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ManageLecturers));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> DeleteLecturer(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lecturer = await _userManager.FindByIdAsync(id);
            if (lecturer == null) return NotFound();

            return View(lecturer);
        }

        [HttpPost, ActionName("DeleteLecturer")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ConfirmDeleteLecturer(string id)
        {
            var lecturer = await _userManager.FindByIdAsync(id);
            if (lecturer == null) return NotFound();

            var result = await _userManager.DeleteAsync(lecturer);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ManageLecturers));
            }

            ModelState.AddModelError(string.Empty, "Failed to delete the lecturer.");
            return View(lecturer);
        }



    }
}
