using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using STM_API.Model;
using STM_API.Model.BreezeAPIModel;
using STM_API.Services;
using System;
using System.Data.Common;
using System.Text.Json;

namespace STM_API.Hubs
{
    public class BreezeOperationHUB : Hub
    {
        public BreezeOperationHUB(BreezapiServices breezapiServices)
        {
            _breezapiServices = breezapiServices;
        }
        private readonly BreezapiServices _breezapiServices;
        public string GetConnectionId() => Context.ConnectionId;

        public async Task GetPortfolioPosition ()
        {
           PortfolioPositions results = _breezapiServices.GetPositions();
            await Clients.Caller.SendAsync("SendPortfolioPosition", results);
        }

        public async Task SetBuyPriceAlter(string symbol ,double price)
        {
            _breezapiServices.SetBuyPriceAlter(symbol,price);
            await Clients.All.SendAsync("SendSetBuyPriceAlter", "Success For " + symbol + "at Price" + price );

        }

        public async Task GetBuyStockTriggers()
        {
           var result= _breezapiServices.GetBuyStockTriggers().ToList();
            await Clients.All.SendAsync("SendGetBuyStockTriggers", result);

        }

        public async Task BuyOrSellEquity(string Symbol, int Quanity, string exchange, string marketor_Limit, string buyprice, string stoploss, string stockcode,
             string buysell)
        {
            _breezapiServices.BuyOrSellEquity(Symbol, Quanity, exchange, marketor_Limit, Convert.ToDecimal(buyprice), Convert.ToDecimal(stoploss), stockcode, buysell);
            await Clients.All.SendAsync("SendBuyOrSellEquity", "Success For " + Symbol + "at Price" + buyprice);

        }

        public async Task CaptureLiveData(string data)
        {
            try
            {
                Equities livedata = System.Text.Json.JsonSerializer.Deserialize<Equities>(data);

                string orginaltext = livedata.ttv;
                try
                {
                    double volume;
                    switch (livedata.ttv)
                    {
                        case var s when livedata.ttv.Contains("C"):
                            volume = (Convert.ToDouble(livedata.ttv.Replace("C", "")) * 10000000);
                            break;
                        case var s when livedata.ttv.Contains("L"):
                            volume = Convert.ToDouble(livedata.ttv.Replace("L", "")) * 100000;
                            break;
                        default:
                            double.TryParse(livedata.ttv, out volume);
                            break;

                    }
                    try
                    {
                        data = data.Replace(orginaltext, volume.ToString("F"));
                        data = data.Replace("}", string.Format(",\"volumeC\":\"{0}\" {1}", "" + orginaltext.ToString() + "", "}"));
                        await Clients.AllExcept(Context.ConnectionId).SendAsync("SendLiveData", data);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
