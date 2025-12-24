using microbloom.Data;
using microbloom.Entities;
using microbloom.Services.Interfaces;
using microbloom.DTOs;
using Microsoft.EntityFrameworkCore;

namespace microbloom.Services
{
    public class MessageService : IMessageService
    {
        private readonly KariyerDBContext _context;

        public MessageService(KariyerDBContext context)
        {
            _context = context;
        }

        public async Task<Message> SendMessageAsync(string senderId, string receiverId, string content, string? attachmentUrl = null, string? attachmentName = null)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                AttachmentUrl = attachmentUrl,
                AttachmentName = attachmentName
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetConversationAsync(string userId1, string userId2)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<ContactDto>> GetRecentContactsAsync(string userId)
        {
            // Get latest message for each conversation
            var latestMessages = await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                .Select(g => new { 
                    UserId = g.Key, 
                    LastMessage = g.OrderByDescending(x => x.SentAt).FirstOrDefault()
                })
                .ToListAsync();

            var contactIds = latestMessages.Select(x => x.UserId).ToList();
            var users = await _context.Users.Where(u => contactIds.Contains(u.Id)).ToListAsync();

            var contactDtos = new List<ContactDto>();

            foreach (var item in latestMessages)
            {
                var user = users.FirstOrDefault(u => u.Id == item.UserId);
                if (user != null && item.LastMessage != null)
                {
                    contactDtos.Add(new ContactDto
                    {
                        ContactId = user.Id,
                        UserName = user.UserName ?? "Unknown",
                        User = user,
                        LastMessage = item.LastMessage.Content,
                        LastMessageDate = item.LastMessage.SentAt,
                        HasUnread = item.LastMessage.ReceiverId == userId && !item.LastMessage.IsRead // Check specifically if the LAST message is unread
                    });
                }
            }
            
            // Note: HasUnread above is simplistic (only checks updated msg). 
            // Better to check distinct unread count per user if needed, but for "bolding" the list item, checking if *any* unread exists or just the last is often sufficient. 
            // Let's refine HasUnread to check ANY unread message from that sender.

            var unreadSenders = await _context.Messages
                .Where(m => m.ReceiverId == userId && !m.IsRead)
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();

            foreach(var dto in contactDtos)
            {
                if (unreadSenders.Contains(dto.ContactId))
                {
                    dto.HasUnread = true;
                }
            }

            return contactDtos.OrderByDescending(c => c.LastMessageDate).ToList();
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Messages
                .CountAsync(m => m.ReceiverId == userId && !m.IsRead);
        }
    }
}
