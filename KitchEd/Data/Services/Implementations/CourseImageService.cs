using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.CourseImage;
using Microsoft.EntityFrameworkCore;

namespace KitchEd.Data.Services.Implementations
{
    public class CourseImageService : ICourseImageService
    {
        private readonly ApplicationDbContext _context;

        public CourseImageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(CourseImageViewModel viewModel)
        {
            var courseImage = new CourseImage
            {
                ImageUrl = viewModel.ImageUrl,
                CourseId = viewModel.CourseId
            };
            await _context.CourseImages.AddAsync(courseImage);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var courseImage = await _context.CourseImages.FindAsync(id);
            if (courseImage == null)
            {
                throw new Exception("Course image not found");
            }
            _context.CourseImages.Remove(courseImage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseImageViewModel>> GetAllCourseImages(int courseId)
        {
            var courseImages = await _context.CourseImages.Where(c => c.CourseId == courseId).ToListAsync();
            return courseImages.Select(c => new CourseImageViewModel
            {
                Id = c.CourseImageId,
                ImageUrl = c.ImageUrl,
                CourseId = c.CourseId
            });
        }

        public async Task<CourseImageViewModel> GetById(int id)
        {
            var courseImage = await _context.CourseImages.FindAsync(id);
            if (courseImage == null)
            {
                throw new Exception("Course image not found");
            }
            return new CourseImageViewModel
            {
                Id = courseImage.CourseImageId,
                ImageUrl = courseImage.ImageUrl,
                CourseId = courseImage.CourseId
            };


        }

    }
}