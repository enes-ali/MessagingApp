namespace MessagingApp.Models
{
    public class DirectMessage
    {
        public int Id { get; set; }
        
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
