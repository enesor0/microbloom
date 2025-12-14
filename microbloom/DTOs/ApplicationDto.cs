namespace microbloom.DTOs
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string? Status { get; set; }
        
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        
        public string AppUserId { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        
        public string UserFullName => $"{UserFirstName} {UserLastName}";
        
        public DateTime ApplicationDate { get; set; }
    }
}
