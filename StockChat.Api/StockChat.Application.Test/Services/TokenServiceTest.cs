using Microsoft.AspNetCore.Identity;
using StockChat.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;

namespace StockChat.Application.Test.Services
{
    public class TokenServiceTest
    {
        [Fact]
        public void GenerateBotTokenShouldCreateTokenWithRoleBot()
        {
            var token = new TokenService().GenerateBotToken();

            var tokenDecoder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)tokenDecoder.ReadToken(token);

            Assert.Equal("unique_name: ChatBot", jwtSecurityToken.Claims.ToArray()[0].ToString());
            Assert.Equal("role: Bot", jwtSecurityToken.Claims.ToArray()[1].ToString());
        }

        [Fact]
        public void GenerateUSerTokenShouldCreateTokenWithRoleUser()
        {
            var token = new TokenService().GenerateToken(new IdentityUser() { UserName = "Test" }, "User");

            var tokenDecoder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)tokenDecoder.ReadToken(token);

            Assert.Equal("unique_name: Test", jwtSecurityToken.Claims.ToArray()[0].ToString());
            Assert.Equal("role: User", jwtSecurityToken.Claims.ToArray()[1].ToString());
        }
    }
}
