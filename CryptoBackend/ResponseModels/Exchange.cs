using System;
using System.Collections.Generic;
using CryptoBackend;

namespace CryptoBackend.ResponseModels
{
    public class Exchange
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Models.Country Country { get; set; }
        public bool ShowWarning { get; set; }
        public bool BlockTrades { get; set; }
        public List<CoinOption> Coins { get; set; }
        // public List<FiatOption> Fiats { get; set; }
    }
}