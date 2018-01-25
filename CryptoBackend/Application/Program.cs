using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CryptoBackend.Utils;

namespace CryptoBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = Config.Default;
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseIISIntegration()
                .UseUrls("http://127.0.0.1:" + config.Port)
                .Build()
                .Run();
        }
    }
}
