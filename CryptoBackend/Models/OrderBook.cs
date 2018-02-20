using System;
using System.Collections.Generic;
using System.Linq;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class OrderBook
    {
        private Guid id;
        public class Order
        {
            public decimal price;
            public decimal amount;
        }
        private List<Order> asks;
        private List<Order> bids;
        private DateTime updatedAt;

        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
       
        public List<Order> Asks { get => Asks; set => asks = value; } 
        public List<Order> Bids { get => Asks; set => bids = value; } 
  

        public Guid Id { get => id; set => id = value; }
   
        public void Save(){

        }
  }
}