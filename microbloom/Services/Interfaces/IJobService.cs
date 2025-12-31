using microbloom.DTOs;

namespace microbloom.Services.Interfaces
{
    public interface IJobService
    {
        Task<List<JobPostingDto>> GetAllJobsAsync();
        Task<JobPostingDetailDto?> GetJobByIdAsync(int jobId);
        Task<List<JobPostingDto>> SearchJobsAsync(string keyword, string location);
        Task ApplyForJobAsync(int jobId, string userId);
        Task<List<ApplicationDto>> GetUserApplicationsAsync(string userId);
    }
}
