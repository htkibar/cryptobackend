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
        public decimal CalculateProfitPercentage(decimal sellingPrice,decimal buyingPrice){
            return (((sellingPrice-buyingPrice))/buyingPrice);
        }
        public decimal PriceToUsd(CoinData coin){  
            return coin.PriceFiat.PriceUsd;
        }
        List<CoinData> coinData;
        private decimal sellingPrice;
        private decimal buyingPrice;
        private string profitCurrency;
        private decimal fromBTCRate;
        private decimal toBTCRate;
        public decimal[] getBTCRate (CoinData from,CoinData to){
            decimal[] rates=new decimal[2];
            var baseSymbolQueryResultFrom = CoinData.GetBidAskForExchangeCoin(exchangeId: from.Exchange.Id, coinId: Coin.Find(symbol: "BTC")[0].Id);
            var fromBid = from.Bid;
            var toAsk = to.Ask;
            if (baseSymbolQueryResultFrom != null) {
                var baseSymbolData = baseSymbolQueryResultFrom;

                fromBid = fromBid / baseSymbolData.Bid;
                rates[0]=fromBid;
            } else {
                    rates[0]=0;
            }

            var baseSymbolQueryResultTo = CoinData.GetBidAskForExchangeCoin(exchangeId: to.Exchange.Id, coinId: Coin.Find(symbol: "BTC")[0].Id);
            
            if (baseSymbolQueryResultTo != null) {
                var baseSymbolData = baseSymbolQueryResultTo;

                toAsk = toAsk / baseSymbolData.Ask;
                rates[1]=toAsk;
            } else {
                    rates[1]=0;
            }
           return rates;
        }
        [HttpGet]
        public List<ResponseModels.Arbitrage> GetArbitrages([FromQuery] decimal volume, [FromQuery] string symbol, [FromQuery] bool isCoin) {
            List<ResponseModels.Arbitrage> arbitrageList = new List<ResponseModels.Arbitrage>();
            Fiat volumeFiat = null;
            Coin volumeCoin = null;

            if (isCoin) {
                volumeCoin = Coin.Find(symbol: symbol)[0];
            } else {
                volumeFiat = Fiat.Find(symbol: symbol)[0];
            }

            var coins = Coin.Find();
            foreach(var coin in coins) {
                // TODO: THIS DOESNT WORK AT ALLLLLLLL!!
                for (int i = 0; i < coin.LastData.Count-1; i++) {
                    for (int j = i+1; j < coin.LastData.Count; j++) {
                        CoinData from;
                        CoinData to;

                        CoinData first = coin.LastData[i];
                        CoinData second = coin.LastData[j];

                        decimal expectedProfitPercentage;

                        var firstBid = first.Bid;
                        var firstAsk = first.Ask;
                        var secondAsk = second.Ask;
                        var secondBid = second.Bid;

                        if (first.PriceIsCoin) {
                            if (isCoin) {
                                // TODO: Different symbol transform. For now, not needed as all prices are BTC.
                            } else {
                                // When price is a coin and we request USD, currently there is no way to transform.
                                continue;
                            }
                        } else {
                            if (isCoin) {
                                var baseSymbolQueryResult = CoinData.GetBidAskForExchangeCoin(exchangeId: first.Exchange.Id, coinId: volumeCoin.Id);

                                if (baseSymbolQueryResult != null) {
                                    var baseSymbolData = baseSymbolQueryResult;

                                    firstBid = firstBid / baseSymbolData.Bid;
                                    firstAsk = firstAsk / baseSymbolData.Ask;
                                } else {
                                    continue;
                                }
                            } else {
                                // TODO: Different fiat transform. For now, not needed as all prices are USD.
                            }
                        }

                        if (second.PriceIsCoin) {
                            if (isCoin) {
                                // TODO: Different symbol transform. For now, not needed as all prices are BTC.
                            } else {
                                // When price is a coin and we request USD, currently there is no way to transform.
                                continue;
                            }
                        } else {
                            if (isCoin) {
                                var baseSymbolQueryResult = CoinData.GetBidAskForExchangeCoin(exchangeId: second.Exchange.Id, coinId: volumeCoin.Id);

                                if (baseSymbolQueryResult != null) {
                                    var baseSymbolData = baseSymbolQueryResult;

                                    secondBid = secondBid / baseSymbolData.Bid;
                                    secondAsk = secondAsk / baseSymbolData.Ask;
                                } else {
                                    continue;
                                }
                            } else {
                                // TODO: Different fiat transform. For now, not needed as all prices are USD.
                            }
                        }
                        
                        if (((secondAsk - firstBid) / firstBid) >  ((firstAsk - secondBid) / secondBid)) {
                            from = first;
                            to = second;

                            sellingPrice = secondAsk;
                            buyingPrice = firstBid;

                            expectedProfitPercentage = CalculateProfitPercentage(sellingPrice,buyingPrice);
                        } else {
                            from = second;
                            to = first;

                            sellingPrice = firstAsk;
                            buyingPrice = secondBid;
                            
                            expectedProfitPercentage = CalculateProfitPercentage(sellingPrice,buyingPrice);
                        }
                    //    var BTCRates =getBTCRate(from,to);
                    //    fromBTCRate = BTCRates[0];
                    //    toBTCRate = BTCRates[1];
                        Arbitrage arbitrage = new Arbitrage {
                            FromCoinData = from,
                            ToCoinData = to,
                            ExpectedProfit = expectedProfitPercentage,
                            Volume = volume,
                            VolumeFiat = volumeFiat,
                            VolumeCoin = volumeCoin,
                            VolumeIsCoin = isCoin,
                            CreatedAt = DateTime.Now
                        };

                        arbitrageList.Add(new ResponseModels.Arbitrage{
                            FromExchange = arbitrage.FromCoinData.Exchange.Name,
                            ToExchange = arbitrage.ToCoinData.Exchange.Name,
                            Coin = arbitrage.FromCoinData.Coin.Name,
                            ExpectedProfit = arbitrage.ExpectedProfit,
                            Volume = arbitrage.Volume,
                            CreatedAt = arbitrage.CreatedAt,
                            profitCurrency=symbol,
                            ToPrice = buyingPrice,
                            FromPrice = sellingPrice,
                            ToBTCRate = toBTCRate,
                            FromBTCRate = fromBTCRate
                        });

                        arbitrage.Save();
                    }
                }
            }
         
            return arbitrageList.OrderByDescending(arbitrage => arbitrage.ExpectedProfit).ToList();
        }
    }
}