using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Dtos
{
    public class CreateDirectMessageDto
    {
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
    }

    public class UpdateDirectMessageDto
    {
        public string Content { get; set; }
    }

    public class DirectMessageResultDto
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public DateTime SentAt { get; set; }
        public UserDto Sender { get; set; }
    }
}
