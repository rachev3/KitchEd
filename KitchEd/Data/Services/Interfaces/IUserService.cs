using KitchEd.Models.ViewModels.User;

namespace KitchEd.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<UserViewModel> GetUserById(string id);
        Task<bool> UpdateUser(string id, UserViewModel model);
        Task<bool> TransferCoursesAndDeleteUser(string userId);
    }
}