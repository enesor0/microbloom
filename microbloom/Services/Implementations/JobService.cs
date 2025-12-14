using Microsoft.EntityFrameworkCore;
using microbloom.Data;
using microbloom.DTOs;
using microbloom.Services.Interfaces;

namespace microbloom.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly KariyerDBContext _context;

        public JobService(KariyerDBContext context)
        {
            _context = context;
        }

        public async Task<List<JobPostingDto>> GetAllJobsAsync()
        {
            return await _context.JobPostings
                .Include(jp => jp.Company)
                .Where(jp => jp.IsActive)
                .OrderByDescending(jp => jp.PostedDate)
                .Select(jp => new JobPostingDto
                {
                    Id = jp.Id,
                    Title = jp.Title ?? string.Empty,
                    Description = jp.Description ?? string.Empty,
                    Location = jp.Location ?? string.Empty,
                    PostedDate = jp.PostedDate,
                    IsActive = jp.IsActive,
                    CompanyName = jp.Company != null ? jp.Company.Name ?? string.Empty : string.Empty
                })
                .ToListAsync();
        }

        public async Task<JobPostingDetailDto?> GetJobByIdAsync(int jobId)
        {
            var jobPosting = await _context.JobPostings
                .Include(jp => jp.Company)
                .FirstOrDefaultAsync(jp => jp.Id == jobId);

            if (jobPosting == null) return null;

            return new JobPostingDetailDto
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title ?? string.Empty,
                JobDescription = jobPosting.Description ?? string.Empty,
                Location = jobPosting.Location ?? string.Empty,
                PostedDate = jobPosting.PostedDate,
                IsActive = jobPosting.IsActive,
                CompanyId = jobPosting.CompanyId,
                CompanyName = jobPosting.Company != null ? jobPosting.Company.Name ?? string.Empty : string.Empty,
                CompanyDescription = jobPosting.Company != null ? jobPosting.Company.Description ?? string.Empty : string.Empty
            };
        }

        public async Task<List<JobPostingDto>> SearchJobsAsync(string keyword, string location)
        {
            var query = _context.JobPostings
                .Include(jp => jp.Company)
                .Where(jp => jp.IsActive);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(jp =>
                    (jp.Title != null && jp.Title.Contains(keyword)) ||
                    (jp.Description != null && jp.Description.Contains(keyword)));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(jp => jp.Location != null && jp.Location.Contains(location));
            }

            return await query
                .OrderByDescending(jp => jp.PostedDate)
                .Select(jp => new JobPostingDto
                {
                    Id = jp.Id,
                    Title = jp.Title ?? string.Empty,
                    Description = jp.Description ?? string.Empty,
                    Location = jp.Location ?? string.Empty,
                    PostedDate = jp.PostedDate,
                    IsActive = jp.IsActive,
                    CompanyName = jp.Company != null ? jp.Company.Name ?? string.Empty : string.Empty
                })
                .ToListAsync();
        }

        public async Task ApplyForJobAsync(int jobId, string userId)
        {
            var existingApplication = await _context.JobApplications
                .FirstOrDefaultAsync(ja => ja.JobPostingId == jobId && ja.AppUserId == userId);

            if (existingApplication != null)
            {
                throw new InvalidOperationException("Bu ilana zaten başvuru yaptınız.");
            }

            var application = new JobApplication
            {
                JobPostingId = jobId,
                AppUserId = userId,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationDto>> GetUserApplicationsAsync(string userId)
        {
            return await _context.JobApplications
                .Include(ja => ja.JobPosting)
                .ThenInclude(jp => jp.Company)
                .Where(ja => ja.AppUserId == userId)
                .OrderByDescending(ja => ja.ApplicationDate)
                .Select(ja => new ApplicationDto
                {
                    Id = ja.Id,
                    JobPostingId = ja.JobPostingId,
                    JobTitle = ja.JobPosting != null ? ja.JobPosting.Title ?? string.Empty : string.Empty,
                    AppUserId = ja.AppUserId ?? string.Empty,
                    ApplicationDate = ja.ApplicationDate,
                    Status = ja.Status ?? "Pending",
                    CompanyId = ja.JobPosting != null ? ja.JobPosting.CompanyId.ToString() : null,
                    CompanyName = (ja.JobPosting != null && ja.JobPosting.Company != null) ? ja.JobPosting.Company.Name : string.Empty
                })
                .ToListAsync();
        }
    }
}
