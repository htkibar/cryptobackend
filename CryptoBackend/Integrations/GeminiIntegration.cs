using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{



    class GeminiIntegration : IExchangeIntegration
    {  
    class TickerData
    {
        [JsonProperty(PropertyName = "volume")]
        public Dictionary<string, object> Volume { get; set; } // TODO: Does it work?
        [JsonProperty(PropertyName = "last")]
        public string Last { get; set; }
        [JsonProperty(PropertyName = "bid")]
        public string Bid { get; set; }

        [JsonProperty(PropertyName = "ask")]
        public string Ask { get; set; }

        [JsonProperty(PropertyName = "pair")]
        public string Pair { get; set; }
 
    }
        private static readonly string BASE_URL = ApiConsumer.GEMINI_BASE_URL;
        
        public void UpdateCoinDetails()
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