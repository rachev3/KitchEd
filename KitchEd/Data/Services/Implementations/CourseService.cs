using KitchEd.Data.Enums;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.Course;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KitchEd.Data.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ICourseCategoryService _categoryService;
        private readonly IDishTypeService _dishTypeService;
        private readonly ISkillLevelService _skillLevelService;

        public CourseService(ApplicationDbContext context, UserManager<User> userManager, ICourseCategoryService categoryService, IDishTypeService dishTypeService, ISkillLevelService skillLevelService)
        {
            _context = context;
            _userManager = userManager;
            _categoryService = categoryService;
            _dishTypeService = dishTypeService;
            _skillLevelService = skillLevelService;
        }

        // Create
        public async Task<int> Create(CreateCourseViewModel model, string chefId)
        {
            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                MaxParticipants = model.MaxParticipants,
                MainImageUrl = model.MainImageUrl,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CourseStatus = CourseStatus.Inactive,
                CourseCategoryId = model.CategoryId,
                DishTypeId = model.DishTypeId,
                SkillLevelId = model.SkillLevelId
            };

            // Add the chef as the course owner
            var userCourse = new UserCourse
            {
                UserId = chefId,
                Course = course,
                Status = UserCourseStatus.Pending,
                SignUpDate = DateTime.UtcNow,
                Role = UserRoles.Chef.ToString()
            };

            await _context.Courses.AddAsync(course);
            await _context.UserCourses.AddAsync(userCourse);
            await _context.SaveChangesAsync();

            return course.CourseId;
        }

        // Read
        public async Task<IEnumerable<CourseViewModel>> GetAll()
        {
            return await _context.Courses
                .Include(c => c.CourseCategory)
                .Include(c => c.DishType)
                .Include(c => c.SkillLevel)
                .Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    MaxParticipants = c.MaxParticipants,
                    MainImageUrl = c.MainImageUrl,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.CourseStatus,
                    CategoryName = c.CourseCategory.Name,
                    DishTypeName = c.DishType.Name,
                    SkillLevelName = c.SkillLevel.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseViewModel>> GetAllNeedApproval()
        {
            return await _context.Courses
                .Where(c => c.CourseStatus == CourseStatus.Inactive)
                .Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    MaxParticipants = c.MaxParticipants,
                    MainImageUrl = c.MainImageUrl,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.CourseStatus,
                    CategoryName = c.CourseCategory.Name,
                    DishTypeName = c.DishType.Name,
                    SkillLevelName = c.SkillLevel.Name,
                    CategoryId = c.CourseCategoryId,
                    DishTypeId = c.DishTypeId,
                    SkillLevelId = c.SkillLevelId
                })
                .ToListAsync();
        }

        public async Task<CourseViewModel> GetById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseCategory)
                .Include(c => c.DishType)
                .Include(c => c.SkillLevel)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return null;

            return new CourseViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                MaxParticipants = course.MaxParticipants,
                MainImageUrl = course.MainImageUrl,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Status = course.CourseStatus,
                CategoryId = course.CourseCategoryId,
                DishTypeId = course.DishTypeId,
                SkillLevelId = course.SkillLevelId,
                CategoryName = course.CourseCategory.Name,
                DishTypeName = course.DishType.Name,
                SkillLevelName = course.SkillLevel.Name
            };
        }

        public async Task<CourseDetailsViewModel> GetDetailsById(int id, string currentUserId = null)
        {
            var course = await _context.Courses
                .Include(c => c.CourseCategory)
                .Include(c => c.DishType)
                .Include(c => c.SkillLevel)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            var courseDetails = new CourseDetailsViewModel();
            if (currentUserId == null)
            {
                courseDetails.IsOwner = false;
                courseDetails.CanEnroll = false;
                courseDetails.UserEnrollmentStatus = null;
            }
            else
            {


                courseDetails.IsOwner = await IsChefOwner(id, currentUserId);
                courseDetails.CanEnroll = await HasAvailableSpots(id);
                courseDetails.UserEnrollmentStatus = await GetEnrollmentStatus(id, currentUserId);
            }
            var chef = await _context.UserCourses
                .Include(uc => uc.User)
                .FirstOrDefaultAsync(uc => uc.CourseId == id && uc.Role == UserRoles.Chef.ToString());

            courseDetails.ChefName = chef.User.FirstName + " " + chef.User.LastName;
            courseDetails.ChefBio = chef.User.ShortBio;
            courseDetails.StatusName = course.CourseStatus.ToString();
            courseDetails.StatusBadgeClass = course.CourseStatus.ToString().ToLower();

            courseDetails.CourseId = course.CourseId;
            courseDetails.Title = course.Title;
            courseDetails.Description = course.Description;
            courseDetails.Price = course.Price;
            courseDetails.MaxParticipants = course.MaxParticipants;
            courseDetails.MainImageUrl = course.MainImageUrl;
            courseDetails.StartDate = course.StartDate;
            courseDetails.EndDate = course.EndDate;
            courseDetails.CategoryId = course.CourseCategoryId;
            courseDetails.DishTypeId = course.DishTypeId;
            courseDetails.SkillLevelId = course.SkillLevelId;
            courseDetails.CategoryName = course.CourseCategory.Name;
            courseDetails.DishTypeName = course.DishType.Name;
            courseDetails.SkillLevelName = course.SkillLevel.Name;
            courseDetails.CurrentParticipants = await GetEnrolledStudentsCount(id);
            courseDetails.Status = course.CourseStatus;


            return courseDetails;


        }

        public async Task<EditCourseViewModel> GetByIdForEdit(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseImages)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return null;

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
                CategoryId = course.CourseCategoryId,
                DishTypeId = course.DishTypeId,
                SkillLevelId = course.SkillLevelId,
                Status = course.CourseStatus,
                ExistingCourseImages = course.CourseImages?.Select(ci => new Models.ViewModels.CourseImage.CourseImageViewModel
                {
                    Id = ci.CourseImageId,
                    ImageUrl = ci.ImageUrl,
                    CourseId = ci.CourseId
                }).ToList() ?? new List<Models.ViewModels.CourseImage.CourseImageViewModel>()
            };

            return editModel;
        }


        private async Task<string?> GetEnrollmentStatus(int courseId, string currentUserId)
        {
            var userCourse = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.CourseId == courseId && uc.UserId == currentUserId);

            if (userCourse == null) return null;

            return userCourse.Status.ToString();
        }

        public async Task<IEnumerable<CourseViewModel>> GetByChefId(string chefId)
        {
            var courses = await _context.UserCourses
                .Where(uc => uc.UserId == chefId && uc.Role == UserRoles.Chef.ToString())
                .Include(uc => uc.Course)
                .ThenInclude(c => c.CourseCategory)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.DishType)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.SkillLevel)
                .Select(uc => new CourseViewModel
                {
                    CourseId = uc.Course.CourseId,
                    Title = uc.Course.Title,
                    Description = uc.Course.Description,
                    Price = uc.Course.Price,
                    MaxParticipants = uc.Course.MaxParticipants,
                    MainImageUrl = uc.Course.MainImageUrl,
                    StartDate = uc.Course.StartDate,
                    EndDate = uc.Course.EndDate,
                    Status = uc.Course.CourseStatus,
                    CategoryName = uc.Course.CourseCategory.Name,
                    DishTypeName = uc.Course.DishType.Name,
                    SkillLevelName = uc.Course.SkillLevel.Name
                })
                .ToListAsync();

            // Get enrolled students count for each course
            foreach (var course in courses)
            {
                course.CurrentParticipants = await GetEnrolledStudentsCount(course.CourseId);
            }

            return courses;
        }

        public async Task<IEnumerable<CourseViewModel>> GetActiveCourses()
        {
            return await _context.Courses
                .Where(c => c.CourseStatus == CourseStatus.Active)
                .Include(c => c.CourseCategory)
                .Include(c => c.DishType)
                .Include(c => c.SkillLevel)
                .Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    MaxParticipants = c.MaxParticipants,
                    MainImageUrl = c.MainImageUrl,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.CourseStatus,
                    CategoryName = c.CourseCategory.Name,
                    DishTypeName = c.DishType.Name,
                    SkillLevelName = c.SkillLevel.Name
                })
                .ToListAsync();
        }

        // Update
        public async Task<Course> Update(int id, EditCourseViewModel model)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            course.Title = model.Title;
            course.Description = model.Description;
            course.Price = model.Price;
            course.MaxParticipants = model.MaxParticipants;
            course.MainImageUrl = model.MainImageUrl;
            course.StartDate = model.StartDate;
            course.EndDate = model.EndDate;
            course.CourseCategoryId = model.CategoryId;
            course.DishTypeId = model.DishTypeId;
            course.SkillLevelId = model.SkillLevelId;

            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> UpdateStatus(int id, CourseStatus newStatus)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            course.CourseStatus = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete
        public async Task<bool> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        // Additional operations
        public async Task<bool> IsChefOwner(int courseId, string chefId)
        {
            return await _context.UserCourses
                .AnyAsync(uc => uc.CourseId == courseId &&
                               uc.UserId == chefId &&
                               uc.Role == UserRoles.Chef.ToString());
        }

        public async Task<int> GetEnrolledStudentsCount(int courseId)
        {
            return await _context.UserCourses
                .CountAsync(uc => uc.CourseId == courseId &&
                                 uc.Role == UserRoles.Student.ToString() &&
                                 uc.Status == UserCourseStatus.Approved);
        }

        public async Task<bool> HasAvailableSpots(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.UserCourses)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null) return false;

            var enrolledStudents = course.UserCourses
                .Count(uc => uc.Role == UserRoles.Student.ToString() &&
                            uc.Status == UserCourseStatus.Approved);

            return enrolledStudents < course.MaxParticipants;
        }


    }
}
