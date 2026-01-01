using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microbloom.Entities
{
    public class MentorshipApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MenteeId { get; set; } = string.Empty;

        [ForeignKey("MenteeId")]
        public virtual AppUser? Mentee { get; set; }

        [Required]
        public string MentorId { get; set; } = string.Empty;

        [ForeignKey("MentorId")]
        public virtual AppUser? Mentor { get; set; }

        public MentorshipStatus Status { get; set; } = MentorshipStatus.Pending;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public string? Note { get; set; }
    }

    public enum MentorshipStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
