using Microsoft.AspNetCore.Identity;
using StockChat.Application.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockChat.Application.Interfaces
{
    public interface IMessageService
    {
        Task<ResponseMessageDto> SaveMessage(IdentityUser user, string message);
        Task<IEnumerable<ResponseMessageDto>> GetLast50Messages();
    }
}
