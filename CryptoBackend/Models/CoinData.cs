using System;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class CoinData
    {
        private Coin coin { get; set; }
        private Exchange exchange { get; set; }
        private DateTime updatedAt { get; set; }
        private Fiat fiat { get; set; }
        private decimal volume { get; set; }
        private decimal high { get; set; }
        private decimal low { get; set; }
        private decimal ask { get; set; }
        private decimal bid { get; set; }
        private decimal lastPrice { get; set; }

        public Coin Coin { get => coin; set => coin = value; }
        public Exchange Exchange { get => exchange; set => exchange = value; }
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
        public Fiat Fiat { get => fiat; set => fiat = value; }
        public decimal Volume { get => volume; set => volume = value; }
        public decimal High { get => high; set => high = value; }
        public decimal Low { get => low; set => low = value; }
        public decimal Ask { get => ask; set => ask = value; }
        public decimal Bid { get => bid; set => bid = value; }
        public decimal LastPrice { get => lastPrice; set => lastPrice = value; }

        public void Save()
        {
            Database.Master.Run<Guid>(@"
                insert into coin_data
                (
                    coin_id,
                    exchange_id,
                    updated_at,
                    volume,
                    low,
                    high,
                    ask,
                    bid,
                    last_price,
                    price_fiat_id
                )
                values
                (
                    @CoinId,
                    @ExchangeId,
                    @UpdatedAt,
                    @Volume,
                    @Low,
                    @High,
                    @Ask,
                    @Bid,
                    @LastPrice,
                    @PriceFiatId
                );
            ", new {
                CoinId = Coin.Id,
                ExchangeId = Exchange.Id,
                UpdatedAt = DateTime.UtcNow,
                Volume = Volume,
                Low = Low,
                High = High,
                Ask = Ask,
                Bid = Bid,
                LastPrice = LastPrice,
                PriceFiatId = Fiat.Id
            });
        }
    }
}