using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using STM_API.AutomationModel;
using STM_API.AutomationModels;
using STM_API.Extention;
using STM_API.Model;
using STM_API.Model.BreezeAPIModel;
using STM_API.Services;
using STMAPI.AutomationModels;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public async Task GetPortfolioPosition()
        {
            PortfolioPositions results = _breezapiServices.GetPositions();
            await Clients.Caller.SendAsync("SendPortfolioPosition", results);
        }

        public async Task SetBuyPriceAlter(string symbol, double price)
        {
            _breezapiServices.SetBuyPriceAlter(symbol, price);
            await Clients.All.SendAsync("SendSetBuyPriceAlter", "Success For " + symbol + "at Price " + price);
        }
        public async Task SetBuyChangeAlter(string symbol, string Change)
        {
            _breezapiServices.SetBuyChangeAlter(symbol, (Change));
            await Clients.All.SendAsync("SendSetBuyChangeAlterNew", "Success For " + symbol + "at Change " + Change);
        }

        public async Task SetForT3(string symbol, string Change)
        {
            _breezapiServices.SetForT3(symbol, Change);
            await Clients.All.SendAsync("SendSetForT3", "Success For " + symbol + "at T3 " + Change);
        }

        public async Task SetForWacthList(string symbol, string Change)
        {
            _breezapiServices.SetForWacthList(symbol, Change);
            await Clients.All.SendAsync("SetForWacthList", "Success For " + symbol + "at WacthList " + Change);
        }


        public async Task GetDashboard_option_data()
        {
            var result = _breezapiServices.GetDashboard_Stats();
            await Clients.Caller.SendAsync("SendGetDashboard_option_data", result);
        }


        public async Task GetBuyStockTriggers(int Id)
        {
           

            try
            {
                if (System.IO.File.Exists(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoksForAutomation", Id)))
                {
                    var text = System.IO.File.ReadAllText(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoksForAutomation", Id));
                    var equities = JsonConvert.DeserializeObject<List<BuyStockAlertModel>>(text).ToList();

                    var result = _breezapiServices.GetBuyStockTriggers().ToList();
                    await Clients.Caller.SendAsync("SendGetBuyStockTriggers", result.Where(x => equities.Any(y => y.symbol == x.symbol)).ToList());
                    //var results = _stockTicker.SendAllStocksForLoad().ToList();
                    //await Clients.Caller.SendAsync("SendAllStocksForLoad", equities);
                }
            }
            catch (Exception ex)
            {

                await Clients.Caller.SendAsync("SendAllStocksForLoad", ex);
            }
            //var result = _breezapiServices.GetBuyStockTriggers().ToList();
            //await Clients.All.SendAsync("SendGetBuyStockTriggers", result);

        }

        public async Task BuyOrSellEquity(string Symbol, int Quanity, string exchange, string marketor_Limit, string buyprice, string stoploss, string stockcode,
             string buysell)
        {
            _breezapiServices.BuyOrSellEquity(Symbol, Quanity, exchange, marketor_Limit, Convert.ToDecimal(buyprice), Convert.ToDecimal(stoploss), stockcode, buysell);
            await Clients.All.SendAsync("SendBuyOrSellEquity", "Success For " + Symbol + "at Price" + buyprice);

        }

        public async Task GetAllStocksForLoadAutomation(int Id)
        {
          

            if (System.IO.File.Exists(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoksForAutomation", Id)))
            {

                FileInfo fileInfo = new FileInfo(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoksForAutomation", Id));
                var created = fileInfo.CreationTime; //File Creation
                var lastmodified = fileInfo.LastWriteTime;//File Modification

                if (lastmodified.Date != DateTime.Now.Date)
                {
                    _breezapiServices.ExportAutomationLiveStocksToJson();
                }


                var text = System.IO.File.ReadAllText(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoksForAutomation", Id));
                var equities = System.Text.Json.JsonSerializer.Deserialize<List<Equities>>(text).ToList();
                await Clients.All.SendAsync("SendGetAllStocksForLoadAutomation", equities.Select(x => x.symbol).ToList());
                //var results = _stockTicker.SendAllStocksForLoad().ToList();
                //await Clients.Caller.SendAsync("SendAllStocksForLoad", equities);
            }
            else
            {
                _breezapiServices.ExportAutomationLiveStocksToJson();
                GetAllStocksForLoadAutomation(Id);
            }
            //var results = _stockTicker.SendAllStocksForLoad().ToList();
            //await Clients.Caller.SendAsync("SendAllStocksForLoad", results);
            // return _stockTicker.GetAllStocks();
        }

        public async Task ExportAutomationFeedFile()
        {
            _breezapiServices.ExportAutomationLiveStocksToJson();
            ExecuteBuyOrSell();
            await Clients.All.SendAsync("SendExportAutomationFeedFile", "Success");
        }


        public async Task GetTopStockforBuyAutomation(bool Isorderbysize = true, string Date = "")
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.Date.ToShortDateString();
            }
            List<PredictedStocksAutomation> result = _breezapiServices.GetTopStockforBuyAutomation(Isorderbysize, Date).ToList();
            await Clients.All.SendAsync("SendExportBuyStockAlterFromAPP_IND", result);
        }


        public async Task GetStocksStatisticsBenchMarks(int number, bool Isorderbysize = true, string Date = "" )
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.Date.ToShortDateString();
            }
            List<StockTalibStats> result = _breezapiServices.GetStocksStatisticsBenchMarks(number, Isorderbysize, Date).ToList();
            await Clients.All.SendAsync("SendStocksStatisticsBenchMarks", (result));
        }



        public async Task ExportBuyStockAlterFromAPP_IND(string data)
        {

            await Clients.All.SendAsync("ExportBuyStockAlterFromAPP_IND_Filesave", data);

            //if (!string.IsNullOrEmpty(data))
            //{
            //    var livedata = System.Text.Json.JsonSerializer.Deserialize<List<EquityModelAutomation>>(data, new JsonSerializerOptions
            //    {
            //        PropertyNameCaseInsensitive = true
            //    }).ToList().
            //        Select(x => new Ticker_Stocks_Histry_Extended_Mdl
            //        {
            //            symbol = x.symbol,
            //            Ltt = x.lttDateTime < DateTime.Now.AddDays(-1).Date ? DateTime.Now : x.lttDateTime,
            //            //Createdon = DateTime.Now,
            //            BearishCount = x.bearishCount,
            //            BulishCount = x.bullishCount,
            //            Match = x.match,
            //            Data = x.data,
            //            Stock_Name = x.stockName,
            //            StockCode = x.stockCode,
            //            BulishCount_100 = x.bullishCount_100,
            //            BulishCount_95 = x.bullishCount_95,
            //            Volumedifference = x.volumeDifferecne,
            //            TriggredPrice = x.triggredPrice,
            //            TriggredLtt = x.triggredLtt.ToString() == "01-01-0001 12:00:00 AM" ? null : x.triggredLtt
            //        }).ToList().ToDataTable();







            //    CopyToSQL(livedata, "dbo.Ticker_Stocks_Histry_Extended_Ticks");

            //}
        }

        public async Task ExportBuyStockAlterFromAPP(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var livedata = System.Text.Json.JsonSerializer.Deserialize<List<EquityModelAutomation>>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }).ToList().
                    Select(x => new Ticker_Stocks_Histry_Extended_Mdl
                    {
                        symbol = x.symbol,
                        Ltt = x.lttDateTime < DateTime.Now.AddDays(-1).Date ? DateTime.Now : x.lttDateTime,
                        //Createdon = DateTime.Now,
                        BearishCount = x.bearishCount,
                        BulishCount = x.bullishCount,
                        Match = x.match

                    }).ToList()
                    .ToDataTable();
                CopyToSQL(livedata, "dbo.Ticker_Stocks_Histry_Extended");

            }
        }

        private void CopyToSQL(DataTable dt, string tablename)
        {




            try
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, null)
                    {
                        DestinationTableName = tablename,
                        BatchSize = dt.Rows.Count
                    };
                    //foreach (DataColumn dataColumn in dt.Columns)
                    //{
                    //    bc.ColumnMappings.Add(dataColumn.ColumnName, dataColumn.ColumnName);
                    //}
                    //bc.ColumnMappings.Add("symbol", "symbol");
                    //bc.ColumnMappings.Add("ltt", "ltt");
                    //bc.ColumnMappings.Add("Createdon", "Createdon");
                    //bc.ColumnMappings.Add("BearishCount", "BearishCount");
                    //bc.ColumnMappings.Add("BulishCount", "lttBulishCount");
                    //   bc.ColumnMappings.Add("Match", "Match");
                    conn.Open();
                    bc.WriteToServer(dt);
                    conn.Close();
                    bc.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task SendBullish100(string stockName, string volume, string price)
        {
            PushServices pushServices = new PushServices();
            await pushServices.SendPushServicesAsyncASAP(stockName, volume, price);
        }
        public async Task SendPOPlaceOrder(string title, string message)
        {
            PushServices pushServices = new PushServices();
            await pushServices.SendPushServicesAsyncASAPMsg(title, message);
        }
        public async Task CaptureLiveDataForAutomation(string data)
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
                        if (!string.IsNullOrEmpty(orginaltext) && volume > 0) { data = data.Replace(orginaltext, volume.ToString("F")); }
                        data = data.Replace("}", string.Format(",\"volumeC\":\"{0}\" {1}", "" + orginaltext.ToString() + "", "}"));
                        await Clients.All.SendAsync("SendCaptureLiveDataForAutomation", data);
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


        public async Task CaptureLiveDataForAutomation_Auto(string data)
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
                        if (!string.IsNullOrEmpty(orginaltext) && volume > 0) { data = data.Replace(orginaltext, volume.ToString("F")); }
                        data = data.Replace("}", string.Format(",\"volumeC\":\"{0}\" {1}", "" + orginaltext.ToString() + "", "}"));
                        await Clients.All.SendAsync("SendCaptureLiveDataForAutomation_Auto", data);
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


        public async Task CaptureLiveDataForBuyForAutomationNIFTY(string data)
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
                        if (!string.IsNullOrEmpty(orginaltext) && volume > 0) { data = data.Replace(orginaltext, volume.ToString("F")); }
                        data = data.Replace("}", string.Format(",\"volumeC\":\"{0}\" {1}", "" + orginaltext.ToString() + "", "}"));
                        await Clients.All.SendAsync("SendCaptureLiveDataForBuyForAutomationNIFTY", data);
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

        public async Task CaptureLiveDataForBuyForAutomationNIFTYBANK(string data)
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
                        if (!string.IsNullOrEmpty(orginaltext) && volume > 0) { data = data.Replace(orginaltext, volume.ToString("F")); }
                        data = data.Replace("}", string.Format(",\"volumeC\":\"{0}\" {1}", "" + orginaltext.ToString() + "", "}"));
                        await Clients.All.SendAsync("SendCaptureLiveDataForBuyForAutomationNIFTYBANK", data);
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

        public async Task CaptureLiveDataForBuyForAutomation(string data)
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
                        if (!string.IsNullOrEmpty(orginaltext) && volume > 0) { data = data.Replace(orginaltext, volume.ToString("F")); }
                        data = data.Replace("}", string.Format(",\"volumeC\":\"{0}\" {1}", "" + orginaltext.ToString() + "", "}"));
                        await Clients.All.SendAsync("SendCaptureLiveDataForBuyForAutomation", data);
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


        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            //process.WaitForExit

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }
        public async Task ExecuteBuyOrSell()
        {

            ExecuteCommand(@"C:\Hosts\Py\BuyOrSell.bat");

        }

        public async Task GetListOfSymbols()
        {
            var results = _breezapiServices.SendAllStocksForLoad();
            await Clients.All.SendAsync("SendGetListOfSymbols", results.ToList());
        }

        public async Task GetBuyForAutomation()
        {
            var results = _breezapiServices.GetBuyForAutomation();
            await Clients.All.SendAsync("SendGetBuyForAutomation", results.ToList());
        }

        public async Task GetBuyForAutomation_Auto()
        {
            var results = _breezapiServices.GetBuyForAutomation();
            await Clients.All.SendAsync("SendGetBuyForAutomation_Auto", results.ToList());
        }


    }
}
