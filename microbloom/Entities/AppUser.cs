using Microsoft.AspNetCore.Identity;

namespace microbloom.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? CvUrl { get; set; }
        
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Skills { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<JobApplication>? Applications { get; set; } 
    }   

    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        
        public string? Industry { get; set; }
        public string? EmployeeCount { get; set; }
        public int? FoundedYear { get; set; }
        public string? Website { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        
        public ICollection<AppUser>? Employees { get; set; }
        public ICollection<JobPosting>? JobPostings { get; set; } 
    }

    public class JobPosting
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }

    public class JobApplication
    {
        public int Id { get; set; } 
        public int JobPostingId { get; set; }
        public JobPosting? JobPosting { get; set; }  
        public string? AppUserId { get; set; }   
        public AppUser? AppUser { get; set; }    
        public DateTime ApplicationDate { get; set; }   
        public string? Status { get; set; }  
    }
}