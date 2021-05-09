using Microsoft.AspNetCore.Identity;

namespace StockChat.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(IdentityUser identityUser, string roles);
        public string GenerateBotToken();
    }
}
