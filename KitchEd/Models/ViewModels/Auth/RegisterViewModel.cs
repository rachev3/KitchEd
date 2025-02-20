using KitchEd.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Моля, въведете потребителско име.")]
        [StringLength(25, ErrorMessage = "Потребителското име не може да надвишава 25 символа.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Моля, въведете email.")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля, въведете име.")]
        [StringLength(50, ErrorMessage = "Името не може да надвишава 50 символа.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Моля, въведете фамилия.")]
        [StringLength(50, ErrorMessage = "Фамилията не може да надвишава 50 символа.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Моля, изберете роля.")]
        [EnumDataType(typeof(UserRoles))]
        public UserRoles Role { get; set; }

        [Phone(ErrorMessage = "Моля, въведете валиден телефонен номер.")]
        public string PhoneNumber { get; set; } // Optional

        [StringLength(200, ErrorMessage = "Описанието не може да надвишава 200 символа.")]
        public string ShortBio { get; set; }   // Optional

        [Required(ErrorMessage = "Моля, въведете парола.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да бъде между 6 и 100 символа.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Моля, повторете паролата.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }
}
