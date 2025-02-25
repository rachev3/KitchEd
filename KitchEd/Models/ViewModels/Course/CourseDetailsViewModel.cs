using KitchEd.Data.Enums;
using KitchEd.Models.ViewModels.CourseImage;

namespace KitchEd.Models.ViewModels.Course;

public class CourseDetailsViewModel : CourseViewModel
{
    public string ChefName { get; set; } = null!;
    public string ChefBio { get; set; } = null!;
    public bool IsOwner { get; set; }
    public bool CanEnroll { get; set; }
    public string? UserEnrollmentStatus { get; set; }
    public string StatusName { get; set; } = null!;
    public string StatusBadgeClass { get; set; } = null!;

    public List<CourseImageViewModel>? CourseImages { get; set; }

    public string GetStatusBadgeClass()
    {
        return Status switch
        {
            CourseStatus.Inactive => "secondary",
            CourseStatus.Active => "success",
            CourseStatus.Ongoing => "primary",
            CourseStatus.Completed => "info",
            _ => "secondary"
        };
    }

    public string GetStatusName()
    {
        return Status switch
        {
            CourseStatus.Inactive => "Чакащ одобрение",
            CourseStatus.Active => "Активен",
            CourseStatus.Ongoing => "В процес",
            CourseStatus.Completed => "Завършен",
            _ => "Неизвестен"
        };
    }
}