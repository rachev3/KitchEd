using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.Entities
{
    public class CourseCategory
    {
        [Key]
        public int CourseCategoryId { get; set; }
        public string Name { get; set; }
    }
}
