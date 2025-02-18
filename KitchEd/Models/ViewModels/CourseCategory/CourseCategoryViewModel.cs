using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.CourseCategory
{
    public class CourseCategoryViewModel
    {
        public int CourseCategoryId { get; set; }

        [Required(ErrorMessage = "Моля въведете име.")]
        [StringLength(25, ErrorMessage = "Името на категорията не може да надвишава 25 символа.")]
        public string Name { get; set; }
    }
}
