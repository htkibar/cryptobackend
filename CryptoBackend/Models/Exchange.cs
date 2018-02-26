using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class Exchange
    {
        private Guid id = Guid.Empty;
        private string name;
        private Country country;
        private bool showWarning;
        private bool blockTrades;
        private List<CoinOption> coins = null;
        private List<FiatOption> fiats = null;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Country Country { get => country; set => country = value; }
        public bool ShowWarning { get => showWarning; set => showWarning = value; }
        public bool BlockTrades { get => blockTrades; set => blockTrades = value; }
        public List<CoinOption> Coins {
            get
            {
                if (coins == null) {
                    coins = CoinOption.Find(exchangeId: id);
                }

                return coins;
            }
            set => coins = value;
        }
        public List<FiatOption> Fiats {
            get
            {
                if (fiats == null) {
                    fiats = FiatOption.Find(exchangeId: id);
                }

                return fiats;
            }
            set => fiats = value;
        }

        public static Exchange Find(Guid id)
        {
            var sql = @"
                select
                id as Id,
                name as Name,
                show_warning as ShowWarning,
                block_trades as BlockTrades
                from exchanges as exchange
                where id = @Id
            ";

            return Database.Master.One<Exchange>(sql, new {
                Id = id
            });
        }

        public static List<Exchange> Find(string name = null)
        {
            var sql = @"
                select
                id as Id,
                name as Name,
                show_warning as ShowWarning,
                block_trades as BlockTrades
                from exchanges as exchange
                where 1=1
            ";

            if (name != null) {
                sql += @" and exchange.name like concat('%', @Name, '%')";
            }

            sql += @" order by exchange.id";

            var exchanges = Database.Master.Many<Exchange>(sql, new {
                Name = name
            }).ToList();

            return exchanges;
        }

        public void Save()
        {
            if (id == Guid.Empty) {
                using (var transaction = Database.Master.BeginTransaction()) {
                    try {
                        var tasks = new List<Task>();
                        id = Database.Master.Run<Guid>(@"
                            insert into exchanges
                            (
                                id,
                                name,
                                country_id,
                                show_warning,
                                block_trades
                            )
                            values
                            (
                                @Id,
                                @Name,
                                @CountryId,
                                @ShowWarning,
                                @BlockTrades
                            )
                            returning id;
                        ", new {
                            Id = Guid.NewGuid(),
                            Name = Name,
                            CountryId = Country.Id,
                            ShowWarning = false,
                            BlockTrades = false
                        });

                        foreach (var coin in coins) {
                            coin.Save();
                        }

                        foreach (var fiat in fiats) {
                            fiat.Save();
                        }
                        
                        transaction.Commit();
                    } catch {
                        transaction.Rollback();
                    }
                }
            } else {
                Database.Master.Run<Guid>(@"
                    update exchanges set
                    name=@Name,
                    country_id=@CountryId,
                    show_warning=@ShowWarning,
                    block_trades=@BlockTrades
                ", new {
                    Name = Name,
                    CountryId = Country.Id,
                    ShowWarning = ShowWarning,
                    BlockTrades = BlockTrades
                });
            }
        }
    }
}
