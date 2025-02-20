using Microsoft.AspNetCore.Mvc;

namespace KitchEd.Controllers
{
    [AdminOnly]
    [Route("admin-panel")]
    public class AdminPanelController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAdmin)
            {
                return RedirectToHomeWithError("Нямате достъп до тази страница.");
            }
            
            return View("~/Views/AdminPanel/Home/Index.cshtml");
        }
    }                                                    
}