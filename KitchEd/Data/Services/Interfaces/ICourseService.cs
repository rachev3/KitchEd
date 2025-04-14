using KitchEd.Data.Enums;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.Course;

namespace KitchEd.Data.Services.Interfaces
{
    public interface ICourseService
    {
        Task<int> Create(CreateCourseViewModel model, string chefId);

        Task<IEnumerable<CourseViewModel>> GetAll();
        Task<IEnumerable<CourseViewModel>> GetAllNeedApproval();
        Task<CourseViewModel> GetById(int id);
        Task<CourseDetailsViewModel> GetDetailsById(int id, string currentUserId);
        Task<EditCourseViewModel> GetByIdForEdit(int id);
        Task<IEnumerable<CourseViewModel>> GetByChefId(string chefId);
        Task<IEnumerable<CourseViewModel>> GetActiveCourses();

        Task<Course> Update(int id, EditCourseViewModel model);
        Task<bool> UpdateStatus(int id, CourseStatus newStatus);

        Task<bool> Delete(int id);
        Task<bool> IsChefOwner(int courseId, string chefId);
        Task<int> GetEnrolledStudentsCount(int courseId);
        Task<bool> HasAvailableSpots(int courseId);
    }
}
