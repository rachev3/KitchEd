using System.ComponentModel.DataAnnotations;
using KitchEd.Data.Enums;
using KitchEd.Models.ViewModels.CourseCategory;
using KitchEd.Models.ViewModels.DishType;
using KitchEd.Models.ViewModels.SkillLevel;

namespace KitchEd.Models.ViewModels.Course;

public class EditCourseViewModel : CourseViewModel
{


    public bool CanEdit => Status == CourseStatus.Inactive;

    public string? StatusMessage { get; set; }

    public List<CourseCategoryViewModel> Categories { get; set; } = new List<CourseCategoryViewModel>();
    public List<DishTypeViewModel> DishTypes { get; set; } = new List<DishTypeViewModel>();
    public List<SkillLevelViewModel> SkillLevels { get; set; } = new List<SkillLevelViewModel>();
}
