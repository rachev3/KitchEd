using KitchEd.Data.Enums;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.ViewModels.Course;
using KitchEd.Models.ViewModels.UserCourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KitchEd.Models.Entities;

namespace KitchEd.Controllers
{
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly ICourseCategoryService _categoryService;
        private readonly IDishTypeService _dishTypeService;
        private readonly ISkillLevelService _skillLevelService;
        private readonly IUserCourseService _userCourseService;
        private readonly UserManager<User> _userManager;

        public CourseController(
            ICourseService courseService,
            ICourseCategoryService categoryService,
            IDishTypeService dishTypeService,
            ISkillLevelService skillLevelService,
            IUserCourseService userCourseService,
            UserManager<User> userManager)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _dishTypeService = dishTypeService;
            _skillLevelService = skillLevelService;
            _userCourseService = userCourseService;
            _userManager = userManager;
        }

        // GET: /Course
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetActiveCourses();
            return View(courses);
        }

        // GET: /Course/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var details = await _courseService.GetDetailsById(id, currentUserId);
            return View(details);
        }

        // GET: /Course/Create
        [ChefOnly]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAll();
            var dishTypes = await _dishTypeService.GetAll();
            var skillLevels = await _skillLevelService.GetAll();
            return View(new CreateCourseViewModel
            {
                Categories = categories.ToList(),
                DishTypes = dishTypes.ToList(),
                SkillLevels = skillLevels.ToList()
            });
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChefOnly]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAll();
                ViewBag.DishTypes = await _dishTypeService.GetAll();
                ViewBag.SkillLevels = await _skillLevelService.GetAll();
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            await _courseService.Create(model, userId);

            TempData["SuccessMessage"] = "Курсът е създаден успешно и очаква одобрение от администратор.";
            return RedirectToAction(nameof(MyCourses));
        }

        // GET: /Course/Edit/5
        [ChefOnly]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (!await _courseService.IsChefOwner(id, userId))
            {
                return RedirectToHomeWithError("Нямате достъп до този курс.");
            }

            var course = await _courseService.GetById(id);
            Console.WriteLine(course.CategoryName);
            if (course == null)
            {
                return NotFound();
            }

            if (course.Status == CourseStatus.Active)
            {
                return RedirectToHomeWithError("Не можете да редактирате активен курс.");
            }

            var categories = await _categoryService.GetAll();
            var dishTypes = await _dishTypeService.GetAll();
            var skillLevels = await _skillLevelService.GetAll();

            var editModel = new EditCourseViewModel
            {
                Categories = categories.ToList(),
                DishTypes = dishTypes.ToList(),
                SkillLevels = skillLevels.ToList(),
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                MaxParticipants = course.MaxParticipants,
                MainImageUrl = course.MainImageUrl,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                CategoryId = course.CategoryId,
                DishTypeId = course.DishTypeId,
                SkillLevelId = course.SkillLevelId
            };

            return View(editModel);
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChefOnly]
        public async Task<IActionResult> Edit(int id, EditCourseViewModel model)
        {
            if (id != model.CourseId)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!await _courseService.IsChefOwner(id, userId))
            {
                return RedirectToHomeWithError("Нямате достъп до този курс.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAll();
                ViewBag.DishTypes = await _dishTypeService.GetAll();
                ViewBag.SkillLevels = await _skillLevelService.GetAll();
                return View(model);
            }

            await _courseService.Update(id, model);
            TempData["SuccessMessage"] = "Курсът е обновен успешно.";
            return RedirectToAction(nameof(MyCourses));
        }

        // GET: /Course/MyCourses
        [ChefOnly]
        public async Task<IActionResult> MyCourses()
        {
            var userId = _userManager.GetUserId(User);
            var courses = await _courseService.GetByChefId(userId);
            return View(courses);
        }

        // POST: /Course/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChefOnly]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (!await _courseService.IsChefOwner(id, userId))
            {
                return RedirectToHomeWithError("Нямате достъп до този курс.");
            }

            await _courseService.Delete(id);
            TempData["SuccessMessage"] = "Курсът е изтрит успешно.";
            return RedirectToAction(nameof(MyCourses));
        }

        // POST: /Course/Enroll/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [StudentOnly]
        public async Task<IActionResult> Enroll(EnrollmentRequestViewModel model)
        {
            if (!await _courseService.HasAvailableSpots(model.CourseId))
            {
                return RedirectToHomeWithError("Няма свободни места в този курс.");
            }

            try
            {
                model.StudentId = _userManager.GetUserId(User);
                await _userCourseService.EnrollStudent(model);
                TempData["SuccessMessage"] = "Записахте се успешно за курса. Изчакайте одобрение от преподавателя.";
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToHomeWithError(ex.Message);
            }

            return RedirectToAction(nameof(Details), new { id = model.CourseId });
        }

        // POST: /Course/ApproveStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChefOnly]
        public async Task<IActionResult> ApproveStudent(EnrollmentStatusUpdateViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (!await _courseService.IsChefOwner(model.CourseId, userId))
            {
                return RedirectToHomeWithError("Нямате достъп до този курс.");
            }

            model.NewStatus = UserCourseStatus.Approved;
            await _userCourseService.UpdateStudentStatus(model);
            TempData["SuccessMessage"] = "Ученикът е одобрен успешно.";
            return RedirectToAction(nameof(Details), new { id = model.CourseId });
        }

        // POST: /Course/RejectStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChefOnly]
        public async Task<IActionResult> RejectStudent(EnrollmentStatusUpdateViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (!await _courseService.IsChefOwner(model.CourseId, userId))
            {
                return RedirectToHomeWithError("Нямате достъп до този курс.");
            }

            model.NewStatus = UserCourseStatus.Rejected;
            await _userCourseService.UpdateStudentStatus(model);
            TempData["SuccessMessage"] = "Ученикът е отхвърлен успешно.";
            return RedirectToAction(nameof(Details), new { id = model.CourseId });
        }

        // GET: /Course/MyEnrollments
        [StudentOnly]
        public async Task<IActionResult> MyEnrollments()
        {
            var userId = _userManager.GetUserId(User);
            var enrollments = await _userCourseService.GetStudentEnrollments(userId);
            return View(enrollments);
        }
    }
}