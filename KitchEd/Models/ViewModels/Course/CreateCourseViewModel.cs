using System.ComponentModel.DataAnnotations;
using KitchEd.Data.Enums;
using KitchEd.Models.ViewModels.CourseCategory;
using KitchEd.Models.ViewModels.DishType;
using KitchEd.Models.ViewModels.SkillLevel;

namespace KitchEd.Models.ViewModels.Course;

public class CreateCourseViewModel : CourseViewModel
{
    public override CourseStatus Status { get; set; } = CourseStatus.Inactive;
    public override int CurrentParticipants { get; set; } = 0;


    public override DateTime EndDate { get; set; }

    // Display names for form labels
    [Display(Name = "Заглавие")]
    public override string Title { get; set; } = null!;

    [Display(Name = "Описание")]
    public override string Description { get; set; } = null!;

    [Display(Name = "Категория")]
    public override int CategoryId { get; set; }

    [Display(Name = "Тип ястие")]
    public override int DishTypeId { get; set; }

    [Display(Name = "Ниво на умения")]
    public override int SkillLevelId { get; set; }

    [Display(Name = "Начална дата")]
    public override DateTime StartDate { get; set; }

    [Display(Name = "Цена")]
    public override double Price { get; set; }

    [Display(Name = "Максимален брой участници")]
    public override int MaxParticipants { get; set; }

    [Display(Name = "URL на основната снимка")]
    public override string MainImageUrl { get; set; } = null!;

    public List<CourseCategoryViewModel> Categories { get; set; } = new List<CourseCategoryViewModel>();
    public List<DishTypeViewModel> DishTypes { get; set; } = new List<DishTypeViewModel>();
    public List<SkillLevelViewModel> SkillLevels { get; set; } = new List<SkillLevelViewModel>();
}

