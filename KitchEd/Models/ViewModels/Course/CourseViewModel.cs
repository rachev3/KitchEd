using KitchEd.Data.Enums;

namespace KitchEd.Models.ViewModels.Course
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MaxParticipants { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatus CourseStatus { get; set; }
        
        // Navigation properties
        public string CategoryName { get; set; }
        public string DishTypeName { get; set; }
        public string SkillLevelName { get; set; }
    }
} 