using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Newtonsoft.Json;


namespace CryptoBackend.Integrations
{
    class TickerDataBitstamp
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
        public string Pair { get; set; }
    }

    class BitstampIntegration : IExchangeIntegration
    {
        private static readonly string BASE_URL = ApiConsumer.BITSTAMP_BASE_URL;
        List<string> symbolPairs=new List<string>(new string[]
        {
            "btcusd",
            "xrpusd", 
            "ltcusd", 
            "ethusd",
            "bchusd"
        });    
        public Task UpdateCoinDetails()
        {
            List<TickerDataBitstamp> coinDetails=new List<TickerDataBitstamp>();
            foreach(var symbolPair in symbolPairs){
                var requestUrl=BASE_URL+"/ticker/"+symbolPair;
                var response = ApiConsumer.Get<TickerDataBitstamp>(requestUrl).Result; 
                response.Pair=symbolPair;
                coinDetails.Add(response);
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