using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KitchEd.Data.Enums;

namespace KitchEd.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool IsAdmin => User.IsInRole(UserRoles.Admin.ToString());
        protected bool IsChef => User.IsInRole(UserRoles.Chef.ToString());
        protected bool IsStudent => User.IsInRole(UserRoles.Student.ToString());

        protected IActionResult RedirectToHomeWithError(string error)
        {
            TempData["ErrorMessage"] = error;
            return RedirectToAction("Index", "Home");
        }

        protected bool HasAccessToResource(string userId)
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value == userId;
        }
    }

    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = UserRoles.Admin.ToString();
        }
    }

    public class ChefOnlyAttribute : AuthorizeAttribute
    {
        public ChefOnlyAttribute()
        {
            Roles = UserRoles.Chef.ToString();
        }
    }

    public class StudentOnlyAttribute : AuthorizeAttribute
    {
        public StudentOnlyAttribute()
        {
            Roles = UserRoles.Student.ToString();
        }
    }
}