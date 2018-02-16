using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{

    class BtcTurkIntegration : IExchangeIntegration
    {   
        private class TickerData
        {
            [JsonProperty(PropertyName = "pair")]
            public string Pair { get; set; }
            [JsonProperty(PropertyName = "last")]
            public decimal Last { get; set; }
            [JsonProperty(PropertyName = "timestamp")]
            public decimal Timestamp { get; set; }
            [JsonProperty(PropertyName = "bid")]
            public decimal Bid { get; set; }
            [JsonProperty(PropertyName = "high")]
            public decimal High { get; set; }
            [JsonProperty(PropertyName = "ask")]
            public decimal Ask { get; set; }
            [JsonProperty(PropertyName = "low")]
            public decimal Low { get; set; }
            [JsonProperty(PropertyName = "volume")]
            public decimal Volume { get; set; }
            [JsonProperty(PropertyName = "open")]
            public decimal Open { get; set; }
            [JsonProperty(PropertyName = "average")]
            public decimal Mid { get; set; }
            [JsonProperty(PropertyName = "daily")]
            public decimal Daily { get; set; }
            [JsonProperty(PropertyName = "dailyPercent")]
            public decimal DailyPercent { get; set; }
        }
        private class TickerResponse
        {
            public List<TickerData> Data { get; set; }
        }
        private readonly string BASE_URL = ApiConsumer.BTCTURK_BASE_URL;
        private Exchange exchange = null;
        private Fiat fiat = null;

        public BtcTurkIntegration()
        {
            var exchanges = Exchange.Find(name: "btcturk");

            if (exchanges.Count > 0)
            {
                exchange = exchanges[0];
            }

            var fiats = Fiat.Find(symbol: "TRY");

            if (fiats.Count > 0)
            {
                fiat = fiats[0];
            }
        }
        public void UpdateCoinDetails()
        {
            var requestUri = BASE_URL + "/ticker";
            var response = ApiConsumer.Get<TickerResponse>(requestUri).Result;

            foreach (var data in response.Data) {
                if(!data.Pair.Contains("TRY")){
                    break;
                }

                var symbol = data.Pair.Split("TRY")[0];
                var coins = Coin.Find(symbol: symbol);

                if (coins.Count > 0) {
                    var coin = coins[0];
                    var coinData = new CoinData{
                        Coin = coin,
                        Exchange = exchange,
                        UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(data.Timestamp.ToString())).DateTime,
                        Fiat = fiat,
                        Volume = decimal.Parse(data.Volume.ToString()),
                        High = decimal.Parse(data.High.ToString()),
                        Low = decimal.Parse(data.Low.ToString()),
                        Ask = decimal.Parse(data.Ask.ToString()),
                        Bid = decimal.Parse(data.Bid.ToString()),
                        LastPrice = decimal.Parse(data.Last.ToString())
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