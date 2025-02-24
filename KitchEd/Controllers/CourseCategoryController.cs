using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.ViewModels.CourseCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KitchEd.Controllers
{
    [AdminOnly]
    public class CourseCategoryController : BaseController
    {
        private readonly ICourseCategoryService _courseCategoryService;

        public CourseCategoryController(ICourseCategoryService courseCategoryService)
        {
            _courseCategoryService = courseCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _courseCategoryService.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            try
            {
                await _courseCategoryService.Create(viewModel);
                TempData["SuccessMessage"] = "Категорията е създадена успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(viewModel);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _courseCategoryService.GetById(id);
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CourseCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            try
            {
                await _courseCategoryService.Update(id, viewModel);
                TempData["SuccessMessage"] = "Категорията е обновена успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(viewModel);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _courseCategoryService.Delete(id);
                TempData["SuccessMessage"] = "Категорията е изтрита успешно!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
