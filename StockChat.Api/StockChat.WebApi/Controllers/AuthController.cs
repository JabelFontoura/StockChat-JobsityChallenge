using Microsoft.AspNetCore.Mvc;
using StockChat.Application.Dtos.Request;
using StockChat.Application.Interfaces;
using System.Threading.Tasks;

namespace StockChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RequestUserDto requestUserDto)
        {
            return Ok(await _userService.Register(requestUserDto));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] RequestUserDto requestUserDto)
        {
            return Ok(await _userService.Authenticate(requestUserDto));
        }

    }
}
