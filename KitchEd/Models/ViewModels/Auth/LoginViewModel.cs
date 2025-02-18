using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Моля, въведете email.")]
        [EmailAddress(ErrorMessage = "Моля, въведете валиден email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля, въведете парола.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
