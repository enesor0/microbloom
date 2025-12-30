namespace microbloom.DTOs
{
    public class MentorDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Skills { get; set; }
        public string? Bio { get; set; }
    }
}
