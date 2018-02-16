using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;

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
              public TickerData CoinData { get; set; } 
           // public Dictionary<TickerData, object> CoinData { get; set; } // TODO: Does it work?
         //   public TickerData CoinData { get; set; }
        }

        private static readonly string BASE_URL = ApiConsumer.KRAKEN_BASE_URL;
        private Exchange exchange=null;
        private Fiat fiat = null;
               
        public KrakenIntegration()
        {
            var exchanges = Exchange.Find(name: "Kraken");
            if (exchanges.Count > 0) 
            {
                exchange = exchanges[0];
            } else 
            {
                throw new System.NotImplementedException();
            }
            
            var fiats= Fiat.Find(symbol: "USD");
            
            if (fiats.Count > 0) 
            {
                fiat = fiats[0];
            } else 
            {
                throw new System.NotImplementedException();
            }          
        }
        public void UpdateCoinDetails()
        {
            List<string> symbolPairs=new List<string>(new string[]
            {
                "dashusd",
                "xrpusd", 
                "ethusd", 
                "xbtusd",
            });  
            foreach (var symbolPair in symbolPairs) {
                var reuqestUri= BASE_URL + "/public/Ticker?pair=" +symbolPairs;
                var response = ApiConsumer.Get<TickerResult>(reuqestUri).Result;

                var symbol = symbolPair.Split("usd")[0];

                var coins = Coin.Find(symbol : symbol.ToUpper());

                if(coins.Count > 0) {
                    var coin = coins[0];
                    var coinData = new CoinData {
                        Coin=coin,
                        Exchange=exchange,
                        UpdatedAt = DateTime.Now,
                        Fiat = fiat,
                        Volume = decimal.Parse(response.CoinData.Volume[0]),
                        High = decimal.Parse(response.CoinData.High[0]),
                        Low = decimal.Parse(response.CoinData.Low[0]),
                        Ask = decimal.Parse(response.CoinData.Ask[0]),
                        Bid = decimal.Parse(response.CoinData.Bid[0]),
                        LastPrice = decimal.Parse(response.CoinData.Last[0])
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