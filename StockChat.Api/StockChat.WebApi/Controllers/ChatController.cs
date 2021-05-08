using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockChat.Domain.Constants;

namespace StockChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = AuthConstants.UserAuthRole)]
        public async Task<IActionResult> Teste()
        {
            return Ok();
        }
    }
}
