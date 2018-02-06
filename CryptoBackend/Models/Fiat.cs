using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class Fiat
    {
        private Guid id = Guid.Empty;
        private string name;
        private string symbol;
        private decimal priceUsd;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public decimal PriceUsd { get => priceUsd; set => priceUsd = value; }

        public static Fiat Find(Guid id)
        {
            var fiat = Database.Master.One<Fiat>(@"
                select
                id as Id,
                name as Name,
                symbol as Symbol,
                price_usd as PriceUsd
                from fiats as fiat
                where id = @FiatId
            ", new {
                FiatId = id
            });

            if (object.Equals(fiat, default(Fiat))) {
                return null;
            } else {
                return fiat;
            }
        }

        public static List<Fiat> Find(string symbol = null)
        {
            var sql = @"
                select
                id as Id,
                name as Name,
                symbol as Symbol,
                price_usd as PriceUsd
                from fiats as fiat
                where 1=1
            ";

            if (symbol != null) {
                sql += @" and fiat.symbol like concat('%', @Symbol, '%')";
            }

            sql += @" order by fiat.id";

            return Database.Master.Many<Fiat>(sql, new {
                Symbol = symbol
            }).ToList();
        }

        public void Save()
        {
            if (id == Guid.Empty) {
                id = Database.Master.Run<Guid>(@"
                    insert into fiats
                    (
                        id,
                        name,
                        symbol,
                        price_usd
                    )
                    values
                    (
                        @Id,
                        @Name,
                        @Symbol,
                        @PriceUsd
                    )
                    returning id;
                ", new {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Symbol = Symbol,
                    PriceUsd = PriceUsd
                });
            } else {
                Database.Master.Run<Guid>(@"
                    update fiats set
                    name=@Name
                    symbol=@Symbol
                    price_usd=@PriceUsd
                ", new {
                    Name = Name,
                    Symbol = Symbol,
                    PriceUsd = PriceUsd
                });
            }
        }
    }
}
