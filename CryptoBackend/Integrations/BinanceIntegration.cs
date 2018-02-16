using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{
    class BinanceIntegration : IExchangeIntegration
    {
        private class TickerData
        {
            [JsonProperty(PropertyName = "symbol")]
            public string Symbol { get; set; }
            [JsonProperty(PropertyName = "priceChange")]
            public string PriceChange { get; set; }
            [JsonProperty(PropertyName = "priceChangePercent")]
            public string PriceChangePercent { get; set; }
            [JsonProperty(PropertyName = "weightedAvgPrice")]
            public string WeightedAvgPrice { get; set; }
            [JsonProperty(PropertyName = "prevClosePrice")]
            public string PrevClosePrice { get; set; }
            [JsonProperty(PropertyName = "lastPrice")]
            public string LastPrice { get; set; }
            [JsonProperty(PropertyName = "lastQty")]
            public string LastQty { get; set; }
            [JsonProperty(PropertyName = "bidPrice")]
            public string BidPrice { get; set; }
            [JsonProperty(PropertyName = "bidQty")]
            public string BidQty { get; set; }
            [JsonProperty(PropertyName = "askPrice")]
            public string AskPrice { get; set; }
            [JsonProperty(PropertyName = "askQty")]
            public string AskQty { get; set; }
            [JsonProperty(PropertyName = "openPrice")]
            public string OpenPrice { get; set; }
            [JsonProperty(PropertyName = "highPrice")]
            public string HighPrice { get; set; }
            [JsonProperty(PropertyName = "lowPrice")]
            public string LowPrice { get; set; }
            [JsonProperty(PropertyName = "volume")]
            public string Volume { get; set; }
            [JsonProperty(PropertyName = "quoteVolume")]
            public string QuoteVolume { get; set; }
            [JsonProperty(PropertyName = "openTime")]
            public decimal OpenTime { get; set; }
            [JsonProperty(PropertyName = "closeTime")]
            public decimal CloseTime { get; set; }
            [JsonProperty(PropertyName = "firstId")]
            public decimal FirstId { get; set; }
            [JsonProperty(PropertyName = "lastId")]
            public decimal LastId { get; set; }
            [JsonProperty(PropertyName = "count")]
            public decimal Count { get; set; }

        }
        private static readonly string BASE_URL = ApiConsumer.BINANCE_BASE_URL;
        public void UpdateCoinDetails()
        {
            List<string> symbolPairs=new List<string>(new string[]
            {
                //TODO:
            });  
            foreach (var symbolPair in symbolPairs) {
                var requestUri=BASE_URL + "/ticker/24hr?symbol=" + symbolPair;
                var response=ApiConsumer.Get<TickerData>(requestUri).Result;

                var symbol = symbolPair.Split("usd");
            }
            throw new System.NotImplementedException();
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