namespace microbloom.DTOs
{
    public class JobPostingDetailDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? JobDescription { get; set; }
        public string? Location { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? ContactUserId { get; set; } // ID of the company owner/contact person
    }
}
