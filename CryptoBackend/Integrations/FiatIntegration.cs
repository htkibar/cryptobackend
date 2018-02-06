using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{
    class FiatIntegration 
    {    
        class FiatData
        {
            [JsonProperty(PropertyName = "selling")]
            public decimal Selling { get; set; }
            [JsonProperty(PropertyName = "update_date")]
            public decimal UpdateDate { get; set; }
            [JsonProperty(PropertyName = "currency")]
            public Guid CurrencyID { get; set; }
            [JsonProperty(PropertyName = "buying")]
            public decimal Buying { get; set; }
            [JsonProperty(PropertyName = "change_rate")]
            public decimal ChangeRate { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }  
            [JsonProperty(PropertyName = "full_name")]
            public string FullName { get; set; }  
            [JsonProperty(PropertyName = "code")]
            public string Symbol { get; set; }  
        }
        private static readonly string BASE_URL = ApiConsumer.DOVIZ_BASE_URL;
        private Fiat fiat=null;
  
        public FiatIntegration(){
            var fiats = Fiat.Find(symbol: "TRY");
            if(fiats.Count > 0) {
                fiat = fiats[0];
            } else {
                throw new System.NotImplementedException();
            }

        }
        public void UpdateFiatDetails()
        {   var requestUri = BASE_URL+"/USD/latest";
            var responce = ApiConsumer.Get<FiatData>(requestUri).Result;
            
            if(fiat!=null){
                var fiatData= new Fiat{
                    Name = fiat.Name,
                    Symbol = fiat.Symbol,
                    PriceUsd =responce.Selling
                };
                fiatData.Save();
            }
        }
    }
}