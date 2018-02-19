using System;
using System.Collections.Generic;
using System.Linq;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class CoinData
    {
        private Guid id;
        private Guid coinId;
        private Coin coin;
        private Guid exchangeId;
        private Exchange exchange;
        private DateTime updatedAt;
        private Guid? priceFiatId = null;
        private Fiat priceFiat;
        private Guid? priceCoinId = null;
        private Coin priceCoin;
        private bool priceIsCoin;
        private decimal volume;
        private decimal high;
        private decimal low;
        private decimal ask;
        private decimal bid;
        private decimal lastPrice;

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
        }
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
        public Guid PriceFiatId { set => priceFiatId = value; }
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
        public bool PriceIsCoin { get => priceIsCoin; set => priceIsCoin = value; }
        public decimal Volume { get => volume; set => volume = value; }
        public decimal High { get => high; set => high = value; }
        public decimal Low { get => low; set => low = value; }
        public decimal Ask { get => ask; set => ask = value; }
        public decimal Bid { get => bid; set => bid = value; }
        public decimal LastPrice { get => lastPrice; set => lastPrice = value; }

        public static CoinData Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public static List<CoinData> Find(
            Guid? exchangeId = null,
            Guid? coinId = null
        ) {
            var sql = @"
                select
                data.id as Id,
                data.coin_id as CoinId,
                data.exchange_id as ExchangeId,
                data.updated_at as UpdatedAt,
                data.volume as Volume,
                data.low as Low,
                data.high as High,
                data.ask as Ask,
                data.bid as Bid,
                data.last_price as LastPrice,
                data.price_fiat_id as FiatId
                from coin_data as data
                left outer join coin_data compare_data
                on data.exchange_id = compare_data.exchange_id
                and data.updated_at < compare_data.updated_at
                and data.coin_id = compare_data.coin_id
                where compare_data.exchange_id is null
            ";

            if (exchangeId != null) {
                sql += @" and data.exchange_id = @ExchangeId";
            }

            if (coinId != null) {
                sql += @" and data.coin_id = @CoinId";
            }

            sql += @" order by data.updated_at desc";

            return Database.Master.Many<CoinData>(sql, new {
                ExchangeId = exchangeId,
                CoinId = coinId
            }).ToList();
        }

        public void Save()
        {
            Database.Master.Run<Guid>(@"
                insert into coin_data
                (
                    id,
                    coin_id,
                    exchange_id,
                    updated_at,
                    volume,
                    low,
                    high,
                    ask,
                    bid,
                    last_price,
                    price_fiat_id,
                    price_coin_id,
                    price_is_coin
                )
                values
                (
                    @Id,
                    @CoinId,
                    @ExchangeId,
                    @UpdatedAt,
                    @Volume,
                    @Low,
                    @High,
                    @Ask,
                    @Bid,
                    @LastPrice,
                    @PriceFiatId,
                    @PriceCoinId,
                    @PriceIsCoin
                );
            ", new {
                Id = Guid.NewGuid(),
                CoinId = Coin.Id,
                ExchangeId = Exchange.Id,
                UpdatedAt = DateTime.UtcNow,
                Volume = Volume,
                Low = Low,
                High = High,
                Ask = Ask,
                Bid = Bid,
                LastPrice = LastPrice,
                PriceFiatId = priceFiatId,
                PriceCoinId = priceCoinId,
                PriceIsCoin = PriceIsCoin
            });
        }
    }
}