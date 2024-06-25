namespace MessagingApp.Dtos
{
    public class GroupMemberDto
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }
    }

    public class GroupMemberResultDto
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public UserDto User { get; set; }
    }
}
