using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{
    class CexIntegration : IExchangeIntegration
    {
        private class TickerData
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
        private class TickerResponse
        {
            [JsonProperty(PropertyName = "ok")]
            public string Status { get; set; }
            [JsonProperty(PropertyName = "e")]
            public string Endpoint { get; set; }
            [JsonProperty(PropertyName = "data")]
            public List<TickerData> Data { get; set; }
        }
        private readonly string BASE_URL = ApiConsumer.CEX_BASE_URL;
        private Exchange exchange = null;
        private Fiat fiat = null;

        public CexIntegration()
        {
            var exchanges = Exchange.Find(name: "cex.io");

            if (exchanges.Count > 0)
            {
                exchange = exchanges[0];
            }

            var fiats = Fiat.Find(symbol: "USD");

            if (fiats.Count > 0)
            {
                fiat = fiats[0];
            }
        }

        public void UpdateCoinDetails()
        {
            var requestUri = BASE_URL + "/tickers/USD";
            var response = ApiConsumer.Get<TickerResponse>(requestUri).Result;

            foreach (var data in response.Data) {
                var symbol = data.Pair.Split(':')[0];
                var coins = Coin.Find(symbol: symbol);

                if (coins.Count > 0) {
                    var coin = coins[0];
                    var coinData = new CoinData{
                        Coin = coin,
                        Exchange = exchange,
                        UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(data.Timestamp)).DateTime,
                        Fiat = fiat,
                        Volume = decimal.Parse(data.Volume),
                        High = decimal.Parse(data.High),
                        Low = decimal.Parse(data.Low),
                        Ask = decimal.Parse(data.Ask),
                        Bid = decimal.Parse(data.Bid),
                        LastPrice = decimal.Parse(data.Last)
                    };

                    coinData.Save();
                }
            }
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