using System.Threading.Tasks;

namespace CryptoBackend.Integrations
{
    class BitfinexIntegration : IExchangeIntegration
    {
        public Task UpdateCoinDetails()
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