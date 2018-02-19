using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoBackend.Integrations
{
    class KrakenIntegration : IExchangeIntegration
    {   private class TickerData
        {   [JsonProperty(PropertyName = "a")]
            public List<string> Ask;

            [JsonProperty(PropertyName = "b")]
            public List<string> Bid;

            [JsonProperty(PropertyName = "c")]
            public List<string> Last;
 
            [JsonProperty(PropertyName = "v")]
            public List<string> Volume;

            [JsonProperty(PropertyName = "p")]
            public List<string> VolumeAverage;
 
            [JsonProperty(PropertyName = "t")]
            public List<decimal> NumberOfTrades;

            [JsonProperty(PropertyName = "l")]
            public List<string> Low;

            [JsonProperty(PropertyName = "h")]
            public List<string> High;

            [JsonProperty(PropertyName = "o")]
            public string OpeningPrice;
        }
        private class TickerResult
        {
            [JsonProperty(PropertyName = "result")]
            public Dictionary<string, List<string>> CoinData { get; set; }
        }

        private static readonly string BASE_URL = ApiConsumer.KRAKEN_BASE_URL;
        private Exchange exchange = null;
        private Fiat fiat = null;
               
        public KrakenIntegration()
        {
            var exchanges = Exchange.Find(name: "kraken");
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
            List<string> symbolPairs = new List<string>(new string[]
            {
                "dashusd",
                "xxrpzusd", 
                "xethzusd", 
                "xxbtzusd",
            });  
            foreach (var symbolPair in symbolPairs) {
                var requestUri = BASE_URL + "/public/Ticker?pair=" + symbolPair;
                var response = ApiConsumer.Get<dynamic>(requestUri).Result;

                var symbol = symbolPair.Split("usd")[0];

                if (symbol == "xbtz") {
                    symbol = "btc";
                }

                if (symbol == "xxrpz") {
                    symbol = "xrp";
                }

                if (symbol == "xethz") {
                    symbol = "eth";
                }

                var coins = Coin.Find(symbol: symbol.ToUpper());
                //string value;
                //response.CoinData.TryGetValue(symbolPair.ToUpper(), out value);
                //Console.WriteLine(value);

                if(coins.Count > 0) {
                    var coin = coins[0];
                    var result = response.result[symbolPair.ToUpper()];
                    var volumeArray = result.v;
                    var highArray = result.h;
                    var lowArray = result.l;
                    var askArray = result.a;
                    var bidArray = result.b;
                    var lastPriceArray = result.c;

                    var coinData = new CoinData {
                        Coin=coin,
                        Exchange=exchange,
                        UpdatedAt = DateTime.Now,
                        PriceFiat = fiat,
                        Volume = decimal.Parse(volumeArray[0].ToString()),
                        High = decimal.Parse(highArray[0].ToString()),
                        Low = decimal.Parse(lowArray[0].ToString()),
                        Ask = decimal.Parse(askArray[0].ToString()),
                        Bid = decimal.Parse(bidArray[0].ToString()),
                        LastPrice = decimal.Parse(lastPriceArray[0].ToString())
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