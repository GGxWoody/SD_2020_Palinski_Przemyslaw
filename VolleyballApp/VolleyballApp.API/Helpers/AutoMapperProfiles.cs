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
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));;
            CreateMap<UserForRegisterDto,User>();
        }
    }
}