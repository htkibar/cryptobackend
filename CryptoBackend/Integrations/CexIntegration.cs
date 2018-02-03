using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{
    class TickerData
    {
        [JsonProperty(PropertyName = "volume")]
        public string Volume { get; set; }
        [JsonProperty(PropertyName = "last")]
        public string Last { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty(PropertyName = "bid")]
        public string Bid { get; set; }
        [JsonProperty(PropertyName = "high")]
        public string High { get; set; }
        [JsonProperty(PropertyName = "ask")]
        public string Ask { get; set; }
        [JsonProperty(PropertyName = "low")]
        public string Low { get; set; }
        [JsonProperty(PropertyName = "pair")]
        public string Pair { get; set; }
        [JsonProperty(PropertyName = "volume30d")]
        public string MonthlyVolume { get; set; }
    }
    class TickerResponse
    {
        [JsonProperty(PropertyName = "ok")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "e")]
        public string Endpoint { get; set; }
        [JsonProperty(PropertyName = "data")]
        public List<TickerData> Data { get; set; }
        
    }
    class CexIntegration : IExchangeIntegration
    {
        private static readonly string BASE_URL = ApiConsumer.CEX_BASE_URL;
        public Task UpdateCoinDetails()
        {
            var task = new Task(() => {
                var requestUri = BASE_URL + "/tickers/USD";
                var response = ApiConsumer.Get<TickerResponse>(requestUri).Result;

                response.Data.FindAll(data =>
                {
                    var pair = data.Pair.Split(':');
                    var result = false;
                    
                    foreach (var symbol in pair) {
                        if (symbol == "USD") {
                            result = true;
                        }
                    }

                    return result;
                });
            });
        }

        public Task UpdateCoinPrices()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateOrderbook()
        {
            throw new System.NotImplementedException();
        }
    }
}