using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;


namespace CryptoBackend.Integrations
{

    class BitfinexIntegration : IExchangeIntegration
    {
    private class TickerData
    {
        [JsonProperty(PropertyName = "volume")]
        public string Volume { get; set; }
        [JsonProperty(PropertyName = "last_price")]
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
        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }
    }

        private readonly string BASE_URL = ApiConsumer.BITFINEX_BASE_URL;
        private Exchange exchange=null;
        private Fiat fiat = null;

        public BitfinexIntegration()
        {
            var exchanges = Exchange.Find(name: "bitfinex");
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
                "btcusd",
                "ethusd",       
            });   
            // List<TickerData> coinDetails=new List<TickerData>();
            // var requestUri=BASE_URL+"/symbols";
            // var response = ApiConsumer.Get<List<string>>(requestUri).Result; //get symbol pairs which contains ...usd (btcusd,ltcusd)..
            // foreach (var symbolPair in response) {
            //     if(symbolPair.Contains("usd")){
            //         symbolPairs.Add(symbolPair);
            //     }
            // }
            foreach (var symbolPair in symbolPairs) {
                var requestUri=BASE_URL+"/pubticker/"+symbolPair;
                var response=ApiConsumer.Get<TickerData>(requestUri).Result;
                // coinDetails.Add(response);

                var symbol = symbolPair.Split("usd")[0];
                var coins = Coin.Find(symbol: symbol.ToUpper());
                
                if(coins.Count > 0){ 
                    var coin = coins[0];
                    var coinData= new CoinData{
                        Coin=coin,
                        Exchange=exchange,
                        UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(response.Timestamp.Split('.')[0])).DateTime,
                        Fiat = fiat,
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