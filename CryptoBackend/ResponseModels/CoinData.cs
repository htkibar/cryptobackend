using System;
using System.Collections.Generic;
using CryptoBackend;

namespace CryptoBackend.ResponseModels
{
    public class CoinData
    {
        public Guid Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        // public Fiat Fiat { get; set; }
        public decimal Volume { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        public decimal LastPrice { get; set; }
    }
}