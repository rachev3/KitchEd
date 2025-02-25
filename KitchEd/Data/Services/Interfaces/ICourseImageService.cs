using KitchEd.Models.ViewModels.CourseImage;

namespace KitchEd.Data.Services.Interfaces
{
    public interface ICourseImageService
    {
        Task<IEnumerable<CourseImageViewModel>> GetAllCourseImages(int courseId);
        Task<CourseImageViewModel> GetById(int id);
        Task Create(CourseImageViewModel viewModel);
        Task Update(int id, CourseImageViewModel viewModel);
        Task Delete(int id);
    }
}