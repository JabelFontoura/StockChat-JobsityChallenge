using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockChat.Application.Interfaces;
using StockChat.Application.Services;

namespace StockChatBot.Worker.Producer
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
                    services.AddTransient<ITokenService, TokenService>();
                    services.AddHostedService<Worker>();
                });
    }
}
