using microbloom.Data;
using microbloom.DTOs;
using microbloom.Entities;
using microbloom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace microbloom.Services.Implementations
{
    public class MentorshipService : IMentorshipService
    {
        private readonly IDbContextFactory<KariyerDBContext> _factory;

        public MentorshipService(IDbContextFactory<KariyerDBContext> factory)
        {
            _factory = factory;
        }

        public async Task<bool> ApplyAsync(string menteeId, string mentorId, string? note)
        {
            using var context = await _factory.CreateDbContextAsync();
            if (string.IsNullOrEmpty(menteeId) || string.IsNullOrEmpty(mentorId))
                return false;

            // Check if already applied
            var existing = await context.MentorshipApplications
                .FirstOrDefaultAsync(a => a.MenteeId == menteeId && a.MentorId == mentorId && a.Status == MentorshipStatus.Pending);

            if (existing != null)
                return false;

            var application = new MentorshipApplication
            {
                MenteeId = menteeId,
                MentorId = mentorId,
                RequestDate = DateTime.Now,
                Status = MentorshipStatus.Pending,
                Note = note
            };

            context.MentorshipApplications.Add(application);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<MentorshipApplicationDto>> GetApplicationsForMentorAsync(string mentorId)
        {
            using var context = await _factory.CreateDbContextAsync();
            return await context.MentorshipApplications
                .Include(a => a.Mentee)
                .Where(a => a.MentorId == mentorId)
                .Select(a => new MentorshipApplicationDto
                {
                    Id = a.Id,
                    MenteeId = a.MenteeId,
                    MenteeName = a.Mentee != null ? $"{a.Mentee.FirstName} {a.Mentee.LastName}" : "Unknown",
                    MenteePhotoUrl = a.Mentee != null ? a.Mentee.ProfilePictureUrl : null,
                    MentorId = a.MentorId,
                    MentorName = a.Mentor != null ? $"{a.Mentor.FirstName} {a.Mentor.LastName}" : "Unknown",
                    Status = a.Status.ToString(),
                    RequestDate = a.RequestDate,
                    Note = a.Note
                })
                .OrderByDescending(a => a.RequestDate)
                .ToListAsync();
        }

        public async Task<List<MentorshipApplicationDto>> GetApplicationsForMenteeAsync(string menteeId)
        {
            using var context = await _factory.CreateDbContextAsync();
            return await context.MentorshipApplications
                .Include(a => a.Mentor)
                .Where(a => a.MenteeId == menteeId)
                .Select(a => new MentorshipApplicationDto
                {
                    Id = a.Id,
                    MenteeId = a.MenteeId,
                    MenteeName = a.Mentee != null ? $"{a.Mentee.FirstName} {a.Mentee.LastName}" : "Unknown",
                    MentorId = a.MentorId,
                    MentorName = a.Mentor != null ? $"{a.Mentor.FirstName} {a.Mentor.LastName}" : "Unknown",
                    Status = a.Status.ToString(),
                    RequestDate = a.RequestDate,
                    Note = a.Note
                })
                .OrderByDescending(a => a.RequestDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int applicationId, MentorshipStatus status)
        {
            using var context = await _factory.CreateDbContextAsync();
            var application = await context.MentorshipApplications.FindAsync(applicationId);
            if (application == null)
                return false;

            application.Status = status;
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasPendingApplicationAsync(string menteeId, string mentorId)
        {
             using var context = await _factory.CreateDbContextAsync();
             return await context.MentorshipApplications
                .AnyAsync(a => a.MenteeId == menteeId && a.MentorId == mentorId && a.Status == MentorshipStatus.Pending);
        }
    }
}
