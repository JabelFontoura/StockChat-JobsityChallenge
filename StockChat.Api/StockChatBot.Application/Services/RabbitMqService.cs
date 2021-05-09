using Newtonsoft.Json;
using RabbitMQ.Client;
using StockChatBot.Application.Interfaces;
using System.Text;

namespace StockChatBot.Application.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        public void SendMessage(object payload)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "stocksQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message = (string) payload;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "stocksQueue",
                        basicProperties: null,
                        body: body);
                }

            }
        }
    }
}
