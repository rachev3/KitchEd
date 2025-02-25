using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace KitchEd.Controllers
{
    [AdminOnly]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewData["CurrentFilter"] = searchTerm;

            var users = await _userService.GetAllUsers();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                users = users.Where(u =>
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm) ||
                    u.Username.ToLower().Contains(searchTerm)
                ).ToList();
            }

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.UpdateUser(id, model);
            if (!result)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Потребителят е обновен успешно.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.TransferCoursesAndDeleteUser(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Възникна грешка при изтриването на потребителя.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Потребителят е изтрит успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}