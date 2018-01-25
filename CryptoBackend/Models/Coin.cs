using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoBackend.Models
{
    public class Coin
    {
        private Guid id;
        private string name;
        private string symbol;
        private decimal price;
        private decimal transferTimeMins;
        private Coin priceCurrency;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public decimal Price { get => price; set => price = value; }
        public decimal TransferTimeMins { get => transferTimeMins; set => transferTimeMins = value; }
        public Coin PriceCurrency { get => priceCurrency; set => priceCurrency = value; }
    }
}
