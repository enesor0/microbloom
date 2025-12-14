using microbloom.DTOs;
using microbloom.Entities;

namespace microbloom.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<JobPostingDto> CreateJobPostingAsync(CreateJobDto jobDto, int companyId);
        Task<List<JobPostingDto>> GetJobsByCompanyAsync(int companyId);
        Task<List<ApplicationDto>> GetApplicationsForJobAsync(int jobId);
        Task UpdateApplicationStatusAsync(int applicationId, string status, int companyId);
        Task DeleteJobPostingAsync(int jobId, int companyId);
        Task ToggleJobStatusAsync(int jobId, bool isActive, int companyId);
        Task<CompanyProfileDto?> GetCompanyProfileAsync(int companyId);
        Task UpdateCompanyProfileAsync(int companyId, CompanyProfileDto profile);
    }
}

