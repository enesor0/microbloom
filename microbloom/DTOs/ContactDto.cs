using microbloom.Entities;

namespace microbloom.DTOs
{
    public class ContactDto
    {
        public string ContactId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageDate { get; set; }
        public bool HasUnread { get; set; }
        public AppUser? User { get; set; }
    }
}
