using System;
using System.Collections.Generic;
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
            return new List<Arbitrage>();
            /* return Database.Master.Many<Arbitrage, Arbitrage, Arbitrage>(@"
                select
                id as Id,
                from_coin_data_id
            ", new {

            }); */
        }

        public void Save()
        {

        }
    }
}