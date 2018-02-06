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
        private Coin coin;
        private int minimumConfirmations;
        private Limit depositLimits;
        private Limit withdrawalLimits;
        private Fee depositFees;
        private Fee withdrawalFees;
        private bool depositable;
        private bool withdrawable;

        public Coin Coin { get => coin; set => coin = value; }
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
        private Fiat fiat;
        private Limit depositLimits;
        private Limit withdrawalLimits;
        private Fee depositFees;
        private Fee withdrawalFees;
        private bool depositable;
        private bool withdrawable;
        
        public Fiat Fiat { get => fiat; set => fiat = value; }
        public Limit DepositLimits { get => depositLimits; set => depositLimits = value; }
        public Limit WithdrawalLimits { get => withdrawalLimits; set => withdrawalLimits = value; }
        public Fee DepositFees { get => depositFees; set => depositFees = value; }
        public Fee WithdrawalFees { get => withdrawalFees; set => withdrawalFees = value; }
        public bool Depositable { get => depositable; set => depositable = value; }
        public bool Withdrawable { get => withdrawable; set => withdrawable = value; }
    }

    public class Exchange
    {
        private class CoinDWOption {
            public Guid CoinId { get; set; }
            public decimal DepositLimitMin { get; set; }
            public decimal DepositLimitMax { get; set; }
            public decimal WithdrawLimitMin { get; set; }
            public decimal WithdrawLimitMax { get; set; }
            public decimal DepositFeePercentage { get; set; }
            public decimal DepositFeeFixed { get; set; }
            public decimal WithdrawFeePercentage { get; set; }
            public decimal WithdrawFeeFixed { get; set; }
            public bool Depositable { get; set; }
            public bool Withdrawable { get; set; }
            public string Symbol { get; set; }
            public int MinimumConfirmations { get; set; }
        }
        private Guid id = Guid.Empty;
        private string name;
        private Country country;
        private bool showWarning;
        private bool blockTrades;
        private List<ExchangeCoin> coins = new List<ExchangeCoin>();
        private List<ExchangeFiat> fiats = new List<ExchangeFiat>();

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Country Country { get => country; set => country = value; }
        public bool ShowWarning { get => showWarning; set => showWarning = value; }
        public bool BlockTrades { get => blockTrades; set => blockTrades = value; }
        public List<ExchangeCoin> Coins { get => coins; set => coins = value; }
        public List<ExchangeFiat> Fiats { get => fiats; set => fiats = value; }

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

            foreach (var exchange in exchanges) {
                var coinDWOptions = Database.Master.Many<CoinDWOption>(@"
                    select
                    option.coin_id as CoinId,
                    option.deposit_limit_min as DepositLimitMin,
                    option.deposit_limit_max as DepositLimitMax,
                    option.withdraw_limit_min as WithdrawLimitMin,
                    option.withdraw_limit_max as WithdrawLimitMax,
                    option.deposit_fee_percentage as DepositFeePercentage,
                    option.deposit_fee_fixed as DepositFeeFixed,
                    option.withdraw_fee_percentage as WithdrawFeePercentage,
                    option.withdraw_fee_fixed as WithdrawFeeFixed,
                    option.depositable as Depositable,
                    option.withdrawable as Withdrawable,
                    option.symbol as Symbol,
                    transfer_time.minimum_confirmations as MinimumConfirmations
                    from coin_dw_options as option
                    left join transfer_times as transfer_time on
                    (transfer_time.exchange_id = @ExchangeId
                    and transfer_time.coin_id = option.coin_id)
                    where option.exchange_id = @ExchangeId
                    order by option.coin_id
                ", new {
                    ExchangeId = exchange.Id
                });

                foreach (var coinDWOption in coinDWOptions) {
                    var coins = Coin.Find(id: coinDWOption.CoinId);

                    if (coins.Count > 0) {
                        var coin = coins[0];
                    
                        exchange.coins.Add(new ExchangeCoin{
                            Coin = coin,
                            MinimumConfirmations = coinDWOption.MinimumConfirmations,
                            DepositLimits = new Limit {
                                Min = coinDWOption.DepositLimitMin,
                                Max = coinDWOption.DepositLimitMax
                            },
                            WithdrawalLimits = new Limit {
                                Min = coinDWOption.WithdrawLimitMin,
                                Max = coinDWOption.WithdrawLimitMax
                            },
                            DepositFees = new Fee {
                                Percentage = coinDWOption.DepositFeePercentage,
                                Fixed = coinDWOption.DepositFeeFixed
                            },
                            WithdrawalFees = new Fee {
                                Percentage = coinDWOption.WithdrawFeePercentage,
                                Fixed = coinDWOption.WithdrawFeeFixed
                            },
                            Depositable = coinDWOption.Depositable,
                            Withdrawable = coinDWOption.Withdrawable
                        });
                    }
                }
            }

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
                            tasks.Add(Database.Master.RunAsync<int>(@"
                                insert into coin_dw_options
                                (
                                    exchange_id,
                                    coin_id,
                                    deposit_limit_min,
                                    deposit_limit_max,
                                    withdraw_limit_min,
                                    withdraw_limit_max,
                                    deposit_fee_percentage,
                                    deposit_fee_fixed,
                                    withdraw_fee_percentage,
                                    withdraw_fee_fixed,
                                    depositable,
                                    withdrawable
                                )
                                values
                                (
                                    @ExchangeId,
                                    @CoinId,
                                    @DepositLimitMin,
                                    @DepositLimitMax,
                                    @WithdrawLimitMin,
                                    @WithdrawLimitMax,
                                    @DepositFeePercentage,
                                    @DepositFeeFixed,
                                    @WithdrawFeePercentage,
                                    @WithdrawFeeFixed,
                                    @Depositable,
                                    @Withdrawable
                                );
                            ", new {
                                ExchangeId = Id,
                                CoinId = coin.Coin.Id,
                                DepositLimitMin = coin.DepositLimits.Min,
                                DepositLimitMax = coin.DepositLimits.Max,
                                WithdrawLimitMin = coin.WithdrawalLimits.Min,
                                WithdrawLimitMax = coin.WithdrawalLimits.Max,
                                DepositFeePercentage = coin.DepositFees.Percentage,
                                DepositFeeFixed = coin.DepositFees.Fixed,
                                WithdrawFeePercentage = coin.WithdrawalFees.Percentage,
                                WithdrawFeeFixed = coin.WithdrawalFees.Fixed,
                                Depositable = coin.Depositable,
                                Withdrawable = coin.Withdrawable
                            }));
                        }

                        foreach (var fiat in fiats) {
                            tasks.Add(Database.Master.RunAsync<int>(@"
                                insert into fiat_dw_options
                                (
                                    exchange_id,
                                    fiat_id,
                                    deposit_limit_min,
                                    deposit_limit_max,
                                    withdraw_limit_min,
                                    withdraw_limit_max,
                                    deposit_fee_percentage,
                                    deposit_fee_fixed,
                                    withdraw_fee_percentage,
                                    withdraw_fee_fixed,
                                    depositable,
                                    withdrawable
                                )
                                values
                                (
                                    @ExchangeId,
                                    @FiatId,
                                    @DepositLimitMin,
                                    @DepositLimitMax,
                                    @WithdrawLimitMin,
                                    @WithdrawLimitMax,
                                    @DepositFeePercentage,
                                    @DepositFeeFixed,
                                    @WithdrawFeePercentage,
                                    @WithdrawFeeFixed,
                                    @Depositable,
                                    @Withdrawable
                                );
                            ", new {
                                ExchangeId = Id,
                                FiatId = fiat.Fiat.Id,
                                DepositLimitMin = fiat.DepositLimits.Min,
                                DepositLimitMax = fiat.DepositLimits.Max,
                                WithdrawLimitMin = fiat.WithdrawalLimits.Min,
                                WithdrawLimitMax = fiat.WithdrawalLimits.Max,
                                DepositFeePercentage = fiat.DepositFees.Percentage,
                                DepositFeeFixed = fiat.DepositFees.Fixed,
                                WithdrawFeePercentage = fiat.WithdrawalFees.Percentage,
                                WithdrawFeeFixed = fiat.WithdrawalFees.Fixed,
                                Depositable = fiat.Depositable,
                                Withdrawable = fiat.Withdrawable
                            }));
                        }
                        
                        Task.WaitAll(tasks.ToArray());
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
