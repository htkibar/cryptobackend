using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoBackend.Models
{
    public class Country
    {
        private Guid id;
        private string name;
        private bool showWarning;
        private bool blockTrades;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public bool ShowWarning { get => showWarning; set => showWarning = value; }
        public bool BlockTrades { get => blockTrades; set => blockTrades = value; }
    }
}
