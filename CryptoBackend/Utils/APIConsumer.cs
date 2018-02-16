using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoBackend.Utils
{
    class ApiConsumer
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static readonly string CEX_BASE_URL = "https://cex.io/api";
        public static readonly string BITFINEX_BASE_URL = "https://api.bitfinex.com/v1"; // control v2 again
        public static readonly string BINANCE_BASE_URL = "https://api.binance.com/api/v1";
        public static readonly string BITSTAMP_BASE_URL = "https://www.bitstamp.net/api/v2";
        public static readonly string BTCTURK_BASE_URL = "https://www.btcturk.com/api";
        public static readonly string COINBASE_BASE_URL = "https://api.coinbase.com/v2";
        public static readonly string GEMINI_BASE_URL = "https://api.gemini.com/v1";
        public static readonly string KRAKEN_BASE_URL = "https://api.kraken.com/0";
        public static readonly string DOVIZ_BASE_URL ="http://www.doviz.com/api/v1/currencies";

        public async static Task<T> Get<T>(string requestUri) {
            return await Task.Run<T>(() => {
                var response = httpClient.GetAsync(requestUri).Result;

                // Throw an error if not successful
                response.EnsureSuccessStatusCode();

                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(content);
            });
        }
        public static Task<T> Post<T>() {
            throw new NotImplementedException();
        }
    }
}