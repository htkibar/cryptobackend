using System;
using System.Threading.Tasks;
namespace CryptoBackend.Integrations{

    class CDFetcher{
        public async Task Fetch(){
            var cexIntegration=new CexIntegration();
            cexIntegration.UpdateCoinDetails();
            throw new System.NotImplementedException();
        }
    }
}