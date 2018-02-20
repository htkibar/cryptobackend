using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Models;
using CryptoBackend.Utils;
using Newtonsoft.Json;
namespace CryptoBackend.Integrations
{
    class CoinbaseIntegration : IExchangeIntegration
    {   
        private readonly string BASE_URL = ApiConsumer.COINBASE_BASE_URL;
        private Exchange exchange = null;
        private Fiat fiat = null;

        public CoinbaseIntegration()
        {
            var exchanges = Exchange.Find(name: "Coinbase");

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
            throw new System.NotImplementedException();
        }

        public Task UpdateCoinPrices()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateOrderbook()
        {
            throw new System.NotImplementedException();
        }
    }
}