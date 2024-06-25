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
    public class ConversationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ConversationController(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("groups-and-conversations")]
        [Authorize]
        public async Task<IActionResult> GetGroupsAndConversations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var groups = await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(gm => gm.User)
                .Include(g => g.Messages)
                    .ThenInclude(m => m.Sender)
                .Where(g => g.Members.Any(m => m.UserId == userId) || g.AdminId == userId)
                .ToListAsync();

            var conversations = await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToListAsync();

            var result = new
            {
                Groups = _mapper.Map<List<GroupResultDto>>(groups),
                Conversations = _mapper.Map<List<ConversationResultDto>>(conversations)
            };

            return Ok(result);
        }

        [HttpGet("conversations")]
        [Authorize]
        public async Task<IActionResult> GetConversations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversations = await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToListAsync();

            return Ok(_mapper.Map<List<ConversationResultDto>>(conversations));
        }

        [HttpGet("conversations/{conversationId}")]
        [Authorize]
        public async Task<IActionResult> GetConversationDetail(int conversationId)
        {
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.Sender)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ConversationResultDto>(conversation));
        }

        [HttpPost("create-conversation")]
        [Authorize]
        public async Task<IActionResult> CreateConversation(CreateConversationDto createConversationDto)
        {
            var conversation = _mapper.Map<Conversation>(createConversationDto);
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ConversationResultDto>(conversation));
        }

        [HttpPost("send-directmessage")]
        [Authorize]
        public async Task<IActionResult> CreateDirectMessage(CreateDirectMessageDto createDirectMessageDto)
        {
            var directMessage = _mapper.Map<DirectMessage>(createDirectMessageDto);
            directMessage.SentAt = DateTime.UtcNow;

            _context.DirectMessages.Add(directMessage);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<DirectMessageResultDto>(directMessage));
        }

        [HttpDelete("delete-conversation/{conversationId}")]
        [Authorize]
        public async Task<IActionResult> DeleteConversation(int conversationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
            {
                return NotFound();
            }

            if (conversation.User1Id != userId && conversation.User2Id != userId)
            {
                return Forbid();
            }

            _context.Conversations.Remove(conversation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
