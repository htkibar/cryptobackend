using System.Threading.Tasks;

namespace CryptoBackend.Integrations
{
    public interface IExchangeIntegration
    {
        void UpdateOrderbook();
        Task UpdateCoinPrices();
        void UpdateCoinDetails();
    }
}