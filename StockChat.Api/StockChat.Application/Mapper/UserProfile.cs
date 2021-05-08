using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StockChat.Application.Dtos.Request;
using StockChat.Application.Dtos.Response;

namespace StockChat.Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RequestUserDto, IdentityUser>();
            CreateMap<IdentityUser, ResponseUserDto>();
        }
    }
}
