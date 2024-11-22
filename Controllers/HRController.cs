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
