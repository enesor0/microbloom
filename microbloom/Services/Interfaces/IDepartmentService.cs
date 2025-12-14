using microbloom.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microbloom.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<List<DepartmentDto>> GetDepartmentsByUniversityIdAsync(int universityId);
        Task<List<DepartmentDto>> SearchDepartmentsAsync(string searchTerm, string? scoreType = null);
    }
}
