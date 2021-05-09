using Microsoft.Extensions.DependencyInjection;
using StockChat.Application.Interfaces;
using StockChat.Application.Services;

namespace StockChat.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IMessageService, MessageService>();

            return services;
        }
    }
}
