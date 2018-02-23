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
        private class OrderData
        {
            [JsonProperty(PropertyName = "price")]
            public string Price { get; set; }
            [JsonProperty(PropertyName = "amount")]
            public string Amount { get; set; }
            [JsonProperty(PropertyName = "timestamp")]
            public string Timestamp { get; set; }
        }
        private class OrderResponse {
            [JsonProperty(PropertyName = "bids")]
            public List<OrderData>  Bids{ get; set; }
            
            [JsonProperty(PropertyName = "asks")]
            public List<OrderData> Asks{ get; set; }
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
                "dshusd",
                "xrpusd",
                "ltcusd"
            });   

            foreach (var symbolPair in symbolPairs) {
                var requestUri=BASE_URL+"/pubticker/"+symbolPair;
                var response=ApiConsumer.Get<TickerData>(requestUri).Result;

                var symbol = symbolPair.Split("usd")[0];

                if (symbol == "dsh") {
                    symbol = "dash";
                }
                var coins = Coin.Find(symbol: symbol.ToUpper());
                
                if(coins.Count > 0){ 
                    var coin = coins[0];
                    var coinData= new CoinData{
                        Coin=coin,
                        Exchange=exchange,
                        UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(response.Timestamp.Split('.')[0])).DateTime,
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

        
        public void UpdateOrderbook()
        {
            List<string> symbolPairs=new List<string>(new string[]
            {
                "btcusd",
                "ethusd",
                "dshusd",
                "xrpusd",
                "ltcusd"
            });   
            foreach (var symbolPair in symbolPairs) {
                var requestUri=BASE_URL+"/book/"+symbolPair;
                var response=ApiConsumer.Get<OrderResponse>(requestUri).Result;        

                var symbol = symbolPair.Split("usd")[0];

                if (symbol == "dsh") {
                    symbol = "dash";
                }
                
                var coins = Coin.Find(symbol: symbol.ToUpper());

                var asks = new List<Ask>();
                var bids = new List<Bid>();

                foreach (var responseAsk in response.Asks) {
                    asks.Add(new Ask{
                        Price = decimal.Parse(responseAsk.Price),
                        Amount = decimal.Parse(responseAsk.Amount)
                    });
                }

                foreach (var responseBid in response.Bids) {
                    bids.Add(new Bid{
                        Price = decimal.Parse(responseBid.Price),
                        Amount = decimal.Parse(responseBid.Amount)
                    });
                }

                if (coins.Count > 0) { 
                    var coin = coins[0];
                    var orderBook = new OrderBook {
                        Coin=coin,
                        Exchange=exchange,
                        UpdatedAt = DateTime.Now,
                        Asks = asks,
                        Bids = bids
                    };
                    orderBook.Save();
                }
            }
        }
    }
}