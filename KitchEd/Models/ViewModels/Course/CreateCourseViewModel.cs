using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.Course
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Моля, въведете заглавие.")]
        [StringLength(50, ErrorMessage = "Заглавието не може да надвишава 50 символа.")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Моля, въведете описание.")]
        [StringLength(1000, ErrorMessage = "Описанието не може да надвишава 1000 символа.")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Моля, въведете цена.")]
        [Range(0, double.MaxValue, ErrorMessage = "Цената трябва да бъде положително число.")]
        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Моля, въведете максимален брой участници.")]
        [Range(1, int.MaxValue, ErrorMessage = "Броят участници трябва да е положително число.")]
        [Display(Name = "Максимален брой участници")]
        public int MaxParticipants { get; set; }

        [Required(ErrorMessage = "Моля, въведете URL на основната снимка.")]
        [Url(ErrorMessage = "Моля, въведете валиден URL адрес.")]
        [Display(Name = "URL на основната снимка")]
        public string MainImageUrl { get; set; }

        [Required(ErrorMessage = "Моля, въведете начална дата.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Начална дата")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Моля, въведете крайна дата.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Крайна дата")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Моля, изберете категория.")]
        [Display(Name = "Категория")]
        public int CourseCategoryId { get; set; }

        [Required(ErrorMessage = "Моля, изберете тип ястие.")]
        [Display(Name = "Тип ястие")]
        public int DishTypeId { get; set; }

        [Required(ErrorMessage = "Моля, изберете ниво на умения.")]
        [Display(Name = "Ниво на умения")]
        public int SkillLevelId { get; set; }
    }
} 