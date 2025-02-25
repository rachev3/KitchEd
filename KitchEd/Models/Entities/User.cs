using Microsoft.AspNetCore.Identity;

namespace KitchEd.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShortBio { get; set; }
        public ICollection<UserCourse>? UserCourses { get; set; }
    }
}
