using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.DishType
{
    public class DishTypeViewModel
    {
        public int DishTypeId { get; set; }

        [Required(ErrorMessage = "Моля, въведете име.")]
        [StringLength(50, ErrorMessage = "Името трябва да е между 3 и 50 символа.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}
