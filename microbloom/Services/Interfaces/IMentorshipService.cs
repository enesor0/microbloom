using microbloom.DTOs;
using microbloom.Entities;

namespace microbloom.Services.Interfaces
{
    public interface IMentorshipService
    {
        Task<bool> ApplyAsync(string menteeId, string mentorId, string? note);
        Task<List<MentorshipApplicationDto>> GetApplicationsForMentorAsync(string mentorId);
        Task<List<MentorshipApplicationDto>> GetApplicationsForMenteeAsync(string menteeId);
        Task<bool> UpdateStatusAsync(int applicationId, MentorshipStatus status);
        Task<bool> HasPendingApplicationAsync(string menteeId, string mentorId);
    }
}
