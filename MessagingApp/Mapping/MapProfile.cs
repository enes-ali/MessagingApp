namespace MessagingApp.Mapping
{
    using AutoMapper;
    using MessagingApp.Dtos;
    using MessagingApp.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserDto>();

            CreateMap<CreateConversationDto, Conversation>();
            CreateMap<UpdateConversationDto, Conversation>();
            CreateMap<Conversation, ConversationResultDto>()
                .ForMember(dest => dest.User1, opt => opt.MapFrom(src => src.User1))
                .ForMember(dest => dest.User2, opt => opt.MapFrom(src => src.User2))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));

            CreateMap<CreateDirectMessageDto, DirectMessage>();
            CreateMap<UpdateDirectMessageDto, DirectMessage>();
            CreateMap<DirectMessage, DirectMessageResultDto>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender));

            CreateMap<CreateGroupDto, Group>();
            CreateMap<UpdateGroupDto, Group>();
            CreateMap<Group, GroupResultDto>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members.Select(gm => gm.User)))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));

            CreateMap<CreateGroupMessageDto, GroupMessage>();
            CreateMap<UpdateGroupMessageDto, GroupMessage>();
            CreateMap<GroupMessage, GroupMessageResultDto>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender));

            CreateMap<GroupMember, GroupMemberDto>();
            CreateMap<GroupMemberDto, GroupMember>();
        }
    }
}
