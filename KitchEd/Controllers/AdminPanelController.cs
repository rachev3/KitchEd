using Microsoft.AspNetCore.Mvc;
using KitchEd.Data;

namespace KitchEd.Controllers
{
    [AdminOnly]
    [Route("admin-panel")]
    public class AdminPanelController : BaseController
    {
        private readonly DataSeeder _dataSeeder;

        public AdminPanelController(DataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAdmin)
            {
                return RedirectToHomeWithError("Нямате достъп до тази страница.");
            }

            return View("~/Views/AdminPanel/Home/Index.cshtml");
        }

        [HttpPost]
        [Route("seed")]
        public async Task<IActionResult> Seed()
        {
            if (!IsAdmin)
            {
                return RedirectToHomeWithError("Нямате достъп до тази функционалност.");
            }

            try
            {
                await _dataSeeder.SeedAllAsync();
                TempData["SuccessMessage"] = "Базата данни е успешно заредена с примерни данни.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Грешка при зареждане на данни: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}