using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBackend.Integrations;
using CryptoBackend.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CryptoBackend.Controllers
{
    [Route("api/[controller]")]
    public class ExchangesController : Controller
    {
        [HttpGet]
        public IEnumerable<ResponseModels.Exchange> Get()
        {
            var exchanges = Exchange.Find();
            var response = new List<ResponseModels.Exchange>();

            foreach (var exchange in exchanges) {
                var responseCoinOptions = new List<ResponseModels.CoinOption>();

                foreach (var coinOption in exchange.Coins) {
                    var coinOptionData = new List<ResponseModels.CoinData>();

                    foreach (var data in coinOption.Coin.LastData) {
                        coinOptionData.Add(new ResponseModels.CoinData{
                            Id = data.Id,
                            UpdatedAt = data.UpdatedAt,
                            Volume = data.Volume,
                            High = data.High,
                            Low = data.Low,
                            Ask = data.Ask,
                            Bid = data.Bid,
                            LastPrice = data.LastPrice
                        });
                    }

                    var coin = new ResponseModels.Coin{
                        Id = coinOption.Coin.Id,
                        Name = coinOption.Coin.Name,
                        Symbol = coinOption.Coin.Symbol,
                        TransferTimeMins = coinOption.Coin.TransferTimeMins,
                        LastData = coinOptionData
                    };

                    responseCoinOptions.Add(new ResponseModels.CoinOption{
                        Coin = coin,
                        DepositLimits = coinOption.DepositLimits,
                        WithdrawalLimits = coinOption.WithdrawalLimits,
                        DepositFees = coinOption.DepositFees,
                        WithdrawalFees = coinOption.WithdrawalFees,
                        Depositable = coinOption.Depositable,
                        Withdrawable = coinOption.Withdrawable,
                        Symbol = coinOption.Symbol
                    });
                } 

                response.Add(new ResponseModels.Exchange{
                    Id = exchange.Id,
                    Name = exchange.Name,
                    Country = exchange.Country,
                    ShowWarning = exchange.ShowWarning,
                    BlockTrades = exchange.BlockTrades,
                    Coins = responseCoinOptions
                });
            }

            return response;
        }
    }
}
