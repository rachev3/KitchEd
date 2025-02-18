using KitchEd.Data;
using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.DishType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchEd.Data.Services.Implementations
{
    public class DishTypeService : IDishTypeService
    {
        private readonly ApplicationDbContext _context;

        public DishTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DishTypeViewModel>> GetAll()
        {
            return await _context.DishTypes
                .Select(dt => new DishTypeViewModel
                {
                    DishTypeId = dt.DishTypeId,
                    Name = dt.Name
                })
                .ToListAsync();
        }

        public async Task<DishTypeViewModel> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Dish Type ID.");

            var dishType = await _context.DishTypes.FindAsync(id);
            if (dishType == null)
                throw new KeyNotFoundException("Dish Type not found.");

            return new DishTypeViewModel
            {
                DishTypeId = dishType.DishTypeId,
                Name = dishType.Name
            };
        }

        public async Task Create(DishTypeViewModel dishTypeVM)
        {
            if (dishTypeVM == null)
                throw new ArgumentNullException(nameof(dishTypeVM));

            if (string.IsNullOrWhiteSpace(dishTypeVM.Name))
                throw new ArgumentException("Dish Type Name cannot be empty.");

            bool exists = await _context.DishTypes.AnyAsync(dt => dt.Name == dishTypeVM.Name);
            if (exists)
                throw new InvalidOperationException("Dish Type with the same name already exists.");

            var dishType = new DishType { Name = dishTypeVM.Name };

            await _context.DishTypes.AddAsync(dishType);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, DishTypeViewModel dishTypeVM)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Dish Type ID.");

            if (dishTypeVM == null)
                throw new ArgumentNullException(nameof(dishTypeVM));

            var dishType = await _context.DishTypes.FindAsync(id);
            if (dishType == null)
                throw new KeyNotFoundException("Dish Type not found.");

            if (string.IsNullOrWhiteSpace(dishTypeVM.Name))
                throw new ArgumentException("Dish Type Name cannot be empty.");

            bool exists = await _context.DishTypes
                .AnyAsync(dt => dt.Name == dishTypeVM.Name && dt.DishTypeId != id);
            if (exists)
                throw new InvalidOperationException("Another Dish Type with the same name already exists.");

            dishType.Name = dishTypeVM.Name;
            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Dish Type ID.");

            var dishType = await _context.DishTypes.FindAsync(id);
            if (dishType == null)
                throw new KeyNotFoundException("Dish Type not found.");

            _context.DishTypes.Remove(dishType);
            await _context.SaveChangesAsync();
        }
    }
}
