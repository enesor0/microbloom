using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace microbloom.DTOs
{
    public class RegisterDto : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır.")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountType { get; set; } = "JobSeeker";

        [StringLength(200)]
        public string? CompanyName { get; set; }

        [StringLength(2000)]
        public string? CompanyDescription { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "Geçerli bir logo adresi giriniz.")]
        public string? CompanyLogoUrl { get; set; }

        // Mentor Fields
        public string? Title { get; set; }
        public string? Workplace { get; set; }
        public string? Skills { get; set; } // Comma separated
        public string? Bio { get; set; }
        public string? LinkedInUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var isCompanyAccount = string.Equals(AccountType, "Employer", StringComparison.OrdinalIgnoreCase);

            if (isCompanyAccount && string.IsNullOrWhiteSpace(CompanyName))
            {
                yield return new ValidationResult(
                    "Şirket kaydı için şirket adı gereklidir.",
                    new[] { nameof(CompanyName) });
            }

            var isMentorAccount = string.Equals(AccountType, "Mentor", StringComparison.OrdinalIgnoreCase);
            if (isMentorAccount && string.IsNullOrWhiteSpace(Title))
            {
                yield return new ValidationResult(
                    "Mentor kaydı için ünvan gereklidir.",
                    new[] { nameof(Title) });
            }
        }
    }
}



