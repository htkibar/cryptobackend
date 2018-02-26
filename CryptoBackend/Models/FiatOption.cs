using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class FiatOption
    {
        private Guid exchangeId;
        private Exchange exchange;
        private Guid coinId;
        private Coin coin;
        private Limit depositLimits = new Limit();
        private Limit withdrawalLimits = new Limit();
        private Fee depositFees = new Fee();
        private Fee withdrawalFees = new Fee();
        private bool depositable;
        private bool withdrawable;
        public decimal DepositLimitMin { set => depositLimits.Min = value; }
        public decimal DepositLimitMax { set => depositLimits.Max = value; }
        public decimal WithdrawalLimitMin { set => withdrawalLimits.Min = value; }
        public decimal WithdrawalLimitMax { set => withdrawalLimits.Max = value; }
        public decimal DepositFeeFixed { set => depositFees.Fixed = value; }
        public decimal DepositFeePercentage { set => depositFees.Percentage = value; }
        public decimal WithdrawalFeeFixed { set => withdrawalFees.Fixed = value; }
        public decimal WithdrawalFeePercentage { set => withdrawalFees.Percentage = value; }
        public Guid ExchangeId { set => exchangeId = value; }
        public Exchange Exchange {
            get
            {
                if (exchange == null) {
                    exchange = Exchange.Find(id: exchangeId);
                }

                return exchange;
            }
            set
            {
                exchange = value;
                exchangeId = exchange.Id;
            }
        }
        public Guid CoinId { set => coinId = value; }
        public Coin Coin {
            get
            {
                if (coin == null) {
                    coin = Coin.Find(id: coinId);
                }

                return coin;
            }
            set
            {
                coin = value;
                coinId = coin.Id;
            }
        }
        public Limit DepositLimits { get => depositLimits; set => depositLimits = value; }

        public Limit WithdrawalLimits { get => withdrawalLimits; set => withdrawalLimits = value; }
        public Fee DepositFees { get => depositFees; set => depositFees = value; }
        public Fee WithdrawalFees { get => withdrawalFees; set => withdrawalFees = value; }
        public bool Depositable { get => depositable; set => depositable = value; }
        public bool Withdrawable { get => withdrawable; set => withdrawable = value; }

        public static List<FiatOption> Find(Guid exchangeId)
        {
            return Database.Master.Many<FiatOption>(@"
                select
                option.exchange_id as ExchangeId,
                option.coin_id as CoinId,
                option.deposit_limit_min as DepositLimitMin,
                option.deposit_limit_max as DepositLimitMax,
                option.withdraw_limit_min as WithdrawalLimitMin,
                option.withdraw_limit_max as WithdrawalLimitMax,
                option.deposit_fee_percentage as DepositFeePercentage,
                option.deposit_fee_fixed as DepositFeeFixed,
                option.withdraw_fee_percentage as WithdrawalFeePercentage,
                option.withdraw_fee_fixed as WithdrawalFeeFixed,
                option.depositable as Depositable,
                option.withdrawable as Withdrawable
                from fiat_dw_options as option
                where option.exchange_id = @ExchangeId
                order by option.coin_id
            ", new {
                ExchangeId = exchangeId
            }).ToList();
        }

        public int Save()
        {
            // TODO: PARALLELIZE
            return Database.Master.Run<int>(@"
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
                    withdrawable,
                    symbol
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
                    @Withdrawable,
                    @Symbol
                )
                on conflict (exchange_id, coin_id)
                do update set
                deposit_limit_min=@DepositLimitMin,
                deposit_limit_max=@DepositLimitMax,
                withdraw_limit_min=@WithdrawLimitMin,
                withdraw_limit_max=@WithdrawLimitMax,
                deposit_fee_percentage=@DepositFeePercentage,
                deposit_fee_fixed=@DepositFeeFixed,
                withdraw_fee_percentage=@WithdrawFeePercentage,
                withdraw_fee_fixed=@WithdrawFeeFixed,
                depositable=@Depositable,
                withdrawable=@Withdrawable;
            ", new {
                ExchangeId = exchangeId,
                CoinId = coinId,
                DepositLimitMin = depositLimits.Min,
                DepositLimitMax = depositLimits.Max,
                WithdrawLimitMin = withdrawalLimits.Min,
                WithdrawLimitMax = withdrawalLimits.Max,
                DepositFeePercentage = depositFees.Percentage,
                DepositFeeFixed = depositFees.Fixed,
                WithdrawFeePercentage = withdrawalFees.Percentage,
                WithdrawFeeFixed = withdrawalFees.Fixed,
                Depositable = depositable,
                Withdrawable = withdrawable
            });
        }
    }
}