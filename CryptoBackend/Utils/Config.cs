using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoBackend.Utils
{
    public class DefaultConfig {
        public string BaseUrl { get; set; }
        public string ConnectionString { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpireSeconds { get; set; }
        public int TokenReplaceSeconds { get; set; }
        public int Port { get; set; }
        public bool DoNotMinifyOutput { get; set; }
        public bool DoNotCacheOutput { get; set; }
    }

    public class Config
    {
        private const string DEFAULT_CONFIG_FILE = "settings.json";
        private static Dictionary<string, object> all = new Dictionary<string, object>();

        public static DefaultConfig Default
        {
            get => Get<DefaultConfig>();
            set => Set(value);
        }

        protected static T Get<T>(string configFile = DEFAULT_CONFIG_FILE)
        {
            if (!all.ContainsKey(configFile))
            {
                all[configFile] = JsonConvert.DeserializeObject<T>(File.ReadAllText(configFile));
            }

            return (T)all[configFile];
        }

        protected static void Set(object config, string configFile = DEFAULT_CONFIG_FILE)
        {
            all[configFile] = config;
            File.WriteAllText(configFile, JsonConvert.SerializeObject(config));
        }
    }
}
