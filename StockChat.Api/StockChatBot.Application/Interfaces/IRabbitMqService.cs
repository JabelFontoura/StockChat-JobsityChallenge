namespace StockChatBot.Application.Interfaces
{
    public interface IRabbitMqService
    {
        void SendMessage(object payload);
    }
}
