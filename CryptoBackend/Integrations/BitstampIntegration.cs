using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;


namespace CryptoBackend.Integrations
{


    class BitstampIntegration : IExchangeIntegration
    {    
    private class TickerData
    {
        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }

        [JsonProperty(PropertyName = "bid")]
        public string Bid { get; set; }
        [JsonProperty(PropertyName = "ask")]
        public string Ask { get; set; }
        [JsonProperty(PropertyName = "last_price")]
        public string Last { get; set; }        
        [JsonProperty(PropertyName = "low")]
        public string Low { get; set; }
        [JsonProperty(PropertyName = "high")]
        public string High { get; set; }
        [JsonProperty(PropertyName = "volume")]
        public string Volume { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }                
    }
        private static readonly string BASE_URL = ApiConsumer.BITSTAMP_BASE_URL;
        private Exchange exchange = null;
        private Fiat fiat=null;
        List<string> symbolPairs=new List<string>(new string[]
        {
            "btcusd",
            "xrpusd", 
            "ltcusd", 
            "ethusd",
            "bchusd"
        });    

        public BitstampIntegration(){
            var exchanges = Exchange.Find(name: "bitstamp.net");

            if (exchanges.Count > 0)
            {
                exchange = exchanges[0];
            } else 
            {
                throw new System.NotImplementedException();
            }

            var fiats = Fiat.Find(symbol: "USD");

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
            // List<TickerData> coinDetails=new List<TickerData>();
            foreach(var symbolPair in symbolPairs){
                var requestUri=BASE_URL+"/pubticker/"+symbolPair;
                var response = ApiConsumer.Get<TickerData>(requestUri).Result; 
                // coinDetails.Add(response);
                var symbol = symbolPair.Split("usd")[0];
                var coins = Coin.Find(symbol: symbol);
                
                if(coins.Count > 0){ 
                    var coin = coins[0];
                    var coinData= new CoinData{
                        Coin=coin,
                        Exchange=exchange,
                        UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(response.Timestamp)).DateTime,
                        PriceFiat = fiat,
                        Volume = decimal.Parse(response.Volume),
                        High = decimal.Parse(response.High),
                        Low = decimal.Parse(response.Low),
                        Ask = decimal.Parse(response.Ask),
                        Bid = decimal.Parse(response.Bid),
                        LastPrice = decimal.Parse(response.Last)
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