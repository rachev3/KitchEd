using KitchEd.Data.Enums;
using KitchEd.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KitchEd.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAllAsync()
        {
            await SeedRolesAsync();

            var adminId = await SeedAdminAsync();
            var chefIds = await SeedChefsAsync();
            var studentIds = await SeedStudentsAsync();

            await SeedCourseCategoriesAsync();
            await SeedSkillLevelsAsync();
            await SeedDishTypesAsync();

            var courseIds = await SeedCoursesAsync(chefIds);

            await SeedCourseImagesAsync(courseIds);

            await SeedUserCoursesAsync(chefIds, studentIds, courseIds);

            await _context.SaveChangesAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (await _context.Roles.AnyAsync())
                return;

            foreach (UserRoles role in Enum.GetValues(typeof(UserRoles)))
            {
                string roleName = role.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task<string> SeedAdminAsync()
        {
            var existingAdmin = await _userManager.FindByEmailAsync("admin@kitched.com");
            if (existingAdmin != null)
                return existingAdmin.Id;

            var admin = new User
            {
                UserName = "admin",
                Email = "admin@kitched.com",
                FirstName = "Admin",
                LastName = "Adminov",
                ShortBio = "System Administrator",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin, "Admin123?");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, UserRoles.Admin.ToString());
                return admin.Id;
            }

            throw new Exception("Failed to seed admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        private async Task<List<string>> SeedChefsAsync()
        {
            var chefIds = new List<string>();

            if (await _userManager.Users.AnyAsync(u => _userManager.IsInRoleAsync(u, UserRoles.Chef.ToString()).Result))
                return (await _userManager.GetUsersInRoleAsync(UserRoles.Chef.ToString())).Select(u => u.Id).ToList();

            var chefs = new List<User>
            {
                new User
                {
                    UserName = "chef1",
                    Email = "chef1@kitched.com",
                    FirstName = "Ivan",
                    LastName = "Petrov",
                    ShortBio = "Experienced chef with a passion for traditional Bulgarian cuisine.",
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "chef2",
                    Email = "chef2@kitched.com",
                    FirstName = "Maria",
                    LastName = "Ivanova",
                    ShortBio = "Pastry expert with experience in French bakery techniques.",
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "chef3",
                    Email = "chef3@kitched.com",
                    FirstName = "Georgi",
                    LastName = "Dimitrov",
                    ShortBio = "Italian cuisine specialist with focus on Mediterranean dishes.",
                    EmailConfirmed = true
                }
            };

            foreach (var chef in chefs)
            {
                var result = await _userManager.CreateAsync(chef, "Chef123?");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(chef, UserRoles.Chef.ToString());
                    chefIds.Add(chef.Id);
                }
            }

            return chefIds;
        }

        private async Task<List<string>> SeedStudentsAsync()
        {
            var studentIds = new List<string>();

            if (await _userManager.Users.AnyAsync(u => _userManager.IsInRoleAsync(u, UserRoles.Student.ToString()).Result))
                return (await _userManager.GetUsersInRoleAsync(UserRoles.Student.ToString())).Select(u => u.Id).ToList();

            var students = new List<User>
            {
                new User
                {
                    UserName = "student1",
                    Email = "student1@example.com",
                    FirstName = "Elena",
                    LastName = "Petrova",
                    ShortBio = "Cooking enthusiast looking to improve skills.",
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "student2",
                    Email = "student2@example.com",
                    FirstName = "Dimitar",
                    LastName = "Georgiev",
                    ShortBio = "Amateur home cook with interest in international cuisine.",
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "student3",
                    Email = "student3@example.com",
                    FirstName = "Nikolay",
                    LastName = "Todorov",
                    ShortBio = "Beginner in cooking looking for basic skills.",
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "student4",
                    Email = "student4@example.com",
                    FirstName = "Viktoria",
                    LastName = "Ivanova",
                    ShortBio = "Health food enthusiast interested in nutritious cooking.",
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = "student5",
                    Email = "student5@example.com",
                    FirstName = "Petar",
                    LastName = "Angelov",
                    ShortBio = "Looking to expand my dessert-making repertoire.",
                    EmailConfirmed = true
                }
            };

            foreach (var student in students)
            {
                var result = await _userManager.CreateAsync(student, "Student123?");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student, UserRoles.Student.ToString());
                    studentIds.Add(student.Id);
                }
            }

            return studentIds;
        }

        private async Task SeedCourseCategoriesAsync()
        {
            if (await _context.CourseCategories.AnyAsync())
                return;

            var categories = new List<CourseCategory>
            {
                new CourseCategory { Name = "Българска кухня" },
                new CourseCategory { Name = "Италианска кухня" },
                new CourseCategory { Name = "Френска кухня" },
                new CourseCategory { Name = "Азиатска кухня" },
                new CourseCategory { Name = "Сладкарство" },
                new CourseCategory { Name = "Веган кухня" },
                new CourseCategory { Name = "Здравословно готвене" }
            };

            await _context.CourseCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        private async Task SeedSkillLevelsAsync()
        {
            if (await _context.SkillLevels.AnyAsync())
                return;

            var skillLevels = new List<SkillLevel>
            {
                new SkillLevel { Name = "Начинаещ" },
                new SkillLevel { Name = "Среден" },
                new SkillLevel { Name = "Напреднал" },
                new SkillLevel { Name = "Професионален" }
            };

            await _context.SkillLevels.AddRangeAsync(skillLevels);
            await _context.SaveChangesAsync();
        }

        private async Task SeedDishTypesAsync()
        {
            if (await _context.DishTypes.AnyAsync())
                return;

            var dishTypes = new List<DishType>
            {
                new DishType { Name = "Предястия" },
                new DishType { Name = "Основни ястия" },
                new DishType { Name = "Супи" },
                new DishType { Name = "Салати" },
                new DishType { Name = "Десерти" },
                new DishType { Name = "Хлебни изделия" },
                new DishType { Name = "Паста" },
                new DishType { Name = "Морски дарове" }
            };

            await _context.DishTypes.AddRangeAsync(dishTypes);
            await _context.SaveChangesAsync();
        }

        private async Task<List<int>> SeedCoursesAsync(List<string> chefIds)
        {
            if (await _context.Courses.AnyAsync())
                return await _context.Courses.Select(c => c.CourseId).ToListAsync();

            var courseCategories = await _context.CourseCategories.ToListAsync();
            var skillLevels = await _context.SkillLevels.ToListAsync();
            var dishTypes = await _context.DishTypes.ToListAsync();

            var random = new Random();

            var courses = new List<Course>
            {
                new Course {
                    Title = "Основи на българската кухня",
                    Description = "Запознайте се с основните техники и рецепти на традиционната българска кухня. Ще научите как да приготвяте основни ястия като мусака, сарми, баница и шопска салата.",
                    Price = 150.00,
                    MaxParticipants = 12,
                    MainImageUrl = "/images/courses/bulgarian-cuisine.jpg",
                    StartDate = DateTime.Now.AddDays(10),
                    EndDate = DateTime.Now.AddDays(17),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Българска кухня").CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Начинаещ").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Основни ястия").DishTypeId
                },
                new Course {
                    Title = "Италианска паста и сосове",
                    Description = "Научете се да приготвяте автентична италианска паста и различни видове сосове. Курсът включва приготвяне на домашна паста, карбонара, болонезе, песто и други класически сосове.",
                    Price = 180.00,
                    MaxParticipants = 10,
                    MainImageUrl = "/images/courses/italian-pasta.jpg",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(12),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Италианска кухня").CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Среден").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Паста").DishTypeId
                },
                new Course {
                    Title = "Френски десерти",
                    Description = "Потопете се в света на изискваните френски десерти. Ще се научите да правите макарони, еклери, крем брюле, тарт татен и други класически сладкиши.",
                    Price = 200.00,
                    MaxParticipants = 8,
                    MainImageUrl = "/images/courses/french-desserts.jpg",
                    StartDate = DateTime.Now.AddDays(15),
                    EndDate = DateTime.Now.AddDays(22),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Френска кухня").CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Напреднал").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Десерти").DishTypeId
                },
                new Course {
                    Title = "Азиатска кухня за начинаещи",
                    Description = "Въведение в основните техники и съставки на азиатската кухня. Ще научите как да приготвяте стир-фрай, суши, пад тай и други популярни ястия.",
                    Price = 170.00,
                    MaxParticipants = 10,
                    MainImageUrl = "/images/courses/asian-cuisine.jpg",
                    StartDate = DateTime.Now.AddDays(20),
                    EndDate = DateTime.Now.AddDays(27),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Азиатска кухня").CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Начинаещ").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Основни ястия").DishTypeId
                },
                new Course {
                    Title = "Домашен хляб и тестени изделия",
                    Description = "Научете се да правите различни видове хляб, питки, фокача и други тестени изделия. Курсът включва работа с квас, различни видове брашна и техники за месене и печене.",
                    Price = 140.00,
                    MaxParticipants = 12,
                    MainImageUrl = "/images/courses/bread-baking.jpg",
                    StartDate = DateTime.Now.AddDays(8),
                    EndDate = DateTime.Now.AddDays(15),
                    CourseStatus = CourseStatus.Inactive,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Хлебни изделия" || c.Name.Contains("Сладкарство")).CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Среден").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Хлебни изделия").DishTypeId
                },
                new Course {
                    Title = "Веган готвене",
                    Description = "Открийте разнообразието на растителната кухня. Курсът включва приготвяне на хранителни и вкусни ястия без използване на животински продукти.",
                    Price = 160.00,
                    MaxParticipants = 10,
                    MainImageUrl = "/images/courses/vegan-cooking.jpg",
                    StartDate = DateTime.Now.AddDays(25),
                    EndDate = DateTime.Now.AddDays(32),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Веган кухня").CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Начинаещ").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Основни ястия").DishTypeId
                },
                new Course {
                    Title = "Здравословно готвене",
                    Description = "Научете как да приготвяте балансирани и питателни ястия без да жертвате вкуса. Фокус върху нискокалорични техники и здравословни съставки.",
                    Price = 170.00,
                    MaxParticipants = 12,
                    MainImageUrl = "/images/courses/healthy-cooking.jpg",
                    StartDate = DateTime.Now.AddDays(30),
                    EndDate = DateTime.Now.AddDays(37),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Здравословно готвене").CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Среден").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Основни ястия").DishTypeId
                },
                new Course {
                    Title = "Морска кухня",
                    Description = "Специализиран курс за приготвяне на ястия с риба и морски дарове. Научете как да избирате, почиствате и готвите различни видове морски продукти.",
                    Price = 220.00,
                    MaxParticipants = 8,
                    MainImageUrl = "/images/courses/seafood.jpg",
                    StartDate = DateTime.Now.AddDays(18),
                    EndDate = DateTime.Now.AddDays(25),
                    CourseStatus = CourseStatus.Active,
                    CourseCategoryId = courseCategories.First(c => c.Name == "Френска кухня" || c.Name.Contains("Италианска")).CourseCategoryId,
                    SkillLevelId = skillLevels.First(s => s.Name == "Напреднал").SkillLevelId,
                    DishTypeId = dishTypes.First(d => d.Name == "Морски дарове").DishTypeId
                }
            };

            for (int i = 0; i < courses.Count; i++)
            {
                var chefIndex = i % chefIds.Count;
                var chefId = chefIds[chefIndex];

            }

            await _context.Courses.AddRangeAsync(courses);
            await _context.SaveChangesAsync();

            return courses.Select(c => c.CourseId).ToList();
        }

        private async Task SeedCourseImagesAsync(List<int> courseIds)
        {
            if (await _context.CourseImages.AnyAsync())
                return;

            var courseImages = new List<CourseImage>();

            foreach (var courseId in courseIds)
            {
                for (int i = 1; i <= new Random().Next(2, 4); i++)
                {
                    courseImages.Add(new CourseImage
                    {
                        CourseId = courseId,
                        ImageUrl = $"/images/courses/course-{courseId}-image-{i}.jpg"
                    });
                }
            }

            await _context.CourseImages.AddRangeAsync(courseImages);
            await _context.SaveChangesAsync();
        }

        private async Task SeedUserCoursesAsync(List<string> chefIds, List<string> studentIds, List<int> courseIds)
        {
            if (await _context.UserCourses.AnyAsync())
                return;

            var userCourses = new List<UserCourse>();
            var random = new Random();

            for (int i = 0; i < courseIds.Count; i++)
            {
                var courseId = courseIds[i];
                var chefId = chefIds[i % chefIds.Count]; 

                userCourses.Add(new UserCourse
                {
                    UserId = chefId,
                    CourseId = courseId,
                    Role = UserRoles.Chef.ToString(),
                    SignUpDate = DateTime.Now.AddDays(-random.Next(10, 30)),
                    Status = UserCourseStatus.Approved
                });
            }

            foreach (var studentId in studentIds)
            {
             
                var courseCount = random.Next(2, 5);
                var enrollCourseIds = courseIds.OrderBy(x => random.Next()).Take(courseCount).ToList();

                foreach (var courseId in enrollCourseIds)
                {
                    var statusValue = random.Next(1, 4); 
                    var status = (UserCourseStatus)statusValue;

                    userCourses.Add(new UserCourse
                    {
                        UserId = studentId,
                        CourseId = courseId,
                        Role = UserRoles.Student.ToString(),
                        SignUpDate = DateTime.Now.AddDays(-random.Next(1, 15)),
                        Status = status
                    });
                }
            }

            await _context.UserCourses.AddRangeAsync(userCourses);
            await _context.SaveChangesAsync();
        }
    }
}