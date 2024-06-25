namespace MessagingApp.Dtos
{
    public class CreateGroupMessageDto
    {
        public int GroupId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
    }

    public class UpdateGroupMessageDto
    {
        public string Content { get; set; }
    }

    public class GroupMessageResultDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public DateTime SentAt { get; set; }
        public UserDto Sender { get; set; }
    }
}
