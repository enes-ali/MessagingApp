namespace MessagingApp.Models
{
    public class GroupMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string SenderId { get; set; }
        public AppUser Sender { get; set; }
    }
}
