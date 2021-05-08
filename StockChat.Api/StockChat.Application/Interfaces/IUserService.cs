using StockChat.Application.Dtos.Request;
using StockChat.Application.Dtos.Response;
using System.Threading.Tasks;

namespace StockChat.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseUserDto> Register(RequestUserDto requestUserDto);
        Task<ResponseUserDto> Authenticate(RequestUserDto requestUserDto);
    }
}
