using AutoMapper;
using MessagingApp.Models;
using MessagingApp.Dtos;
using MessagingApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GroupController(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("groups")]
        [Authorize]
        public async Task<IActionResult> GetGroups()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var groups = await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(gm => gm.User)
                .Include(g => g.Messages)
                    .ThenInclude(m => m.Sender)
                .Where(g => g.Members.Any(m => m.UserId == userId) || g.AdminId == userId)
                .ToListAsync();

            return Ok(_mapper.Map<List<GroupResultDto>>(groups));
        }

        [HttpGet("groups/{groupId}")]
        [Authorize]
        public async Task<IActionResult> GetGroupDetail(int groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Admin)
                .Include(g => g.Members)
                    .ThenInclude(gm => gm.User)
                .Include(g => g.Messages)
                    .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GroupResultDto>(group));
        }

        [HttpPost("create-group")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateGroup(CreateGroupDto createGroupDto)
        {
            var group = _mapper.Map<Group>(createGroupDto);
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            group.AdminId = adminId;
            group.Members = new List<GroupMember>();
            group.Messages = new List<GroupMessage>();

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<GroupResultDto>(group));
        }


        [HttpPost("groups/add-member")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> AddGroupMember(GroupMemberDto groupMemberDto)
        {
            var group = await _context.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == groupMemberDto.GroupId);
            if (group == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(groupMemberDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var groupMember = new GroupMember
            {
                GroupId = groupMemberDto.GroupId,
                UserId = groupMemberDto.UserId
            };

            group.Members.Add(groupMember);
            await _context.SaveChangesAsync();

            return Ok(groupMember);
        }

        [HttpDelete("groups/{groupId}/remove-member/{userId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> RemoveGroupMember(int groupId, string userId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            var groupMember = group.Members.FirstOrDefault(gm => gm.UserId == userId);
            if (groupMember == null)
            {
                return NotFound();
            }

            group.Members.Remove(groupMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("send-groupmessage")]
        [Authorize]
        public async Task<IActionResult> CreateGroupMessage(CreateGroupMessageDto createGroupMessageDto)
        {
            var groupMessage = _mapper.Map<GroupMessage>(createGroupMessageDto);
            groupMessage.SentAt = DateTime.UtcNow;

            _context.GroupMessages.Add(groupMessage);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<GroupMessageResultDto>(groupMessage));
        }

        [HttpDelete("groups/delete/{groupId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .Include(g => g.Messages)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
