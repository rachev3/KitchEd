using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.ViewModels.CourseCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KitchEd.Controllers
{
    //[Authorize(Roles = "Admin")] 
    public class CourseCategoryController : Controller
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid) return View(categoryVM);

            try
            {
                await _courseCategoryService.Create(categoryVM);
                TempData["SuccessMessage"] = "Категорията е създадена успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(categoryVM);
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid) return View(categoryVM);

            try
            {
                await _courseCategoryService.Update(id, categoryVM);
                TempData["SuccessMessage"] = "Категорията е обновена успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(categoryVM);
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
