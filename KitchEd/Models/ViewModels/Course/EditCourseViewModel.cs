using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.ViewModels.Course
{
    public class EditCourseViewModel : CreateCourseViewModel
    {
        public int CourseId { get; set; }

        // Add any edit-specific validation or custom logic here
        [CustomValidation(typeof(EditCourseViewModel), nameof(ValidateEndDate))]
        public new DateTime EndDate { get; set; }

        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = (EditCourseViewModel)context.ObjectInstance;
            if (endDate <= instance.StartDate)
            {
                return new ValidationResult("Крайната дата трябва да бъде след началната дата.");
            }
            return ValidationResult.Success;
        }
    }
} 