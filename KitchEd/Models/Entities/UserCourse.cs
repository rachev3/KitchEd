using KitchEd.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchEd.Models.Entities
{
    public class UserCourse
    {
        [Key]
        public int UserCourseId { get; set; }


        public string Role { get; set; } // "Chef" or "Student"
        public DateTime SignUpDate { get; set; }
        public UserCourseStatus Status { get; set; } // "Pending", "Approved", "Rejected"

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
    }

}
