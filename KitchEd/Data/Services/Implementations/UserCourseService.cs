using KitchEd.Data.Enums;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.UserCourse;
using Microsoft.EntityFrameworkCore;

namespace KitchEd.Data.Services.Implementations
{
    public class UserCourseService : IUserCourseService
    {
        private readonly ApplicationDbContext _context;

        public UserCourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserCourseViewModel> EnrollStudent(EnrollmentRequestViewModel request)
        {
            // Check if student is already enrolled
            var existingEnrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.CourseId == request.CourseId && 
                                         uc.UserId == request.StudentId && 
                                         uc.Role == UserRoles.Student.ToString());

            if (existingEnrollment != null)
            {
                throw new InvalidOperationException("Вече сте записани в този курс.");
            }

            var enrollment = new UserCourse
            {
                CourseId = request.CourseId,
                UserId = request.StudentId,
                Role = UserRoles.Student.ToString(),
                Status = UserCourseStatus.Pending,
                SignUpDate = DateTime.UtcNow
            };

            await _context.UserCourses.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            // Load the related data and return view model
            var enrollmentWithDetails = await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.CourseCategory)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.DishType)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.SkillLevel)
                .FirstAsync(uc => uc.UserCourseId == enrollment.UserCourseId);

            return MapToViewModel(enrollmentWithDetails);
        }

        public async Task<bool> UpdateStudentStatus(EnrollmentStatusUpdateViewModel request)
        {
            var enrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.CourseId == request.CourseId && 
                                         uc.UserId == request.StudentId && 
                                         uc.Role == UserRoles.Student.ToString());

            if (enrollment == null)
            {
                return false;
            }

            enrollment.Status = request.NewStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsStudentEnrolled(int courseId, string studentId)
        {
            return await _context.UserCourses
                .AnyAsync(uc => uc.CourseId == courseId && 
                               uc.UserId == studentId && 
                               uc.Role == UserRoles.Student.ToString());
        }

        public async Task<IEnumerable<UserCourseViewModel>> GetCourseEnrollments(int courseId)
        {
            var enrollments = await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.CourseCategory)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.DishType)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.SkillLevel)
                .Where(uc => uc.CourseId == courseId && 
                           uc.Role == UserRoles.Student.ToString())
                .ToListAsync();

            return enrollments.Select(MapToViewModel);
        }

        public async Task<IEnumerable<UserCourseViewModel>> GetStudentEnrollments(string studentId)
        {
            var enrollments = await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.CourseCategory)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.DishType)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.SkillLevel)
                .Where(uc => uc.UserId == studentId && 
                           uc.Role == UserRoles.Student.ToString())
                .ToListAsync();

            return enrollments.Select(MapToViewModel);
        }

        public async Task<IEnumerable<UserCourseViewModel>> GetPendingEnrollments(int courseId)
        {
            var enrollments = await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.CourseCategory)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.DishType)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.SkillLevel)
                .Where(uc => uc.CourseId == courseId && 
                           uc.Role == UserRoles.Student.ToString() && 
                           uc.Status == UserCourseStatus.Pending)
                .ToListAsync();

            return enrollments.Select(MapToViewModel);
        }

        public async Task<bool> TransferCourseOwnership(int courseId, string fromChefId, string toChefId)
        {
            var chefCourse = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.CourseId == courseId && 
                                         uc.UserId == fromChefId && 
                                         uc.Role == UserRoles.Chef.ToString());

            if (chefCourse == null)
            {
                return false;
            }

            chefCourse.UserId = toChefId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TransferAllCoursesOwnership(string fromChefId, string toChefId)
        {
            var chefCourses = await _context.UserCourses
                .Where(uc => uc.UserId == fromChefId && 
                           uc.Role == UserRoles.Chef.ToString())
                .ToListAsync();

            if (!chefCourses.Any())
            {
                return false;
            }

            foreach (var course in chefCourses)
            {
                course.UserId = toChefId;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private static UserCourseViewModel MapToViewModel(UserCourse userCourse)
        {
            return new UserCourseViewModel
            {
                UserId = userCourse.UserId,
                UserFullName = $"{userCourse.User.FirstName} {userCourse.User.LastName}",
                UserEmail = userCourse.User.Email,
                UserPhoneNumber = userCourse.User.PhoneNumber,

                CourseId = userCourse.CourseId,
                CourseTitle = userCourse.Course.Title,
                CourseStartDate = userCourse.Course.StartDate,
                CourseEndDate = userCourse.Course.EndDate,
                CourseCategoryName = userCourse.Course.CourseCategory.Name,
                DishTypeName = userCourse.Course.DishType.Name,
                SkillLevelName = userCourse.Course.SkillLevel.Name,

                Status = userCourse.Status,
                SignUpDate = userCourse.SignUpDate,
                Role = userCourse.Role
            };
        }
    }
}