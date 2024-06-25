using Microsoft.AspNetCore.Identity;

namespace MessagingApp.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    }
}
