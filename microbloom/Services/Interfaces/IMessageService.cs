using microbloom.Entities;
using microbloom.DTOs;

namespace microbloom.Services.Interfaces
{
    public interface IMessageService
    {

        Task<List<Message>> GetConversationAsync(string userId, string contactId);
        Task<List<ContactDto>> GetRecentContactsAsync(string userId);
        Task<Message> SendMessageAsync(string senderId, string receiverId, string content, string? attachmentUrl = null, string? attachmentName = null);
        Task MarkAsReadAsync(int messageId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}
