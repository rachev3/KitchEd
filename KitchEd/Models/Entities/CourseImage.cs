using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchEd.Models.Entities
{
    public class CourseImage
    {
        [Key]
        public int CourseImageId { get; set; }
        public string ImageUrl { get; set; }


        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course? Course { get; set; }
    }
}
