using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;
using Newtonsoft.Json;

namespace CryptoBackend.Integrations
{
    class BinanceIntegration : IExchangeIntegration
    {
        private static readonly string BASE_URL = ApiConsumer.BINANCE_BASE_URL;
        public void UpdateCoinDetails()
        {
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