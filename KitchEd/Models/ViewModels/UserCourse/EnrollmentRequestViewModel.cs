using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.UserCourse
{
    public class EnrollmentRequestViewModel
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public string StudentId { get; set; }

    }
} 