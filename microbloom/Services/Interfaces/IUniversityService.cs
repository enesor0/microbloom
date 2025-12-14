using microbloom.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microbloom.Services.Interfaces
{
    public interface IUniversityService
    {
        Task<List<UniversityDto>> GetAllUniversitiesAsync();
        Task<UniversityDetailDto?> GetUniversityByIdAsync(int id);
        Task<List<UniversityDto>> SearchUniversitiesAsync(string searchTerm, bool? isStateUniversity = null);
    }
}
