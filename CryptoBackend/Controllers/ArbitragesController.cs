using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Integrations;
using Microsoft.AspNetCore.Mvc;
using CryptoBackend.Models;
namespace CryptoBackend.Controllers
{

    class ArbitragesController
    {     
        List<Arbitrage> exchangeProfitList=new List<Arbitrage>();
        public decimal CalculateProfitPercentage(decimal priceFrom,decimal priceTo){
            return ((100*(priceTo-priceFrom))/priceFrom);
        }
        public decimal PricesToUsd(CoinData coin){  
            
            return coin.Fiat.PriceUsd;
        }
        public void UpdateArbitrage(){
            
 
            

         
        }
        public List<Arbitrage> GetArbitrageList() {
            List<Arbitrage> exchangeProfitList=new List<Arbitrage>();

            var coins =Coin.Find();
            



            return exchangeProfitList;
        }

    }
}