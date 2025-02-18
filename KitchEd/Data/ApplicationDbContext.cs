using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KitchEd.Models.Entities;

namespace KitchEd.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseImage> CourseImages { get; set; }
        public DbSet<DishType> DishTypes { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
    }
}
