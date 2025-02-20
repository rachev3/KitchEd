using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.ViewModels.DishType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchEd.Controllers
{
    [AdminOnly]
    public class DishTypeController : BaseController
    {
        private readonly IDishTypeService _dishTypeService;

        public DishTypeController(IDishTypeService dishTypeService)
        {
            _dishTypeService = dishTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var dishTypes = await _dishTypeService.GetAll();
            return View(dishTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishTypeViewModel dishTypeVM)
        {
            if (!ModelState.IsValid) return View(dishTypeVM);

            try
            {
                await _dishTypeService.Create(dishTypeVM);
                TempData["SuccessMessage"] = "Типът ястие е създаден успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Вече съществува такъв тип ястие.";
                return View(dishTypeVM);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dishType = await _dishTypeService.GetById(id);
                return View(dishType);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Типът ястие не бе намерено.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DishTypeViewModel dishTypeVM)
        {
            if (!ModelState.IsValid) return View(dishTypeVM);

            try
            {
                await _dishTypeService.Update(id, dishTypeVM);
                TempData["SuccessMessage"] = "Типът ястие е обновен успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Вече съществува такъв тип ястие.";
                return View(dishTypeVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _dishTypeService.Delete(id);
                TempData["SuccessMessage"] = "Типът ястие е изтрит успешно!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Типът ястие не бе намерено.";
            }

            return RedirectToAction("Index");
        }
    }
}
