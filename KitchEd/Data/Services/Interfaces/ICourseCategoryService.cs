using KitchEd.Models.ViewModels.CourseCategory;

namespace KitchEd.Data.Services.Interfaces
{
    public interface ICourseCategoryService
    {
        Task<IEnumerable<CourseCategoryViewModel>> GetAll();
        Task<CourseCategoryViewModel> GetById(int id);
        Task Create(CourseCategoryViewModel courseCategoryVM);
        Task Update(int id, CourseCategoryViewModel courseCategoryVM);
        Task Delete(int id);
    }
}
