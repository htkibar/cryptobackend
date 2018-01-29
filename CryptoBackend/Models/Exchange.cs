using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class Limit
    {
        private decimal minLimit;
        private decimal maxLimit;

        public decimal Min { get => minLimit; set => minLimit = value; }
        public decimal Max { get => maxLimit; set => maxLimit = value; }
    }

    public class Fee
    {
        private decimal percentageFee;
        private decimal fixedFee;

        public decimal Percentage { get => percentageFee; set => percentageFee = value; }
        public decimal Fixed { get => fixedFee; set => fixedFee = value; }
    }

    public class ExchangeCoin
    {
        private int minimumConfirmations;
        private Limit depositLimits;
        private Limit withdrawalLimits;
        private Fee depositFees;
        private Fee withdrawalFees;
        private bool depositable;
        private bool withdrawable;

        public int MinimumConfirmations { get => minimumConfirmations; set => minimumConfirmations = value; }
        public Limit DepositLimits { get => depositLimits; set => depositLimits = value; }
        public Limit WithdrawalLimits { get => withdrawalLimits; set => withdrawalLimits = value; }
        public Fee DepositFees { get => depositFees; set => depositFees = value; }
        public Fee WithdrawalFees { get => withdrawalFees; set => withdrawalFees = value; }
        public bool Depositable { get => depositable; set => depositable = value; }
        public bool Withdrawable { get => withdrawable; set => withdrawable = value; }
    }

    public class ExchangeFiat
    {
        private Limit depositLimits;
        private Limit withdrawalLimits;
        private Fee depositFees;
        private Fee withdrawalFees;
        private bool depositable;
        private bool withdrawable;
        
        public Limit DepositLimits { get => depositLimits; set => depositLimits = value; }
        public Limit WithdrawalLimits { get => withdrawalLimits; set => withdrawalLimits = value; }
        public Fee DepositFees { get => depositFees; set => depositFees = value; }
        public Fee WithdrawalFees { get => withdrawalFees; set => withdrawalFees = value; }
        public bool Depositable { get => depositable; set => depositable = value; }
        public bool Withdrawable { get => withdrawable; set => withdrawable = value; }
    }

    public class Exchange
    {
        private Guid id = Guid.Empty;
        private string name;
        private Country country;
        private bool showWarning;
        private bool blockTrades;
        private IEnumerable<ExchangeCoin> coins;
        private IEnumerable<ExchangeFiat> fiats;

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Country Country { get => country; set => country = value; }
        public bool ShowWarning { get => showWarning; set => showWarning = value; }
        public bool BlockTrades { get => blockTrades; set => blockTrades = value; }

        public void Save() {
            if (id == Guid.Empty) {
                id = Database.Master.Run<Guid>(@"
                    insert into exchanges
                    (
                        name,
                        country_id,
                        show_warning,
                        block_trades
                    )
                    values
                    (
                        @Name,
                        @CountryId,
                        @ShowWarning,
                        @BlockTrades
                    )
                    returning id;
                ", new {
                    Name = Name,
                    CountryId = Country.Id,
                    ShowWarning = false,
                    BlockTrades = false
                });
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
