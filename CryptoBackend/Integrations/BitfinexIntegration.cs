using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{

    class BitfinexIntegration : IExchangeIntegration
    {
    class TickerData
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

        private static readonly string BASE_URL = ApiConsumer.BITFINEX_BASE_URL;
        public Task UpdateCoinDetails()
        {
            List<string> symbolPairs=new List<string>();
            List<TickerData> coinDetails=new List<TickerData>();
            var requestUri=BASE_URL+"/symbols";
            var response = ApiConsumer.Get<List<string>>(requestUri).Result; //get symbol pairs which contains ...usd (btcusd,ltcusd)..
            foreach(var symbolPair in response){
                if(symbolPair.Contains("usd")){
                    symbolPairs.Add(symbolPair);
                }
            }

            foreach(var symbolpair in symbolPairs){
                requestUri=BASE_URL+"/pubticker/"+symbolpair;
                var tickerData=ApiConsumer.Get<TickerData>(requestUri).Result;
                tickerData.Pair=symbolpair;
                coinDetails.Add(tickerData);

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