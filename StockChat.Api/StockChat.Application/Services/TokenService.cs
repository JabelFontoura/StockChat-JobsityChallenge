using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StockChat.Application.Interfaces;
using StockChat.Domain.Constants;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockChat.Application.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(IdentityUser identityUser, string roles)
        {
            return GenerateToken(identityUser, roles, DateTime.UtcNow.AddHours(2));
        }

        public string GenerateBotToken()
        {
            var identityUser = new IdentityUser() { UserName = "ChatBot" };
            return GenerateToken(identityUser, AuthConstants.BotAuthRole, DateTime.UtcNow.AddDays(10));
        }

        private string GenerateToken(IdentityUser identityUser, string roles, DateTime expires)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthConstants.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName),
                    new Claim(ClaimTypes.Role, roles)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
