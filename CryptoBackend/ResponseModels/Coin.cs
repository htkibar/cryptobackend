using System;
using System.Collections.Generic;
using CryptoBackend;

namespace CryptoBackend.ResponseModels
{
    public class Coin
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal TransferTimeMins { get; set; }
        public List<CoinData> LastData { get; set; }
    }
}