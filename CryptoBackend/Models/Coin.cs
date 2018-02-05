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
        private decimal transferTimeMins;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public decimal TransferTimeMins { get => transferTimeMins; set => transferTimeMins = value; }

        public static List<Coin> Find(
            string name = null,
            string symbol = null
        ) {
            var sql = @"
                select
                coin.id as Id,
                coin.name as Name,
                coin.symbol as Symbol,
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
            }).ToList();
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
                        transfer_time_mins
                    )
                    values
                    (
                        @Id,
                        @Name,
                        @Symbol,
                        @TransferTimeMins
                    )
                    returning id;
                ", new {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Symbol = symbol,
                    TransferTimeMins = transferTimeMins
                });
            } else {
                Database.Master.Run<Guid>(@"
                    update coins set
                    name=@Name,
                    symbol=@Symbol,
                    transfer_time_mins=@TransferTimeMins
                ", new {
                    Name = name,
                    Symbol = symbol,
                    TransferTimeMins = transferTimeMins
                });
            }
        }
    }
}
