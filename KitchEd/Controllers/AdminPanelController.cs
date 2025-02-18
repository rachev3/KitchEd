using Microsoft.AspNetCore.Mvc;

namespace KitchEd.Controllers
{
    //[Authorize(Roles = "Admin")] // Restricts access to Admins only
    public class AdminPanelController : Controller
    {
        // Dashboard (Main Admin Panel Page)
        public ActionResult Index()
        {
            return View("~/Views/AdminPanel/Home/Index.cshtml");
        }

      
    }                                                    
}