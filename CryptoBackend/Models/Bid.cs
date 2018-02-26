using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
  public class Bid
  {
    private Guid orderbookId;
    public Guid OrderbookId { set => orderbookId = value; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }

    public Tuple<string, object> SaveQuery()
    {
      return new Tuple<string, object>(@"
        insert into bids
        (
          orderbook_id,
          amount,
          price
        ) values (
          @OrderbookId,
          @Amount,
          @Price
        );
      ", new {
        OrderbookId = orderbookId,
        Amount = Amount,
        Price = Price
      });
    }
  }
}