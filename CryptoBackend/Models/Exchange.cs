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

        public void Save()
        {
            if (id == Guid.Empty) {
                using (var transaction = Database.Master.BeginTransaction()) {
                    try {
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

                        foreach (var coin in coins) {
                            Database.Master.RunAsync<int>(@"
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
                            });
                        }

                        foreach (var fiat in fiats) {
                            Database.Master.RunAsync<int>(@"
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
                            });
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
