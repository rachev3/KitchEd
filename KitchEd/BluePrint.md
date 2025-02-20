Culinary Course Management Platform - Detailed Assignment

1. Project Overview ⏳
   ✅ ASP.NET MVC (Razor Views), C#
   ❌ Bootstrap implementation
   ❌ Minimal JavaScript
   ⏳ Roles: Admin, Chef, Student (defined but not fully implemented)
   ❌ UI and labels in Bulgarian
2. User Roles & Permissions ⏳
   Admin
   ❌ Approves or rejects newly created courses
   ❌ Can delete users (hard delete)
   ❌ Has full CRUD over all data
   ❌ If a Chef is deleted, Admin automatically becomes the owner of the Chef's courses
   Chef
   ❌ Creates new courses (status Inactive)
   ❌ Submits them for Admin approval
   ❌ Cannot edit once a course is approved (Active)
   ❌ Manages sign-ups: approves or rejects Student enrollments
   Student
   ❌ Views all Active/Ongoing/Completed courses
   ❌ Signs up for courses (status = Pending)
   ❌ Waits for Chef approval (sign-up can become Approved or Rejected)
   Role Assignment
   ⏳ On registration, user picks Admin, Chef, or Student
   ❌ No switching roles later
   Hard Deletion
   ❌ Admin can delete any user, removing them physically from the DB
   ❌ If the user is a Chef, ownership of that Chef's courses transfers to Admin
3. User Data Management ⏳
   Registration & Login
   ✅ Implement via ASP.NET Identity
   ❌ No email verification implementation
   Password Reset
   ❌ Provide a "Forgot Password" or an Admin-managed reset
   Profile Fields
   ✅ FirstName, LastName, ShortBio (no images) - Models defined
4. Course Management ⏳
   Creation & Approval
   ❌ Chef creates a course (Inactive)
   ❌ Admin approves (Active) or rejects
   Lifecycle
   ✅ Models and enums for status tracking defined
   ❌ Active → available for sign-ups implementation
   ❌ Ongoing → once start date arrives implementation
   ❌ Completed → after end date implementation
   ❌ Past courses remain visible implementation
   Sign-Up & Approval by Chef
   ❌ Student signs up → Pending implementation
   ❌ Chef approves or rejects sign-ups implementation
   ❌ MaxParticipants validation implementation
   Images
   ✅ Models for storing image URLs defined
   ❌ Implementation of image management
5. Filtering & Sorting ❌
   Sorting
   ❌ By start date, end date, price, or title
   Multi-Criteria Filtering
   ❌ By category, dish type, skill level, status, etc.
   Implementation
   ❌ Use Bootstrap for layout
   ❌ Minimal JavaScript
   ❌ All text in Bulgarian
6. Database Models ✅
   ✅ Entity Framework implemented
   ✅ All required models created:

- User (extends IdentityUser)
- Course
- UserCourse
- CourseCategory
- CourseImage
- SkillLevel
- DishType
  6.1 User (extends IdentityUser)
  csharp
  Copy
  public class User : IdentityUser
  {
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string ShortBio { get; set; }

        // Navigation
        public ICollection<UserCourse> UserCourses { get; set; }

  }
  6.2 Course
  csharp
  Copy
  public class Course
  {
  public int CourseId { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }

        // Changed from decimal to double
        public double Price { get; set; }

        public int MaxParticipants { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Enum: Inactive, Active, Ongoing, Completed
        public CourseStatus CourseStatus { get; set; }

        // Foreign Keys
        public int CourseCategoryId { get; set; }
        public CourseCategory CourseCategory { get; set; }

        public int DishTypeId { get; set; }
        public DishType DishType { get; set; }

        public int SkillLevelId { get; set; }
        public SkillLevel SkillLevel { get; set; }

        // Navigation
        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<CourseImage> CourseImages { get; set; }

  }
  6.3 UserCourse
  csharp
  Copy
  public class UserCourse
  {
  public int UserCourseId { get; set; }

        // Enum: Pending, Approved, Rejected
        public UserCourseStatus Status { get; set; }

        public DateTime SignUpDate { get; set; }

        // NEW: Track user's role for this course (Chef or Student)
        public string Role { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public User User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

  }
  Note: Typically, a Chef is the creator/owner of the course rather than a "sign-up," but if you need to store the Chef's relationship to their own course in UserCourse, the Role field can reflect that. Alternatively, you can store a separate ChefId in Course. Use whichever approach makes sense for your business logic.

  6.4 CourseCategory
  csharp
  Copy
  public class CourseCategory
  {
  public int CourseCategoryId { get; set; }
  public string Name { get; set; }

      public ICollection<Course> Courses { get; set; }

}
6.5 CourseImage
csharp
Copy
public class CourseImage
{
public int CourseImageId { get; set; }

    // Store URL only
    public string ImageUrl { get; set; }

    // Foreign Key
    public int CourseId { get; set; }
    public Course Course { get; set; }

}
6.6 SkillLevel
csharp
Copy
public class SkillLevel
{
public int SkillLevelId { get; set; }
public string Name { get; set; }

    public ICollection<Course> Courses { get; set; }

}
6.7 DishType
csharp
Copy
public class DishType
{
public int DishTypeId { get; set; }
public string Name { get; set; }

    public ICollection<Course> Courses { get; set; }

} 7. Enums
✅ CourseStatus: Inactive, Active, Ongoing, Completed
✅ UserCourseStatus: Pending, Approved, Rejected
✅ UserRoles (via Identity): Admin, Chef, Student 8. Additional Requirements ❌
Form Validation
❌ Client-side (Bootstrap)
❌ Server-side checks
❌ Validate that Price ≥ 0, StartDate < EndDate, etc.
Dropdown Menus
❌ For Category, Dish Type, Skill Level, etc.
reCAPTCHA
❌ On registration and/or course creation
Minimal JavaScript
❌ Use only if absolutely necessary
All in Bulgarian
❌ Labels, placeholders, validation messages 9. Notifications & Communications ✅
✅ No Email Notifications required
✅ No Internal Messaging required 10. Admin Dashboard ❌
Features
❌ Manage courses (approve/reject)
❌ Manage users (hard delete)
❌ Manage categories, dish types, skill levels
No Analytics/Statistics
No data export or reporting. 11. Sign-Up & Course Ownership Flow ❌
Student Sign-Up
❌ Student signs up → Pending in UserCourse
❌ Chef approves or rejects
❌ MaxParticipants validation
Chef Deletion
❌ Ownership transfer to Admin
❌ Sign-ups preservation
