using KitchEd.Data.Enums;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.Course;
using Microsoft.EntityFrameworkCore;

namespace KitchEd.Data.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Course> Create(CreateCourseViewModel model, string chefId)
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

            return course;
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
                CategoryName = course.CourseCategory.Name,
                DishTypeName = course.DishType.Name,
                SkillLevelName = course.SkillLevel.Name
            };
        }

        public async Task<IEnumerable<CourseViewModel>> GetByChefId(string chefId)
        {
            return await _context.UserCourses
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
