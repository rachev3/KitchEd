using KitchEd.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Потребителско име")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Биография")]
        public string ShortBio { get; set; }

        [Display(Name = "Роля")]
        public UserRoles Role { get; set; }

        // Helper Properties
        public string FullName => $"{FirstName} {LastName}";

        public string RoleBadgeClass => Role switch
        {
            UserRoles.Admin => "badge bg-danger",
            UserRoles.Chef => "badge bg-primary",
            UserRoles.Student => "badge bg-success",
            _ => "badge bg-secondary"
        };

        public string RoleDisplayName => Role switch
        {
            UserRoles.Admin => "Администратор",
            UserRoles.Chef => "Шеф-инструктор",
            UserRoles.Student => "Студент",
            _ => "Неизвестна"
        };
    }
}