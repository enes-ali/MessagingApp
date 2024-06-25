using MessagingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Dtos
{
    public class CreateGroupDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateGroupDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class GroupResultDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AdminId { get; set; }
        public UserDto? Admin { get; set; }

        public string? Description { get; set; }

        public ICollection<GroupMessageResultDto> Messages { get; set; } = new List<GroupMessageResultDto>();

        public ICollection<UserDto> Members { get; set; } = new List<UserDto>();
    }
}
