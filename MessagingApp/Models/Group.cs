namespace MessagingApp.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AdminId { get; set; }
        public AppUser Admin { get; set; }

        public string? Description { get; set; }

        public ICollection<GroupMessage> Messages { get; set; } = new List<GroupMessage>();

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    }
}
