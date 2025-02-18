using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.SkillLevel
{
    public class SkillLevelViewModel
    {
        public int SkillLevelId { get; set; }

        [Required(ErrorMessage = "Моля, въведете име.")]
        [StringLength(25, ErrorMessage = "Името не може да е повече от 25 символа.")]
        public string Name { get; set; }
    }
}
