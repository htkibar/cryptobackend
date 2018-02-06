using System;
using System.Collections.Generic;
using System.Linq;
using CryptoBackend.Utils;

namespace CryptoBackend.Models
{
    public class Arbitrage
    {
        private Guid id;
        private CoinData fromCoinData;
        private CoinData toCoinData;
        private decimal expectedProfit;
        private decimal volume;
        private Fiat volumeFiat;
        private DateTime createdAt;

        public Guid Id { get => id; set => id = value; }
        public CoinData FromCoinData { get => fromCoinData; set => fromCoinData = value; }
        public CoinData ToCoinData { get => toCoinData; set => toCoinData = value; }
        public decimal ExpectedProfit { get => expectedProfit; set => expectedProfit = value; }
        public decimal Volume { get => volume; set => volume = value; }
        public Fiat VolumeFiat { get => volumeFiat; set => volumeFiat = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }

        public static List<Arbitrage> Find(
            Guid? id = null,
            Guid? fromCoinDataId = null,
            Guid? toCoinDataId = null,
            Guid? fromCoinId = null,
            Guid? toCoinId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
        )
        {
            var sql = @"
                select
                arbitrage.id as Id,
                arbitrage.from_coin_data_id as FromCoinDataId,
                arbitrage.to_coin_data_id as ToCoinDataId,
                arbitrage.expected_profit as ExpectedProfit
                arbitrage.volume as Volume
                arbitrage.volume_fiat_id as VolumeFiatId
                arbitrage.created_at as CreatedAt
                coin.id as CoinId
                from arbitrages as arbitrage left join coins as coin
                where 1=1
            ";

            if(id!=null) {
                sql += @" and arbitrage.id = @ArbitrageId";
            }

            if(fromCoinDataId!=null) {
                sql += @" and arbitrage.from_coin_data_id = @FromCoinDataId";                
            }

            if(toCoinDataId!=null) {
                sql += @" and arbitrage.toCoinDataId = @ToCoinDataId";    
            }

            if(fromCoinId!=null) {
                sql += @" and coin.id = @FromCoinId";    
            }

            if(toCoinId!=null) {
                sql += @" and coin.id = @ToCoinId";    
            }

            if(fromDate!=null) {
                sql += @" and arbitrage.created_at = @FromDate";    
            }

            if(toDate!=null) {
                sql += @" and arbitrage.created_at = @ToDate";    
            }
            
            sql += @" order by arbitrage.id";



            return Database.Master.Many<Arbitrage>(sql,new {
                FromDate=fromDate,
                ToDate=toDate,
                FromCoinId=fromCoinId,
                ToCoinId=toCoinId,
                FromCoinDataId=fromCoinDataId,
                ToCoinDataId=toCoinDataId,
                ArbitrageId=id
            }).ToList();
            // return new List<Arbitrage>();
            /* return Database.Master.Many<Arbitrage, Arbitrage, Arbitrage>(@"
                select
                id as Id,
                from_coin_data_id
            ", new {

            }); */
        }

        public void Save()
        {
            Database.Master.Run<Guid>(@"
                insert into coin_data
                (
                    id,
                    from_coin_data_id,
                    to_coin_data_id,
                    expected_profit,
                    volume,
                    volume_fiat_id
                    created_id
                )
                values
                (
                    @Id,
                    @FromCoinDataId,
                    @ToCoinDataId,
                    @ExpectedProfit,
                    @Volume
                    @VolumeFiatId,
                    @CreatedId
                );
            ", new {
                Id = Guid.NewGuid(),
                FromCoinDataId = FromCoinData.Id,
                ToCoinDataId = ToCoinData.Id,
                ExpectedProfit = ExpectedProfit,
                Volume = Volume,
                VolumeFiatId = volumeFiat.Id,
                CreatedId = DateTime.Now
            });
        }
    }
}