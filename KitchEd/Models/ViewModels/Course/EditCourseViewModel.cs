using System.ComponentModel.DataAnnotations;
using KitchEd.Data.Enums;

namespace KitchEd.Models.ViewModels.Course;

public class EditCourseViewModel : CourseViewModel
{
    [Compare(nameof(StartDate), ErrorMessage = "Крайната дата трябва да е след началната дата")]
    public override DateTime EndDate { get; set; }

    public bool CanEdit => Status == CourseStatus.Inactive;

    public string? StatusMessage { get; set; }
}