using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using StockChat.Application.Dtos.Request;
using StockChat.Application.Interfaces;
using StockChat.Application.Mapper;
using StockChat.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace StockChat.Application.Test.Services
{
    public class UserServiceTest
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly Mock<UserManager<IdentityUser>> _userManager;

        public UserServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });

            _mapper = config.CreateMapper();
            _tokenService = new TokenService();
            _userManager = MockUserManager(new List<IdentityUser>());
        }

        [Fact]
        public async Task RegisterUserShouldReturnAuthenticatedUser()
        {
            var requestUserDto = new RequestUserDto()
            {
                Email = "teste@mail.com",
                Password = "Password123",
                UserName = "Teste"
            };

            var result = await new UserService(_userManager.Object, _mapper, _tokenService).Register(requestUserDto);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.NotNull(result.AccessToken);
            Assert.Equal("teste@mail.com", result.Email);
            Assert.Equal("Teste", result.UserName);
        }

        [Fact]
        public async Task GetUserByIdShouldReturnUser()
        {
            var result = await new UserService(_userManager.Object, _mapper, _tokenService).GetUserById("1");

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal("teste@mail.com", result.Email);
            Assert.Equal("Teste", result.UserName);
        }

        [Fact]
        public async Task AuthenticateShouldReturnAuthenticatedUser()
        {
            var requestUserDto = new RequestUserDto()
            {
                Email = "teste@mail.com",
                Password = "Password123",
                UserName = "Teste"
            };

            var result = await new UserService(_userManager.Object, _mapper, _tokenService).Register(requestUserDto);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.NotNull(result.AccessToken);
            Assert.Equal("teste@mail.com", result.Email);
            Assert.Equal("Teste", result.UserName);
        }

        private Mock<UserManager<IdentityUser>> MockUserManager(List<IdentityUser> userList) 
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Object.UserValidators.Add(new UserValidator<IdentityUser>());
            userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());

            var passwordHasherMock = new Mock<IPasswordHasher<IdentityUser>>();
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<IdentityUser>(), string.Empty)).Returns("HASHED_PASSWORD");
            userManagerMock.Object.PasswordHasher = passwordHasherMock.Object;

            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success).Callback<IdentityUser>((x) => userList.Add(x));
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.FindByEmailAsync("teste@mail.com")).ReturnsAsync(new IdentityUser() { UserName = "Teste", Email = "teste@mail.com" });
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(new IdentityUser() { UserName = "Teste", Email = "teste@mail.com" });
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), "1")).ReturnsAsync(true);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>())).ReturnsAsync(new List<string>() { "User"});

            return userManagerMock;
        }
    }
}
