using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockChat.Application.Interfaces;

namespace StockChatBot.Worker.Producer
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> _logger;
        private HubConnection _hubConnection;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ITokenService _tokenService;
        private readonly string _botToken;

        public Worker(ILogger<Worker> logger, ITokenService tokenService)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _tokenService = tokenService;
            _botToken = _tokenService.GenerateBotToken();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:5001/chatHub/?token={_botToken}")
                .Build();
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _hubConnection.StartAsync();
            RegisterRabbitMqListener();
            _logger.LogInformation("RabbitMq Listener registered.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }

        private void RegisterRabbitMqListener()
        {
            _channel.QueueDeclare(queue: "stocksQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                SendToHub("ChatBot", message);
            };

            _channel.BasicConsume(queue: "stocksQueue", autoAck: true, consumer: consumer);
        }

        private void SendToHub(string user, string message)
        {
            _hubConnection.InvokeAsync("SendMessage", user, message);
            _logger.LogInformation($"Worker sengind message to hub: {message}");
        }
    }
}
