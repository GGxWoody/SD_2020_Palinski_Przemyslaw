using System.Linq;
using AutoMapper;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Models;

namespace VolleyballApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.GamesLost,
                    opt => opt.MapFrom(src => src.GamesPlayed - src.GamesWon));
            CreateMap<UserForRegisterDto,User>();
            CreateMap<Team, TeamsForListDto>();
            CreateMap<Team, TeamForDeatailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<TeamForCreationDto,Team>();
            CreateMap<Invite,InviteToReturnDto>();
            CreateMap<Friendlist,FriendToReturnDto>();
            CreateMap<Match,MatchForDetailedDto>();
            CreateMap<Score,ScoreForSendingDto>();
            CreateMap<Match,MatchForListDto>();
            CreateMap<Score,ScoreForListDto>();
            CreateMap<ScoreForAddDto,Score>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<User, UserForTeamListDto>();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photo.Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photo.Url));
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<TeamForUpdateDto, Team>();
            CreateMap<LocationForAddDto, Location>();
        }
    }
}