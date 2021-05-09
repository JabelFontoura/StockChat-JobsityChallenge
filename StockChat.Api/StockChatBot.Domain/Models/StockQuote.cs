using System;

namespace StockChatBot.Domain.Models
{
    public class StockQuote
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public int Volume { get; set; }
    }
}
