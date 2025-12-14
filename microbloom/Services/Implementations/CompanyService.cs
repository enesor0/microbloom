using Microsoft.EntityFrameworkCore;
using System.Linq;
using microbloom.Data;
using microbloom.DTOs;
using microbloom.Services.Interfaces;

namespace microbloom.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly KariyerDBContext _context;

        public CompanyService(KariyerDBContext context)
        {
            _context = context;
        }

        public async Task<JobPostingDto> CreateJobPostingAsync(CreateJobDto jobDto, int companyId)
        {
            var jobPosting = new JobPosting
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                Location = jobDto.Location,
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                CompanyId = companyId
            };

            _context.JobPostings.Add(jobPosting);
            await _context.SaveChangesAsync();

            var companyName = await _context.Companies
                .Where(c => c.Id == companyId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            return new JobPostingDto
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Location = jobPosting.Location,
                PostedDate = jobPosting.PostedDate,
                IsActive = jobPosting.IsActive,
                CompanyName = companyName
            };
        }

        public async Task<List<JobPostingDto>> GetJobsByCompanyAsync(int companyId)
        {
            return await _context.JobPostings
                .Include(jp => jp.Company)
                .Where(jp => jp.CompanyId == companyId && jp.IsActive)
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

        public async Task<List<ApplicationDto>> GetApplicationsForJobAsync(int jobId)
        {
            return await _context.JobApplications
                .Include(ja => ja.AppUser)
                .Include(ja => ja.JobPosting)
                .Where(ja => ja.JobPostingId == jobId)
                .OrderByDescending(ja => ja.ApplicationDate)
                .Select(ja => new ApplicationDto
                {
                    Id = ja.Id,
                    JobPostingId = ja.JobPostingId,
                    JobTitle = ja.JobPosting != null ? ja.JobPosting.Title ?? string.Empty : string.Empty,
                    AppUserId = ja.AppUserId ?? string.Empty,
                    UserFirstName = ja.AppUser != null ? ja.AppUser.FirstName ?? string.Empty : string.Empty,
                    UserLastName = ja.AppUser != null ? ja.AppUser.LastName ?? string.Empty : string.Empty,
                    UserEmail = ja.AppUser != null ? ja.AppUser.Email ?? string.Empty : string.Empty,
                    ApplicationDate = ja.ApplicationDate,
                    Status = ja.Status ?? "Pending"
                })
                .ToListAsync();
        }

        public async Task UpdateApplicationStatusAsync(int applicationId, string status, int companyId)
        {
            var application = await _context.JobApplications
                .Include(ja => ja.JobPosting)
                .FirstOrDefaultAsync(ja => ja.Id == applicationId);

            if (application == null)
                throw new KeyNotFoundException("Başvuru bulunamadı.");

            if (application.JobPosting?.CompanyId != companyId)
                throw new UnauthorizedAccessException("Bu başvuruyu yönetme yetkiniz yok.");

            application.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobPostingAsync(int jobId, int companyId)
        {
            var jobPosting = await _context.JobPostings
                .FirstOrDefaultAsync(jp => jp.Id == jobId);

            if (jobPosting == null)
                throw new KeyNotFoundException("İlan bulunamadı.");

            if (jobPosting.CompanyId != companyId)
                throw new UnauthorizedAccessException("Bu ilanı silme yetkiniz yok.");

            _context.JobPostings.Remove(jobPosting);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleJobStatusAsync(int jobId, bool isActive, int companyId)
        {
            var jobPosting = await _context.JobPostings
                .FirstOrDefaultAsync(jp => jp.Id == jobId);

            if (jobPosting == null)
                throw new KeyNotFoundException("İlan bulunamadı.");

            if (jobPosting.CompanyId != companyId)
                throw new UnauthorizedAccessException("Bu ilanı düzenleme yetkiniz yok.");

            jobPosting.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        public async Task<CompanyProfileDto?> GetCompanyProfileAsync(int companyId)
        {
            var company = await _context.Companies
                .Include(c => c.Employees)
                .Include(c => c.JobPostings)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                return null;
            }

            return new CompanyProfileDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                LogoUrl = company.LogoUrl,
                Industry = company.Industry,
                EmployeeCount = company.EmployeeCount,
                FoundedYear = company.FoundedYear,
                Website = company.Website,
                ContactEmail = company.ContactEmail,
                Phone = company.Phone,
                Location = company.Location,
                LinkedInUrl = company.LinkedInUrl,
                TwitterUrl = company.TwitterUrl,
                InstagramUrl = company.InstagramUrl,
                ActiveJobCount = company.JobPostings?.Count(j => j.IsActive) ?? 0
            };
        }

        public async Task UpdateCompanyProfileAsync(int companyId, CompanyProfileDto profile)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                throw new KeyNotFoundException("Şirket bulunamadı.");
            }

            company.Name = profile.Name?.Trim();
            company.Description = string.IsNullOrWhiteSpace(profile.Description) ? null : profile.Description.Trim();
            company.LogoUrl = string.IsNullOrWhiteSpace(profile.LogoUrl) ? null : profile.LogoUrl.Trim();
            company.Industry = profile.Industry?.Trim();
            company.EmployeeCount = profile.EmployeeCount?.Trim();
            company.FoundedYear = profile.FoundedYear;
            company.Website = string.IsNullOrWhiteSpace(profile.Website) ? null : profile.Website.Trim();
            company.ContactEmail = string.IsNullOrWhiteSpace(profile.ContactEmail) ? null : profile.ContactEmail.Trim();
            company.Phone = string.IsNullOrWhiteSpace(profile.Phone) ? null : profile.Phone.Trim();
            company.Location = string.IsNullOrWhiteSpace(profile.Location) ? null : profile.Location.Trim();
            company.LinkedInUrl = string.IsNullOrWhiteSpace(profile.LinkedInUrl) ? null : profile.LinkedInUrl.Trim();
            company.TwitterUrl = string.IsNullOrWhiteSpace(profile.TwitterUrl) ? null : profile.TwitterUrl.Trim();
            company.InstagramUrl = string.IsNullOrWhiteSpace(profile.InstagramUrl) ? null : profile.InstagramUrl.Trim();

            await _context.SaveChangesAsync();
        }
    }
}
