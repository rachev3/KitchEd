using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.Entities
{
    public class SkillLevel
    {
        [Key]
        public int SkillLevelId { get; set; }
        public string Name { get; set; }
    }
}
