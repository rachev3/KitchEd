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

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages().AddRazorOptions(options =>
{
    options.ViewLocationFormats.Add("/Views/ClientSide/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/ClientSide/Shared/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/AdminPanel/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/AdminPanel/Shared/{0}.cshtml");
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
builder.Services.AddScoped<ICourseImageServie, CourseImageService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IUserCourseService, UserCourseService>();
builder.Services.AddScoped<ISkillLevelService, SkillLevelService>();
builder.Services.AddScoped<IDishTypeService, DishTypeService>();


var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "admin-panel/{controller=AdminPanel}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
