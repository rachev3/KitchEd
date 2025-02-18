using KitchEd.Models.ViewModels.SkillLevel;

namespace KitchEd.Data.Services.Interfaces
{
    public interface ISkillLevelService
    {
        Task<IEnumerable<SkillLevelViewModel>> GetAll();
        Task<SkillLevelViewModel> GetById(int id);
        Task Create(SkillLevelViewModel skillLevelVM);
        Task Update(int id, SkillLevelViewModel skillLevelVM);
        Task Delete(int id);
    }
}
