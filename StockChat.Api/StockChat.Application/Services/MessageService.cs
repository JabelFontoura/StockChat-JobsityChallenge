using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockChat.Application.Dtos.Response;
using StockChat.Application.Interfaces;
using StockChat.Domain.Models;
using StockChat.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly StocksChatDbContext _context;
        private readonly IMapper _mapper;

        public MessageService(StocksChatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<ResponseMessageDto> SaveMessage(IdentityUser user, string messageText)
        {
            if (!messageText.StartsWith("/stock"))
            {
                var message = new Message(messageText, user);

                await _context.Messages.AddAsync(message);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return _mapper.Map<ResponseMessageDto>(message);
            }

            return null;
        }

        public async Task<IEnumerable<ResponseMessageDto>> GetLast50Messages()
        {
            var foundMessages = await _context.Messages
                .OrderByDescending(x => x.Date)
                .Take(50)
                .Include(x => x.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ResponseMessageDto>>(foundMessages);
        }
    }
}
