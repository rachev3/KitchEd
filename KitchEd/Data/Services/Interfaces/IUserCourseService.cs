using KitchEd.Data.Enums;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.Course;
using KitchEd.Models.ViewModels.UserCourse;

namespace KitchEd.Data.Services.Interfaces
{
    public interface IUserCourseService
    {
        // Student Enrollment
        Task<UserCourseViewModel> EnrollStudent(CourseDetailsViewModel model, string userId);
        Task<bool> UpdateStudentStatus(EnrollmentStatusUpdateViewModel request);
        Task<bool> IsStudentEnrolled(int courseId, string studentId);

        // Enrollment Management
        Task<IEnumerable<UserCourseViewModel>> GetCourseEnrollments(int courseId);
        Task<IEnumerable<UserCourseViewModel>> GetStudentEnrollments(string studentId);
        Task<IEnumerable<UserCourseViewModel>> GetPendingEnrollments(int courseId);

        // Course Transfer (when Chef is deleted)
        Task<bool> TransferCourseOwnership(int courseId, string fromChefId, string toChefId);
        Task<bool> TransferAllCoursesOwnership(string fromChefId, string toChefId);
    }
}