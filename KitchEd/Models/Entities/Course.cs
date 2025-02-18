using KitchEd.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchEd.Models.Entities
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MaxParticipants { get; set; }
        public string MainImageUrl { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public CourseStatus CourseStatus { get; set; }     // inactive, active, ongoing, completed

        public int CourseCategoryId { get; set; }
        [ForeignKey(nameof(CourseCategoryId))]
        public virtual CourseCategory? CourseCategory { get; set; }


        public int DishTypeId { get; set; }
        [ForeignKey(nameof(DishTypeId))]
        public virtual DishType? DishType { get; set; }


        public int SkillLevelId { get; set; }
        [ForeignKey(nameof(SkillLevelId))]
        public virtual SkillLevel? SkillLevel { get; set; }



        public ICollection<UserCourse> UserCourses { get; set; }
        public List<CourseImage> CourseImages { get; set; }

    }
}
