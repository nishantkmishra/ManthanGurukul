using AutoMapper;
using ManthanGurukul.Domain.Entities;
using ManthanGurukul.Shared.DTOs;

namespace ManthanGurukul.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
