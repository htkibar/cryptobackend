using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class Coin
    {
        private Guid id = Guid.Empty;
        private string name;
        private string symbol;
        private decimal price;
        private decimal transferTimeMins;
        private Fiat priceCurrency; // Make this fiat OR coin

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public decimal Price { get => price; set => price = value; }
        public decimal TransferTimeMins { get => transferTimeMins; set => transferTimeMins = value; }
        public Fiat PriceCurrency { get => priceCurrency; set => priceCurrency = value; }

        public static IEnumerable<Coin> Find(
            string name = null,
            string symbol = null
        ) {
            // TODO: Retrieve price currency too
            var sql = @"
                select
                coin.id as Id,
                coin.name as Name,
                coin.symbol as Symbol,
                coin.price as Price,
                coin.transfer_time_mins as TransferTimeMins
                from coins as coin
                where 1=1
            ";

            if (name != null) {
                sql += @" and coin.name like concat('%', @Name, '%')";
            }

            if (symbol != null) {
                sql += @" and coin.symbol like @Symbol";
            }

            sql += @" order by coin.id";

            return Database.Master.Many<Coin>(sql, new {
                Name = name,
                Symbol = symbol
            });
        }

        public void Save()
        {
            if (id == Guid.Empty) {
                id = Database.Master.Run<Guid>(@"
                    insert into coins
                    (
                        id,
                        name,
                        symbol,
                        price,
                        transfer_time_mins,
                        price_currency_id
                    )
                    values
                    (
                        @Id,
                        @Name,
                        @Symbol,
                        @Price,
                        @TransferTimeMins,
                        @PriceCurrencyId
                    )
                    returning id;
                ", new {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Symbol = symbol,
                    Price = price,
                    TransferTimeMins = transferTimeMins,
                    PriceCurrencyId = priceCurrency.Id
                });
            } else {
                Database.Master.Run<Guid>(@"
                    update coins set
                    name=@Name,
                    symbol=@Symbol,
                    price=@Price,
                    transfer_time_mins=@TransferTimeMins,
                    price_currency_id=@PriceCurrencyId
                ", new {
                    Name = name,
                    Symbol = symbol,
                    Price = price,
                    TransferTimeMins = transferTimeMins,
                    PriceCurrencyId = priceCurrency.Id
                });
            }
        }
    }
}
