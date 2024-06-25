using MessagingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Dtos
{
    public class CreateConversationDto
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
    }

    public class UpdateConversationDto
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
    }

    public class ConversationResultDto
    {
        public int Id { get; set; }

        public string User1Id { get; set; }
        public UserDto User1 { get; set; }

        public string User2Id { get; set; }
        public UserDto User2 { get; set; }

        public ICollection<DirectMessageResultDto> Messages { get; set; } = new List<DirectMessageResultDto>();
    }
}
