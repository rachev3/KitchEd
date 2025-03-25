# KitchEd - Online Cooking Education Platform

KitchEd is a web application built with ASP.NET Core that offers a platform for cooking education where chefs can create courses and students can enroll in them.

## 🍳 Features

- **User Authentication**: Secure registration and login with role-based access control
- **Role-Based System**: Supports Admin, Chef, and Student roles
- **Course Management**: Chefs can create, edit, and manage cooking courses
- **Course Categorization**: Courses are organized by category, skill level, and dish type
- **Course Enrollment**: Students can browse and enroll in cooking courses
- **User Profile Management**: Users can manage their personal profiles
- **Admin Panel**: Comprehensive administration interface for platform management

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Database**: SQL Server
- **Frontend**: Razor Pages, HTML, CSS, JavaScript
- **Styling**: Bootstrap (likely)
- **Security**: reCAPTCHA integration for form protection

## 🏗️ Architecture

The project follows a clean architecture approach:

- **Controllers**: Handle HTTP requests and responses
- **Models**: Define the data structure
  - **Entities**: Course, CourseCategory, DishType, SkillLevel, User, UserCourse, CourseImage
  - **ViewModels**: Used for data transfer between views and controllers
- **Views**: UI components using Razor syntax
- **Services**: Business logic implementation
- **Data**: Database context and data access logic

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio or Visual Studio Code (recommended)

### Installation

1. Clone the repository:

```
git clone https://github.com/yourusername/KitchEd.git
```

2. Navigate to the project directory:

```
cd KitchEd
```

3. Update the connection string in `appsettings.json` to point to your SQL Server instance.

4. Apply database migrations:

```
dotnet ef database update
```

5. Run the application:

```
dotnet run
```

6. Access the application at `https://localhost:5001` or `http://localhost:5000`

## 👥 User Roles

- **Admin**: Full access to the platform, can manage all aspects of the system
- **Chef**: Can create and manage cooking courses
- **Student**: Can browse and enroll in courses

## 🔒 Security Features

- Password policy enforcement
- Role-based authorization
- reCAPTCHA protection for forms

## 📁 Project Structure

- `/Areas`: Contains area-specific components
- `/Controllers`: MVC controllers that handle HTTP requests
- `/Data`: Database context and data-related services
- `/Models`: Data models and view models
- `/Views`: UI templates with Razor syntax
- `/wwwroot`: Static files (CSS, JS, images)

## 📝 License

This project is licensed under the terms of the LICENSE.txt file included in the repository.

---

_Developed with ❤️ for cooking enthusiasts and educational purposes._
