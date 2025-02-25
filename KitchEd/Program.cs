using KitchEd.Data;
using KitchEd.Data.Services.Implementations;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleInitializationService>();
builder.Services.AddScoped<AdminInitializationService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireChefRole", policy => policy.RequireRole("Chef"));
    options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student"));
});

builder.Services.AddRazorPages().AddRazorOptions(options =>
{
    options.ViewLocationFormats.Add("/Views/ClientSide/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/ClientSide/Shared/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/AdminPanel/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/AdminPanel/Shared/{0}.cshtml");
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IUserCourseService, UserCourseService>();
builder.Services.AddScoped<ISkillLevelService, SkillLevelService>();
builder.Services.AddScoped<IDishTypeService, DishTypeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseImageService, CourseImageService>();

var app = builder.Build();

// Initialize Roles and Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleInitializer = services.GetRequiredService<RoleInitializationService>();
        await roleInitializer.InitializeRoles();

        var adminInitializer = services.GetRequiredService<AdminInitializationService>();
        await adminInitializer.InitializeAdmin();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing roles and admin user.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "admin-panel/{controller}/{action}/{id?}",
    defaults: new { action = "Index", controller = "AdminPanel" },
    constraints: new { controller = "AdminPanel|CourseCategory|DishType|SkillLevel" }
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
