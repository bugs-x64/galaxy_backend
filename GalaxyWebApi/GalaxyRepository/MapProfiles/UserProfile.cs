using AutoMapper;
using GalaxyDto;
using GalaxyRepository.Models;

namespace GalaxyRepository.MapProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserDto, NewUserDto>();
            CreateMap<NewUserDto, UserDto>();
        }
    }
}