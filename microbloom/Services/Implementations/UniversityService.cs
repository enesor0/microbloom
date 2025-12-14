using Microsoft.EntityFrameworkCore;
using microbloom.Services.Interfaces;
using microbloom.DTOs;

namespace microbloom.Services.Implementations
{
    public class UniversityService : IUniversityService
    {
        private readonly KariyerDBContext _context;

        public UniversityService(KariyerDBContext context)
        {
            _context = context;
        }

        public async Task<List<UniversityDto>> GetAllUniversitiesAsync()
        {
            return await _context.Universities
                .Select(u => new UniversityDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    City = u.City,
                    IsStateUniversity = u.IsStateUniversity,
                    LogoUrl = u.LogoUrl,
                    WebSite = u.WebSite,
                    DepartmentCount = u.Departments != null ? u.Departments.Count : 0
                })
                .ToListAsync();
        }

        public async Task<UniversityDetailDto?> GetUniversityByIdAsync(int id)
        {
            var university = await _context.Universities
                .Include(u => u.Departments)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (university == null) return null;

            return new UniversityDetailDto
            {
                Id = university.Id,
                Name = university.Name,
                City = university.City,
                IsStateUniversity = university.IsStateUniversity,
                LogoUrl = university.LogoUrl,
                WebSite = university.WebSite,
                Departments = university.Departments?
                    .OrderBy(d => d.LastYearBaseRanking) // Sıralama: düşükten yükseğe (en iyi sıralama önce)
                    .Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ScoreType = d.ScoreType,
                        LastYearBaseScore = d.LastYearBaseScore,
                        LastYearBaseRanking = d.LastYearBaseRanking,
                        UniversityName = university.Name
                    }).ToList() ?? new List<DepartmentDto>()
            };
        }

        public async Task<List<UniversityDto>> SearchUniversitiesAsync(string searchTerm, bool? isStateUniversity = null)
        {
            var query = _context.Universities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => u.Name.Contains(searchTerm) || u.City.Contains(searchTerm));
            }

            if (isStateUniversity.HasValue)
            {
                query = query.Where(u => u.IsStateUniversity == isStateUniversity.Value);
            }

            return await query
                .Select(u => new UniversityDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    City = u.City,
                    IsStateUniversity = u.IsStateUniversity,
                    LogoUrl = u.LogoUrl,
                    WebSite = u.WebSite,
                    DepartmentCount = u.Departments != null ? u.Departments.Count : 0
                })
                .ToListAsync();
        }
    }
}