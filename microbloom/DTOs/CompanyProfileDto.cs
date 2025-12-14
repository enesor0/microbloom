using System.ComponentModel.DataAnnotations;

namespace microbloom.DTOs
{
    public class CompanyProfileDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Şirket adı 2-200 karakter aralığında olmalıdır.")]
        public string? Name { get; set; }

        [StringLength(2000, ErrorMessage = "Açıklama 2000 karakterden uzun olamaz.")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Geçerli bir logo adresi belirtin.")]
        [StringLength(500, ErrorMessage = "Logo adresi 500 karakterden uzun olamaz.")]
        public string? LogoUrl { get; set; }

        public string? EmployeeCount { get; set; }
        public int ActiveJobCount { get; set; }

        // Yeni alanlar
        public string? Industry { get; set; }
        public int? FoundedYear { get; set; }
        public string? Website { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
    }
}
