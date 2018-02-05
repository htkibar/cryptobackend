using System.Threading.Tasks;

namespace CryptoBackend.Integrations
{
    public interface IExchangeIntegration
    {
        Task UpdateOrderbook();
        Task UpdateCoinPrices();
        void UpdateCoinDetails();
    }
}