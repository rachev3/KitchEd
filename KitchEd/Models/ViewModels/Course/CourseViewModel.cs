using System.ComponentModel.DataAnnotations;
using KitchEd.Data.Enums;

namespace KitchEd.Models.ViewModels.Course
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Заглавието трябва да е между 5 и 100 символа")]
        [Display(Name = "Заглавие")]
        public virtual string Title { get; set; } = null!;

        [Required(ErrorMessage = "Описанието е задължително")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Описанието трябва да е между 20 и 1000 символа")]
        [Display(Name = "Описание")]
        public virtual string Description { get; set; } = null!;

        [Required(ErrorMessage = "Цената е задължителна")]
        [Range(0, 1000, ErrorMessage = "Цената трябва да е между 0 и 1000 лева")]
        [Display(Name = "Цена")]
        public virtual double Price { get; set; }

        [Required(ErrorMessage = "Максималният брой участници е задължителен")]
        [Range(1, 20, ErrorMessage = "Броят участници трябва да е между 1 и 20")]
        [Display(Name = "Максимален брой участници")]
        public virtual int MaxParticipants { get; set; }

        [Required(ErrorMessage = "Началната дата е задължителна")]
        [DataType(DataType.Date)]
        [Display(Name = "Начална дата")]
        public virtual DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Крайната дата е задължителна")]
        [DataType(DataType.Date)]
        [Display(Name = "Крайна дата")]
        public virtual DateTime EndDate { get; set; }

        public virtual CourseStatus Status { get; set; }

        [Required(ErrorMessage = "URL адресът на снимката е задължителен")]
        [Url(ErrorMessage = "Моля, въведете валиден URL адрес")]
        [Display(Name = "URL на снимката")]
        public virtual string MainImageUrl { get; set; } = null!;


        [Required(ErrorMessage = "Категорията е задължителна")]
        [Display(Name = "Категория")]
        public virtual int CategoryId { get; set; }
        public virtual string? CategoryName { get; set; }

        [Required(ErrorMessage = "Типът ястие е задължителен")]
        [Display(Name = "Тип ястие")]
        public virtual int DishTypeId { get; set; }
        public virtual string? DishTypeName { get; set; }

        [Required(ErrorMessage = "Нивото на трудност е задължително")]
        [Display(Name = "Ниво на трудност")]
        public virtual int SkillLevelId { get; set; }
        public virtual string? SkillLevelName { get; set; }


        public virtual int CurrentParticipants { get; set; }

    }
}