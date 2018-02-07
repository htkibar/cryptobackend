using System;
using System.Collections.Generic;
using CryptoBackend;

namespace CryptoBackend.ResponseModels
{
    public class CoinOption
    {
        public Coin Coin { get; set; }
        public Models.Limit DepositLimits { get; set; }
        public Models.Limit WithdrawalLimits { get; set; }
        public Models.Fee DepositFees { get; set; }
        public Models.Fee WithdrawalFees { get; set; }
        public bool Depositable { get; set; }
        public bool Withdrawable { get; set; }
        public string Symbol { get; set; }
    }
}