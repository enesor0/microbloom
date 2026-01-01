using microbloom.Entities;

namespace microbloom.DTOs
{
    public class MentorshipApplicationDto
    {
        public int Id { get; set; }
        public string MenteeId { get; set; } = string.Empty;
        public string MenteeName { get; set; } = string.Empty;
        public string? MenteePhotoUrl { get; set; }
        public string MentorId { get; set; } = string.Empty;
        public string MentorName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string? Note { get; set; }
    }
}
