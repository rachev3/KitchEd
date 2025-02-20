using System.ComponentModel.DataAnnotations;
using KitchEd.Data.Enums;

namespace KitchEd.Models.ViewModels.UserCourse
{
    public class EnrollmentStatusUpdateViewModel
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public UserCourseStatus NewStatus { get; set; }

    }
} 