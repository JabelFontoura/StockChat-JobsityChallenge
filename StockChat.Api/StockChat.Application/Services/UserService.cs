using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StockChat.Application.Dtos.Request;
using StockChat.Application.Dtos.Response;
using StockChat.Application.Interfaces;
using StockChat.Domain.Constants;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockChat.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<IdentityUser> userManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ResponseUserDto> Register(RequestUserDto requestUserDto)
        {
            var userIdentity = _mapper.Map<IdentityUser>(requestUserDto);
            userIdentity.PasswordHash = _userManager.PasswordHasher.HashPassword(userIdentity, requestUserDto.Password);
            var createUserResult = await _userManager.CreateAsync(userIdentity);

            if (!createUserResult.Succeeded)
            {
                throw new Exception("Erro: " + createUserResult.Errors);
            }

            await _userManager.AddToRoleAsync(userIdentity, AuthConstants.UserAuthRole);

            return await Authenticate(requestUserDto);

        }

        public async Task<ResponseUserDto> Authenticate(RequestUserDto requestUserDto)
        {
            var identityUser = await _userManager.FindByEmailAsync(requestUserDto.Email);

            if (identityUser == null && !await _userManager.CheckPasswordAsync(identityUser, requestUserDto.Password))
                throw new Exception("Erro: User not found");

            var user = _mapper.Map<ResponseUserDto>(identityUser);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var token = _tokenService.GenerateToken(identityUser, string.Join(", ", roles.ToArray()));
            user.AccessToken = token;

            return user;
        }

        public async Task<IdentityUser> GetUserById(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);

            if (identityUser == null)
                throw new Exception("Erro: User not found");

            return identityUser;
        }
    }
}
