using KitchEd.Data.Enums;

namespace KitchEd.Models.ViewModels.UserCourse
{
    public class UserCourseViewModel
    {
        // User Information
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }

        // Course Information
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }
        public string CourseCategoryName { get; set; }
        public string DishTypeName { get; set; }
        public string SkillLevelName { get; set; }

        // Enrollment Information
        public UserCourseStatus Status { get; set; }
        public DateTime SignUpDate { get; set; }
        public string Role { get; set; }

        // Helper Properties
        public string StatusBadgeClass => Status switch
        {
            UserCourseStatus.Pending => "badge bg-warning",
            UserCourseStatus.Approved => "badge bg-success",
            UserCourseStatus.Rejected => "badge bg-danger",
            _ => "badge bg-secondary"
        };

        public string StatusText => Status switch
        {
            UserCourseStatus.Pending => "Изчакване",
            UserCourseStatus.Approved => "Одобрен",
            UserCourseStatus.Rejected => "Отхвърлен",
            _ => "Неизвестен"
        };

        public bool CanBeApproved => Status == UserCourseStatus.Pending;
        public bool CanBeRejected => Status == UserCourseStatus.Pending;
    }
} 