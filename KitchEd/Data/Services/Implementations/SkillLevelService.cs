using KitchEd.Data.Services.Interfaces;
using KitchEd.Models.Entities;
using KitchEd.Models.ViewModels.SkillLevel;
using Microsoft.EntityFrameworkCore;


namespace KitchEd.Data.Services.Implementations
{
    public class SkillLevelService : ISkillLevelService
    {
        private readonly ApplicationDbContext _context;

        public SkillLevelService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SkillLevelViewModel>> GetAll()
        {
            return await _context.SkillLevels
                .Select(sl => new SkillLevelViewModel
                {
                    SkillLevelId = sl.SkillLevelId,
                    Name = sl.Name
                })
                .ToListAsync();
        }
        public async Task<SkillLevelViewModel> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Skill Level ID.");

            var skillLevel = await _context.SkillLevels.FindAsync(id);
            if (skillLevel == null)
                throw new KeyNotFoundException("Skill Level not found.");

            return new SkillLevelViewModel
            {
                SkillLevelId = skillLevel.SkillLevelId,
                Name = skillLevel.Name
            };
        }
        public async Task Create(SkillLevelViewModel skillLevelVM)
        {
            if (skillLevelVM == null)
                throw new ArgumentNullException(nameof(skillLevelVM));

            if (string.IsNullOrWhiteSpace(skillLevelVM.Name))
                throw new ArgumentException("Skill Level Name cannot be empty.");

            bool exists = await _context.SkillLevels.AnyAsync(sl => sl.Name == skillLevelVM.Name);
            if (exists)
                throw new InvalidOperationException("Skill Level with the same name already exists.");

            var skillLevel = new SkillLevel { Name = skillLevelVM.Name };

            await _context.SkillLevels.AddAsync(skillLevel);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, SkillLevelViewModel skillLevelVM)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Skill Level ID.");

            if (skillLevelVM == null)
                throw new ArgumentNullException(nameof(skillLevelVM));

            var skillLevel = await _context.SkillLevels.FindAsync(id);
            if (skillLevel == null)
                throw new KeyNotFoundException("Skill Level not found.");

            if (string.IsNullOrWhiteSpace(skillLevelVM.Name))
                throw new ArgumentException("Skill Level Name cannot be empty.");

            bool exists = await _context.SkillLevels
                .AnyAsync(sl => sl.Name == skillLevelVM.Name && sl.SkillLevelId != id);
            if (exists)
                throw new InvalidOperationException("Another Skill Level with the same name already exists.");

            skillLevel.Name = skillLevelVM.Name;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Skill Level ID.");

            var skillLevel = await _context.SkillLevels.FindAsync(id);
            if (skillLevel == null)
                throw new KeyNotFoundException("Skill Level not found.");

            _context.SkillLevels.Remove(skillLevel);
            await _context.SaveChangesAsync();
        }
    }
}
