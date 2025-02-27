using System.ComponentModel.DataAnnotations;
using KitchEd.Data.Enums;
using KitchEd.Models.ViewModels.CourseCategory;
using KitchEd.Models.ViewModels.CourseImage;
using KitchEd.Models.ViewModels.DishType;
using KitchEd.Models.ViewModels.SkillLevel;

namespace KitchEd.Models.ViewModels.Course;

public class EditCourseViewModel : CourseViewModel
{
    public bool CanEdit => Status == CourseStatus.Inactive;

    public string? StatusMessage { get; set; }

    [Display(Name = "Допълнителни снимки")]
    public List<string> AdditionalImageUrls { get; set; } = new List<string>();

    public List<CourseImageViewModel> ExistingCourseImages { get; set; } = new List<CourseImageViewModel>();

    public List<CourseCategoryViewModel> Categories { get; set; } = new List<CourseCategoryViewModel>();
    public List<DishTypeViewModel> DishTypes { get; set; } = new List<DishTypeViewModel>();
    public List<SkillLevelViewModel> SkillLevels { get; set; } = new List<SkillLevelViewModel>();
}
