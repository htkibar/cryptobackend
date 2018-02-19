using System;
using System.Collections.Generic;
using CryptoBackend;

namespace CryptoBackend.ResponseModels
{
    public class Arbitrage
    {
        public string FromExchange;
        public string ToExchange;
        public string Coin;
        public decimal ExpectedProfit;
        public decimal Volume;
        public decimal FromPrice;
        public decimal ToPrice;
        public decimal ToBTCRate;
        public decimal FromBTCRate;
        public string profitCurrency;
        // public Fiat VolumeFiat;
        public DateTime CreatedAt;
    }
}