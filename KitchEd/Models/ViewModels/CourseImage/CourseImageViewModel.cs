using KitchEd.Models.Entities;

namespace KitchEd.Models.ViewModels.CourseImage
{
    public class CourseImageViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int CourseId { get; set; }
        public KitchEd.Models.Entities.Course Course { get; set; }
    }
}