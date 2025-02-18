using System.ComponentModel.DataAnnotations;

namespace KitchEd.Models.Entities
{
    public class DishType
    {
        [Key]
        public int DishTypeId { get; set; }
        public string Name { get; set; }
    }
}
