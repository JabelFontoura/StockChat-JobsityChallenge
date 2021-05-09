using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockChat.Application.Interfaces;
using StockChat.Application.Services;
using StockChatBot.Application.Interfaces;
using StockChatBot.Application.Services;
using StockChatBot.Infrastructure.ExternalServices.Interfaces;
using StockChatBot.Infrastructure.ExternalServices.Services;
using System;

namespace StockChatBot.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddTransient<IStockQuoteService, StockQuoteService>();
                    services.AddTransient<IRabbitMqService, RabbitMqService>();
                    services.AddTransient<ITokenService, TokenService>();
                    services.AddHttpClient<IStockQuoteService, StockQuoteService>(client =>
                     {
                         client.BaseAddress = new Uri("https://stooq.com/q/l/");
                     });
                });
    }
}
