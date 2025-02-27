using KitchEd.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Потребителското име е задължително")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Потребителското име трябва да е между 3 и 25 символа")]

        [Display(Name = "Потребителско име")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email адресът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден email адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(50, ErrorMessage = "Името не може да надвишава 50 символа")]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [StringLength(50, ErrorMessage = "Фамилията не може да надвишава 50 символа")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        [StringLength(500, ErrorMessage = "Биографията не може да надвишава 500 символа")]
        [Display(Name = "Кратка биография")]
        public string? ShortBio { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е поне 6 символа")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Повторете паролата")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Моля, изберете роля")]
        [Display(Name = "Роля")]
        public UserRoles Role { get; set; }

        public string RecaptchaResponse { get; set; }
    }
}
