using KitchEd.Data.Enums;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KitchEd.Data.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserCourseService _userCourseService;

        public UserService(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IUserCourseService userCourseService)
        {
            _context = context;
            _userManager = userManager;
            _userCourseService = userCourseService;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = Enum.Parse<UserRoles>(roles.First());

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    ShortBio = user.ShortBio,
                    Role = role
                });
            }

            return userViewModels;
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = Enum.Parse<UserRoles>(roles.First());

            return new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ShortBio = user.ShortBio,
                Role = role
            };
        }

        public async Task<bool> UpdateUser(string id, UserViewModel model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.ShortBio = model.ShortBio;
            user.Email = model.Email;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, model.Role.ToString());

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(UserRoles.Chef.ToString()))
            {
                // Transfer courses to admin before deletion
                var adminUser = await _userManager.GetUsersInRoleAsync(UserRoles.Admin.ToString());
                await _userCourseService.TransferAllCoursesOwnership(user.Id, adminUser.First().Id);
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> TransferCoursesAndDeleteUser(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(UserRoles.Chef.ToString()))
            {
                var adminUser = await _userManager.GetUsersInRoleAsync(UserRoles.Admin.ToString());
                await _userCourseService.TransferAllCoursesOwnership(user.Id, adminUser.First().Id);
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}