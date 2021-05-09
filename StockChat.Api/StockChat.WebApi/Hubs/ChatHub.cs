using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using StockChat.Application.Interfaces;
using System.Threading.Tasks;

namespace StockChat.WebApi.Hubs
{
    [Authorize(Roles = "User, Bot")]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public ChatHub(IMessageService messagesService, IUserService userService)
        {
            _messageService = messagesService;
            _userService = userService;
        }

        public async Task SendMessage(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var user = new IdentityUser() { UserName = "ChatBot" };
            if (userId != "ChatBot")
                user = await _userService.GetUserById(userId);

            await Clients.All.SendAsync("ReceiveMessage", user, message);
            await _messageService.SaveMessage(user, message);
        }
    }
}
