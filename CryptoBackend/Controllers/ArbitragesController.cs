using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Integrations;
using Microsoft.AspNetCore.Mvc;
using CryptoBackend.Models;
namespace CryptoBackend.Controllers
{
    [Route("api/[controller]")]
    public class ArbitragesController : Controller
    {     
        public decimal CalculateProfitPercentage(decimal priceFrom,decimal priceTo){
            return (((priceTo-priceFrom))/priceFrom);
        }
        public decimal PriceToUsd(CoinData coin){  
            return coin.Fiat.PriceUsd;
        }
        List<CoinData> coinData;

        [HttpGet]
        public List<ResponseModels.Arbitrage> GetArbitrages([FromQuery] decimal volume, [FromQuery] string fiatSymbol) {
            List<ResponseModels.Arbitrage> arbitrageList=new List<ResponseModels.Arbitrage>();
            var volumeFiat = Fiat.Find(symbol: fiatSymbol)[0];

            var coins = Coin.Find();
            // 
            foreach(var coin in coins) {
                for (int i = 0; i < coin.LastData.Count-1; i++)
                {
                    for (int j = i+1; j < coin.LastData.Count; j++)
                    {   var sellingPrice = coin.LastData[i].Bid;
                        var buyingPrice = coin.LastData[j].Ask;
                        var expectedProfitPercentage = CalculateProfitPercentage(sellingPrice,buyingPrice);
                        Arbitrage arbitrage= new Arbitrage{
                            FromCoinData = coin.LastData[i],
                            ToCoinData = coin.LastData[j],
                            ExpectedProfit = expectedProfitPercentage,
                            Volume = volume,
                            VolumeFiat = volumeFiat,
                            CreatedAt = DateTime.Now
                        };
                        arbitrageList.Add(new ResponseModels.Arbitrage{
                            Id = arbitrage.Id,
                            FromExchange = arbitrage.FromCoinData.Exchange.Name,
                            ToExchange = arbitrage.ToCoinData.Exchange.Name,
                            Coin = arbitrage.FromCoinData.Coin.Name,
                            ExpectedProfit = arbitrage.ExpectedProfit,
                            Volume = arbitrage.Volume,
                            CreatedAt = arbitrage.CreatedAt
                        });   
                        arbitrage.Save();
                    }
                }
            }
         
            return arbitrageList;
        }
    }
}