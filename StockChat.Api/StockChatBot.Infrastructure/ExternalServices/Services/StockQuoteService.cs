using CsvHelper;
using StockChatBot.Domain.Models;
using StockChatBot.Infrastructure.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockChatBot.Infrastructure.ExternalServices.Services
{
    public class StockQuoteService : IStockQuoteService
    {
        private readonly HttpClient _client;

        public StockQuoteService(HttpClient httpClient)
        {
            _client = httpClient;
        }
        public async Task<string> GetStockQuote(string stockId)
        {
            var stockIdUpper = stockId.ToUpper();
            var quoteValue = "";
            var uri = $"?s={stockId}&f=sd2t2ohlcv&h&e=csv";

            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            var result = await _client.SendAsync(message);

            if (result.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await result.Content.ReadAsStreamAsync()))
                {
                    using (var csvReader = new CsvReader(reader))
                    {
                        try
                        {

                            csvReader.Configuration.Delimiter = ",";
                            IEnumerable<StockQuote> stockQuotes = csvReader.GetRecords<StockQuote>();
                            quoteValue = stockQuotes?.FirstOrDefault()?.Open ?? string.Empty;
                        }
                        catch (Exception)
                        {
                            quoteValue = string.Empty;
                        }
                    }
                }
            }
            else
            {
                return $"Error retrieving stock data for {stockIdUpper}.";
            }

            message.Dispose();
            result.Dispose();

            return string.IsNullOrWhiteSpace(quoteValue) ? $"Error retrieving stock data for {stockIdUpper}." : $"{stockIdUpper} quote is ${quoteValue} per share.";
        }
    }
}
