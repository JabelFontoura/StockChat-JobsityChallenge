using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StockChat.Application.Dtos.Response;
using StockChat.Domain.Models;

namespace StockChat.Application.Mapper
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, ResponseMessageDto>();
            CreateMap<IdentityUser, ResponseMessageUserDto>();
        }
    }
}
