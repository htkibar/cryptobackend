using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Integrations;
using CryptoBackend.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CryptoBackend.Controllers
{
    [Route("api/[controller]")]
    public class ExchangesController : Controller
    {
        [HttpGet]
        public IEnumerable<Exchange> Get()
        {
            return Exchange.Find();
        }
    }
}
