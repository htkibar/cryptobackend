using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class Country
    {
        private Guid id = Guid.Empty;
        private string name;
        private bool showWarning;
        private bool blockTrades;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public bool ShowWarning { get => showWarning; set => showWarning = value; }
        public bool BlockTrades { get => blockTrades; set => blockTrades = value; }

        public void Save()
        {
            if (id == Guid.Empty) {
                id = Database.Master.Run<Guid>(@"
                    insert into countries
                    (
                        name,
                        show_warning,
                        block_trades
                    )
                    values
                    (
                        @Name,
                        @ShowWarning,
                        @BlockTrades
                    )
                    returning id;
                ", new {
                    Name = Name,
                    ShowWarning = ShowWarning,
                    BlockTrades = BlockTrades
                });
            } else {
                Database.Master.Run<Guid>(@"
                    update countries set
                    name=@Name,
                    show_warning=@ShowWarning,
                    block_trades=@BlockTrades
                ", new {
                    Name = Name,
                    ShowWarning = ShowWarning,
                    BlockTrades = BlockTrades
                });
            }
        }
    }
}
