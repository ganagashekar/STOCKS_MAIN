using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using STM_API.Model;
using STM_API.Services;
using System;
using System.Data.Common;
using System.Text.Json;

namespace STM_API.Hubs
{
    public class UIHUB : Hub
    {
        public UIHUB(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }
        private readonly StockTicker _stockTicker;
        public string GetConnectionId() => Context.ConnectionId;

        public async Task GetPivotData(string Date, string Column,string GroupName,string SubGroup,string CKTNAME="")
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.ToString();
            }
            if (string.IsNullOrEmpty(Column))
            {
                Column = "last";
            }
            var results = _stockTicker.GetPivotData(Date, Column,  GroupName,  SubGroup, CKTNAME);
            await Clients.Caller.SendAsync("SendPivotData", results);
            // return _stockTicker.GetAllStocks();
        }


        public async Task GetSectorName()
        {
            List<DropdownSelect> results = _stockTicker.GetSectorName();
            await Clients.Caller.SendAsync("SendSectorName", results);
        }

        public async Task GetIndustryNewName(string SectorName)
        {
            List<DropdownSelect> results = _stockTicker.GetIndustryNewName(SectorName);
            await Clients.Caller.SendAsync("SendIndustryNewName", results);
        }

        public async Task GetGroupName(string IndustryNewName)
        {
            List<DropdownSelect> results = _stockTicker.GetGroupName(IndustryNewName);
            await Clients.Caller.SendAsync("SendGroupName", results);
        }

        public async Task GetSubgroupName(string GroupName)
        {
            List<DropdownSelect> results = _stockTicker.GetSubgroupName(GroupName);
            await Clients.Caller.SendAsync("SendSubgroupName", results);
        }

        public async Task GetAllStocks()
        {

            var results = _stockTicker.GetAllStocks().Where(x => x.open <= 300).ToList();
            await Clients.Caller.SendAsync("SendAllStocks", results);
            // return _stockTicker.GetAllStocks();
        }

        public async Task GetSTockToBuy(string onDate, string Top = "10")
        {
            try
            {
                if (string.IsNullOrEmpty(onDate))
                {
                    onDate = DateTime.Now.ToString();
                }
                var results = _stockTicker.GetBuysStocks(onDate, Top).ToList();
                await Clients.Caller.SendAsync("SendSTockToBuy", results);
            }
            catch (Exception ex)
            {

                throw;
            }
            // return _stockTicker.GetAllStocks();
        }

        public async Task GET_CKT(string onDate, string Top = "10",string CKT= "upperCktLm")
        {
            try
            {
                if (string.IsNullOrEmpty(onDate))
                {
                    onDate = DateTime.Now.ToString();
                }
                var results = _stockTicker.GET_CKT(onDate, Top, CKT).ToList();
                await Clients.Caller.SendAsync("SendGET_CKT", results);
            }
            catch (Exception ex)
            {

                throw;
            }
            // return _stockTicker.GetAllStocks();
        }

        public async Task FetchSTockToBuy(string onDate, string Top = "10")
        {
            try
            {
                if (string.IsNullOrEmpty(onDate))
                {
                    onDate = DateTime.Now.ToString();
                }
                var results = _stockTicker.GetBuysStocks(onDate, Top).ToList();
                await Clients.Caller.SendAsync("SendSTockToBuy", results);
            }
            catch (Exception ex)
            {

                throw;
            }
            // return _stockTicker.GetAllStocks();
        }

        public async Task FecthTopStocks(string Date, string Top = "10")
        
        {
            try
            {
                if (string.IsNullOrEmpty(Date))
                {
                    Date = DateTime.Now.ToString();
                }

                var results = _stockTicker.GetTopPerformerStocks(Date, Top).ToList();
                await Clients.Caller.SendAsync("SendTopStocks", results);
            }
            catch (Exception ex)
            {

                throw;
            }
            // return _stockTicker.GetAllStocks();
        }

        public async Task ExportStocksToJson()
        
        {
            _stockTicker.ExporttoJsonData();

        }

        public async Task ExportLiveStocksToJson()

        {
            _stockTicker.ExportLiveStocksToJson();

        }

        public async Task DownloadNSEData()

        {
            _stockTicker.DownloadNSEData();

        }


        public async Task SendAlertsUpperCKT()
        {
            PushServices pushServices = new PushServices();
            var results =_stockTicker.GetNotification(true,false);

            foreach(var items in results)
            {
                try
                {
                   var txxt= ": DAYS :" + (_stockTicker.GetJsonFileIndex(items.symbol, Convert.ToDouble(items.Last))) +" BACK ";

                    items.STOCKName += txxt;
                }
                catch (Exception ex)
                {

                    throw;
                }
               
            }

            var groupbyPOResult = results.GroupBy(x => x.PO_KEY_NAME);
            foreach ( var sublist in groupbyPOResult)
            {
                var chunckResult = sublist.Chunk(30);
                foreach (var list in chunckResult)
                {
                    string ttile = string.Empty;

                    await pushServices.SendPushServicesAsync(list.FirstOrDefault().title, list.FirstOrDefault().PO_KEY_TOKEN,
                        list.FirstOrDefault().user, list.FirstOrDefault().priority.ToString(), String.Join(",<br>", list.Select(x => x.STOCKName).ToArray()),
                        list.FirstOrDefault().retry.ToString(), list.FirstOrDefault().expire.ToString(), list.FirstOrDefault().sound);
                        

                }

            }
           

            
          
        }

        public async Task GetAllStocksForLoad()
        {

            //if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoks")))
            //{
            //    var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoks"));
            //    var equities = System.Text.Json.JsonSerializer.Deserialize<List<Equities>>(text).OrderBy(x => x.ltt).ToList();

            //    //var results = _stockTicker.SendAllStocksForLoad().ToList();
            //   //await Clients.Caller.SendAsync("SendAllStocksForLoad", equities);
            //}
            var results = _stockTicker.SendAllStocksForLoad().ToList();
            await Clients.Caller.SendAsync("SendAllStocksForLoad", results);
            // return _stockTicker.GetAllStocks();
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
                        data = data.Replace("}",  string.Format(",\"volumeC\":\"{0}\" {1}", ""+orginaltext.ToString()+"","}"));
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
