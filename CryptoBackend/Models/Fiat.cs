using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoBackend.Models
{
    public class Fiat
    {
        private Guid id;
        private string name;
        private string symbol;
        private decimal priceUsd;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public decimal PriceUsd { get => priceUsd; set => priceUsd = value; }
    }
}
