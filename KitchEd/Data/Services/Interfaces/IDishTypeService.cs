using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.DishType;

namespace KitchEd.Data.Services.Interfaces
{
    public interface IDishTypeService
    {
        Task<IEnumerable<DishTypeViewModel>> GetAll();
        Task<DishTypeViewModel> GetById(int id);
        Task Create(DishTypeViewModel viewModel);
        Task Update(int id, DishTypeViewModel viewModel);
        Task Delete(int id);
    }
}
