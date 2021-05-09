using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockChat.Application.Interfaces;
using StockChat.Domain.Constants;

namespace StockChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("Messages")]
        [Authorize(Roles = AuthConstants.UserAuthRole)]
        public async Task<IActionResult> GetLast50Messages()
        {
            return Ok(await _messageService.GetLast50Messages());
        }
    }
}
