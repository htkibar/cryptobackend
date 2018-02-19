using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
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
            public string Last { get; set; }
            [JsonProperty(PropertyName = "lastQty")]
            public string LastQty { get; set; }
            [JsonProperty(PropertyName = "bidPrice")]
            public string Bid { get; set; }
            [JsonProperty(PropertyName = "bidQty")]
            public string BidQty { get; set; }
            [JsonProperty(PropertyName = "askPrice")]
            public string Ask { get; set; }
            [JsonProperty(PropertyName = "askQty")]
            public string AskQty { get; set; }
            [JsonProperty(PropertyName = "openPrice")]
            public string OpenPrice { get; set; }
            [JsonProperty(PropertyName = "highPrice")]
            public string High { get; set; }
            [JsonProperty(PropertyName = "lowPrice")]
            public string Low { get; set; }
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
        private Exchange exchange = null;
        private Coin coin = null;

        public BinanceIntegration()
        {
            var exchanges = Exchange.Find(name: "binance");

            if (exchanges.Count > 0)
            {
                exchange = exchanges[0];
            }

            var coins = Coin.Find(symbol: "BTC");

            if (coins.Count > 0)
            {
                coin = coins[0];
            }
        }
        public void UpdateCoinDetails()
        {
            List<string> symbolPairs=new List<string>(new string[]
            {
                "ETHBTC",
                "XRPBTC",
                "DASHBTC"
            });  
            foreach (var symbolPair in symbolPairs) {
                var requestUri=BASE_URL + "/ticker/24hr?symbol=" + symbolPair;
                var response=ApiConsumer.Get<TickerData>(requestUri).Result;

                var symbol = response.Symbol.Split("BTC")[0];
                var coins = Coin.Find(symbol: symbol);

                Coin retrievedCoin;

                if (coins.Count > 0) {
                    retrievedCoin = coins[0];
                    
                    var coinData = new CoinData{
                        Coin = retrievedCoin,
                        Exchange = exchange,
                        UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds((long) response.CloseTime).DateTime,
                        PriceCoin = coin,
                        Volume = decimal.Parse(response.Volume),
                        High = decimal.Parse(response.High),
                        Low = decimal.Parse(response.Low),
                        Ask = decimal.Parse(response.Ask),
                        Bid = decimal.Parse(response.Bid),
                        LastPrice = decimal.Parse(response.Last),
                        PriceIsCoin = true
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