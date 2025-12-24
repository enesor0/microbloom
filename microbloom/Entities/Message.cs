using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microbloom.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } = string.Empty;
        
        [ForeignKey("SenderId")]
        public AppUser? Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [ForeignKey("ReceiverId")]
        public AppUser? Receiver { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public string? AttachmentUrl { get; set; }
        public string? AttachmentName { get; set; }
    }
}
