using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.ViewModels.SkillLevel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchEd.Controllers
{
    //[Authorize(Roles = "Admin")] 
    public class SkillLevelController : Controller
    {
        private readonly ISkillLevelService _skillLevelService;

        public SkillLevelController(ISkillLevelService skillLevelService)
        {
            _skillLevelService = skillLevelService;
        }

        public async Task<IActionResult> Index()
        {
            var skillLevels = await _skillLevelService.GetAll();
            return View(skillLevels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SkillLevelViewModel skillLevelVM)
        {
            if (!ModelState.IsValid) return View(skillLevelVM);

            try
            {
                await _skillLevelService.Create(skillLevelVM);
                TempData["SuccessMessage"] = "Нивото на умение е създадено успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(skillLevelVM);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var skillLevel = await _skillLevelService.GetById(id);
                return View(skillLevel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SkillLevelViewModel skillLevelVM)
        {
            if (!ModelState.IsValid) return View(skillLevelVM);

            try
            {
                await _skillLevelService.Update(id, skillLevelVM);
                TempData["SuccessMessage"] = "Нивото на умение е обновено успешно!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(skillLevelVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _skillLevelService.Delete(id);
                TempData["SuccessMessage"] = "Нивото на умение е изтрито успешно!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
