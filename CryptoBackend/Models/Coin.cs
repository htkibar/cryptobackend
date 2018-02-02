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
        private Fiat priceCurrency;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Symbol { get => symbol; set => symbol = value; }
        public decimal Price { get => price; set => price = value; }
        public decimal TransferTimeMins { get => transferTimeMins; set => transferTimeMins = value; }
        public Fiat PriceCurrency { get => priceCurrency; set => priceCurrency = value; }

        public void Save()
        {
            if (id == Guid.Empty) {
                id = Database.Master.Run<Guid>(@"
                    insert into coins
                    (
                        name,
                        symbol,
                        price,
                        transfer_time_mins,
                        price_currency_id
                    )
                    values
                    (
                       @Name,
                       @Symbol,
                       @Price,
                       @TransferTimeMins,
                       @PriceCurrencyId
                    )
                    returning id;
                ", new {
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
