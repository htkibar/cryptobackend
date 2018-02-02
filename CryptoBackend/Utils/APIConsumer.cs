using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoBackend.Utils
{
    class ApiConsumer
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static readonly string CEX_BASE_URL = "";
        public static readonly string BITFINEX_BASE_URL = "";
        public static readonly string BINANCE_BASE_URL = "";
        public static readonly string BITSTAMP_BASE_URL = "";
        public static readonly string BTCTURK_BASE_URL = "";
        public static readonly string COINBASE_BASE_URL = "";
        public static readonly string GEMINI_BASE_URL = "";
        public static readonly string KRAKEN_BASE_URL = "";

        public static Task<T> Get<T>(string requestUri) {
            var task = new Task<T>(() => {
                var response = httpClient.GetAsync(requestUri).Result;

                // Throw an error if not successful
                response.EnsureSuccessStatusCode();

                var content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(content);
            });

            return task;
        }
        public static Task<T> Post<T>() {
            throw new NotImplementedException();
        }
    }
}