using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class OrderBook
    {
        private Guid id;
        private Guid coinId;
        private Coin coin;
        private Guid exchangeId;
        private Exchange exchange;
        private Guid? priceFiatId = null;
        private Fiat priceFiat;
        private Guid? priceCoinId = null;
        private Coin priceCoin;
        private List<Ask> asks;
        private List<Bid> bids;
        private DateTime updatedAt;

        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
       
        public List<Ask> Asks { get => asks; set => asks = value; } 
        public List<Bid> Bids { get => bids; set => bids = value; } 
        public Guid Id { get => id; set => id = value; }
        public Guid CoinId { set => coinId = value; }
        public Coin Coin
        {
            get
            {
                if (coin == null) {
                    coin = Coin.Find(id: coinId);
                }

                return coin;
            }
            set
            {
                coin = value;
                coinId = coin.Id;
            }
        }
        public Guid ExchangeId { set => exchangeId = value; }
        public Exchange Exchange {
            get
            {
                if (exchange == null) {
                    exchange = Exchange.Find(id: exchangeId);
                }

                return exchange;
            }
            set
            {
                exchange = value;
                exchangeId = exchange.Id;
            }
        }        public Guid PriceFiatId { set => priceFiatId = value; }
        public Fiat PriceFiat {
            get
            {
                if (priceFiat == null && priceFiatId.HasValue) {
                    priceFiat = Fiat.Find(id: (Guid) priceFiatId);
                }
                
                return priceFiat;
            }
            set
            {
                priceFiat = value;
                priceFiatId = priceFiat.Id;
            }
        }
        public Guid PriceCoinId { set => priceCoinId = value; }
        public Coin PriceCoin {
            get
            {
                if (priceCoin == null && priceCoinId.HasValue) {
                    priceCoin = Coin.Find(id: (Guid) priceCoinId);
                }
                
                return priceCoin;
            }
            set
            {
                priceCoin = value;
                priceCoinId = priceCoin.Id;
            }
        }
   
        public void Save(){
            id = Guid.NewGuid();

            Database.Master.Run<int>(@"
                insert into orderbooks
                (
                    id,
                    exchange_id,
                    coin_id,
                    price_is_coin,
                    price_coin_id,
                    price_fiat_id
                ) values (
                    @Id,
                    @ExchangeId,
                    @CoinId,
                    @PriceIsCoin,
                    @PriceCoinId,
                    @PriceFiatId
                );
            ", new {
                Id = id,
                ExchangeId = exchangeId,
                CoinId = coinId,
                PriceIsCoin = priceCoinId != null,
                PriceFiatId = priceFiatId,
                PriceCoinId = priceCoinId
            });

            asks = asks.Take(30).ToList();
            bids = bids.Take(30).ToList();

            var queries = new List<Tuple<string, object>>();

            foreach (Ask ask in asks) {
                ask.OrderbookId = id;
                queries.Add(ask.SaveQuery());
            }

            foreach (Bid bid in bids) {
                bid.OrderbookId = id;
                queries.Add(bid.SaveQuery());
            }

            Database.Master.RunAsync(queries);
        }
  }
}