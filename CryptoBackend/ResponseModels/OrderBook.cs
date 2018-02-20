using System;
using System.Collections.Generic;
using CryptoBackend;
using CryptoBackend.Models;

namespace CryptoBackend.ResponseModels
{
    public class OrderBook
    {
        public Guid Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CryptoBackend.Models.Ask> Asks { get; set; }
        public List<CryptoBackend.Models.Bid> Bids { get; set; }
  }
}