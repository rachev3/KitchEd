using KitchEd.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KitchEd.Controllers
{
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // public async Task<IActionResult> Index()
        // {
        //     var courses = await _courseService.;
        //     return View(courses);
        // }
    }
}