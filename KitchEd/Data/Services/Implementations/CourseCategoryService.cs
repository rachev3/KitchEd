using KitchEd.Data;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.CourseCategory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchEd.Data.Services.Implementations
{
    public class CourseCategoryService : ICourseCategoryService
    {
        private readonly ApplicationDbContext _context;

        public CourseCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseCategoryViewModel>> GetAll()
        {
            return await _context.CourseCategories
                .Select(cc => new CourseCategoryViewModel
                {
                    CourseCategoryId = cc.CourseCategoryId,
                    Name = cc.Name
                })
                .ToListAsync();
        }

        public async Task<CourseCategoryViewModel> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Course Category ID.");

            var courseCategory = await _context.CourseCategories.FindAsync(id);
            if (courseCategory == null)
                throw new KeyNotFoundException("Course Category not found.");

            return new CourseCategoryViewModel
            {
                CourseCategoryId = courseCategory.CourseCategoryId,
                Name = courseCategory.Name
            };
        }

        public async Task Create(CourseCategoryViewModel courseCategoryVM)
        {
            if (courseCategoryVM == null)
                throw new ArgumentNullException(nameof(courseCategoryVM));

            if (string.IsNullOrWhiteSpace(courseCategoryVM.Name))
                throw new ArgumentException("Course Category Name cannot be empty.");

            // Prevent duplicate names
            bool exists = await _context.CourseCategories.AnyAsync(cc => cc.Name == courseCategoryVM.Name);
            if (exists)
                throw new InvalidOperationException("A Course Category with the same name already exists.");

            var courseCategory = new CourseCategory { Name = courseCategoryVM.Name };

            await _context.CourseCategories.AddAsync(courseCategory);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, CourseCategoryViewModel courseCategoryVM)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Course Category ID.");

            if (courseCategoryVM == null)
                throw new ArgumentNullException(nameof(courseCategoryVM));

            var courseCategory = await _context.CourseCategories.FindAsync(id);
            if (courseCategory == null)
                throw new KeyNotFoundException("Course Category not found.");

            if (string.IsNullOrWhiteSpace(courseCategoryVM.Name))
                throw new ArgumentException("Course Category Name cannot be empty.");

            bool exists = await _context.CourseCategories
                .AnyAsync(cc => cc.Name == courseCategoryVM.Name && cc.CourseCategoryId != id);
            if (exists)
                throw new InvalidOperationException("Another Course Category with the same name already exists.");

            courseCategory.Name = courseCategoryVM.Name;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Course Category ID.");

            var courseCategory = await _context.CourseCategories.FindAsync(id);
            if (courseCategory == null)
                throw new KeyNotFoundException("Course Category not found.");

            _context.CourseCategories.Remove(courseCategory);
            await _context.SaveChangesAsync();
        }
    }
}
