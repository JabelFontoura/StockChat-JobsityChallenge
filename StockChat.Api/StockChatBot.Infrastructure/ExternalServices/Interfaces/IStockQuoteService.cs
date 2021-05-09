using System.Threading.Tasks;

namespace StockChatBot.Infrastructure.ExternalServices.Interfaces
{
    public interface IStockQuoteService
    {
        Task<string> GetStockQuote(string stockId);
    }
}
