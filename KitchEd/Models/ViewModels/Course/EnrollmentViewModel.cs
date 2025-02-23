using KitchEd.Data.Enums;

namespace KitchEd.Models.ViewModels.Course;

public class EnrollmentViewModel
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string ChefName { get; set; } = null!;
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrolledOn { get; set; }
    public DateTime? StatusChangedOn { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CourseStartDate { get; set; }
    public DateTime CourseEndDate { get; set; }

    public string GetStatusName()
    {
        return Status switch
        {
            EnrollmentStatus.Pending => "Чакащ одобрение",
            EnrollmentStatus.Approved => "Одобрен",
            EnrollmentStatus.Rejected => "Отхвърлен",
            EnrollmentStatus.Completed => "Завършен",
            _ => "Неизвестен"
        };
    }

    public string GetStatusBadgeClass()
    {
        return Status switch
        {
            EnrollmentStatus.Pending => "secondary",
            EnrollmentStatus.Approved => "success",
            EnrollmentStatus.Rejected => "danger",
            EnrollmentStatus.Completed => "info",
            _ => "secondary"
        };
    }
}