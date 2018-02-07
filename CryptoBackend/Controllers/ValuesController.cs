using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Integrations;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CryptoBackend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var cex = new CexIntegration();
            var bitfinex = new BitfinexIntegration();
            BackgroundJob.Enqueue(() => cex.UpdateCoinDetails());
            BackgroundJob.Enqueue(() => bitfinex.UpdateCoinDetails());
            return "ok";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
