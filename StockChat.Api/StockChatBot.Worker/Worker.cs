using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockChat.Application.Interfaces;
using StockChatBot.Application.Interfaces;
using StockChatBot.Infrastructure.ExternalServices.Interfaces;

namespace StockChatBot.Worker
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ITokenService _tokenService;
        private HubConnection _hubConnection;
        private readonly string _botToken;

        public Worker(ILogger<Worker> logger, IStockQuoteService stockQuoteService, IRabbitMqService rabbitMqService, ITokenService tokenService)
        {
            _logger = logger;
            _stockQuoteService = stockQuoteService;
            _rabbitMqService = rabbitMqService;
            _tokenService = tokenService;
            _botToken = _tokenService.GenerateBotToken();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartHubConnection();
            _logger.LogInformation("Hub Connection initiated with SignalR.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }

        private async void StartHubConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5000/chatHub/?token={_botToken}")
                .Build();

            _hubConnection.On<IdentityUser, string>("ReceiveMessage", (user, message) =>
            {
                ProcessMessage(user, message);
            });

            _hubConnection.Closed += async (error) =>
            {
                await Task.Delay(10000);
                await _hubConnection.StartAsync();
            };

            await _hubConnection.StartAsync();
        }

        private async void ProcessMessage(IdentityUser user, string message)
        {
            if (!message.StartsWith("/stock"))
                return;

            string command = message.Split('=')[1];

            _logger.LogInformation($"Worker received command: {message}");

            var result = await _stockQuoteService.GetStockQuote(command);

            _rabbitMqService.SendMessage(result);
            _logger.LogInformation($"Worker sent result: {result} to RabbitMQ.");
        }
    }
}
