
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSNStocks.Models;
using MSNStocks.Models.results;
using MSNStocks.Query;
using MSNStocks.Result;
using MSNStocks.WebApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MSNStocks
{
    public static class API_MSN_Library
    {


        public static async Task getInitStocks()
        {




            using (var db = new STOCKContext())
            {
                Console.WriteLine("Database Connected");
                Console.WriteLine();
                Console.WriteLine("Listing Category Sales For 1997s");
                var equites = db.Equitys.ToList().Where(x => x.Symbol.Contains("1.1!")).Where(x => x.MSN_SECID == null);



                foreach (var equity in equites)
                {

                    try
                    {
                        var result = await HttpHelper.Get<StockQuery>("https://services.bingapis.com/", string.Format("contentservices-finance.csautosuggest/api/v1/Query?query={0}&market=BSE&count=1", equity.SecurityId));
                        if (result.data.stocks.Count > 0)
                        {



                            var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
                            Console.WriteLine(findresult.SecId);
                            //System.Threading.Thread.Sleep(1000);
                            try
                            {
                                equity.MSN_SECID = findresult.SecId;
                                var stockresult = await getstockDetails(findresult.SecId);
                                if (stockresult.equity != null)
                                {

                                    equity.Recommondations = stockresult.equity.analysis.estimate.recommendation ?? "Null";
                                    equity.JsonData = JsonConvert.SerializeObject(stockresult);
                                }
                                // System.Threading.Thread.Sleep(1000);
                            }
                            catch (Exception)
                            {
                                System.Threading.Thread.Sleep(1000);

                            }
                        }
                        else
                        {
                            equity.Recommondations = "NoValue";
                        }

                    }
                    catch (Exception ex)
                    {

                        var result = await HttpHelper.Get<StockQuery>("https://services.bingapis.com/", string.Format("contentservices-finance.csautosuggest/api/v1/Query?query={0}&market=nse&count=1", equity.SecurityName.Replace("Limited", "").Replace("LTD.", "")));
                        var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
                        var stockresult = await getstockDetails(findresult.SecId);
                        equity.Recommondations = stockresult.equity.analysis.estimate.recommendation ?? "Null";
                    }
                    db.Entry(equity).State = EntityState.Modified;
                    db.SaveChanges();

                    Console.WriteLine(equity.SecurityName);
                    Console.WriteLine(equity.Id);

                }
                
            }
        }

        public static string ExecuteCurl(string curlCommand, int timeoutInSeconds = 60)
        {



            using (var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "curl.exe",
                    Arguments = curlCommand.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.SystemDirectory
                }
            })
            {
                proc.Start();

                //proc.WaitForExit(timeoutInSeconds * 1000);

                return proc.StandardOutput.ReadToEnd();
            }
        }

        public static int GetFinancialYearStart(DateTime input)
        {
            return input.Month < 9 ? input.Year - 1 : input.Year;
        }
        public static async Task getInitStocksFromSECIDForBSEResults()
        {
            string quatername = "DEC-23";
            List<Stock_Financial_Results> stock_Financial_Resultslistdata_failed = new List<Stock_Financial_Results>();
            try
            {
                using (var db = new STOCKContext())
                {
                    Console.WriteLine("Database Connected");
                    Console.WriteLine();
                    Console.WriteLine("Listing Category Sales For 1997s");
                    var equites = db.Equitys.ToList().Where(x => x.MSN_SECID != null)
                        // .Where(x=>x.Symbol== "1.1!500189")
                        //.Where(x=>x.FinancialUpdatedOn ==null).
                        .Where(x => x.IsLatestQuaterUpdated == false);
                    // Where(x => Convert.ToDateTime(x.UpdatedOn) != Convert.ToDateTime(DateTime.Now));
                    //db.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.[Stock_Financial_Results]");
                    foreach (var item in equites)
                    {
                        Console.WriteLine(item.Symbol);
                        try
                        {
                            var Stock_Financial_Results_obj = db.Stock_Financial_Results.Where(x => x.Symbol == item.Symbol).ToList();


                            List<Stock_Financial_Results> stock_Financial_Resultslistdata = new List<Stock_Financial_Results>();

                            var cmd = @"curl ""https://api.bseindia.com/BseIndiaAPI/api/TabResults_PAR/w?scripcode={0}&tabtype=RESULTS"" ^
  -H ""authority: api.bseindia.com"" ^
  -H ""accept: application/json, text/plain, */*"" ^
  -H ""accept-language: en-US,en;q=0.9"" ^
  -H ""origin: https://www.bseindia.com"" ^
  -H ""referer: https://www.bseindia.com/"" ^
  -H ""sec-ch-ua: ^\^""Not A(Brand^\^"";v=^\^""99^\^"", ^\^""Microsoft Edge^\^"";v=^\^""121^\^"", ^\^""Chromium^\^"";v=^\^""121^\^"""" ^
  -H ""sec-ch-ua-mobile: ?0"" ^
  -H ""sec-ch-ua-platform: ^\^""Windows^\^"""" ^
  -H ""sec-fetch-dest: empty"" ^
  -H ""sec-fetch-mode: cors"" ^
  -H ""sec-fetch-site: same-site"" ^
  -H ""user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36 Edg/121.0.0.0"" ^";


                            //var output = ExecuteCurl(string.Format(cmd,1, 20231026, 20231026));

                            string output = ExecuteCurl(string.Format(cmd, item.Symbol.Replace("1.1!", "")));
                            if (!string.IsNullOrEmpty(output))
                            {

                                var data = JsonConvert.DeserializeObject<string>(output.ToString());
                                var Resp_obj = JsonConvert.DeserializeObject<FinancialResults>(data);


                                for (int i = 0; i < 6; i++)
                                {
                                    Stock_Financial_Results obj_v1 = new Stock_Financial_Results();

                                    obj_v1.Symbol = item.Symbol;
                                    if (i == 0 || i == 3)
                                    {
                                        obj_v1.VType = "V1";
                                        obj_v1.QuarterEnd = DateTime.ParseExact(Resp_obj.col2, "MMM-yy", CultureInfo.InvariantCulture).AddMonths(+1).AddDays(-1);
                                        obj_v1.CREATED_ON = DateTime.Now;

                                        obj_v1.Stock_Name = item.SecurityName;
                                        obj_v1.URL = Resp_obj.resultinS.FirstOrDefault().LQ;
                                    }
                                    if (!string.IsNullOrEmpty(Resp_obj.col3) && (i == 1 || i == 4))
                                    {
                                        obj_v1.VType = "V2";
                                        obj_v1.QuarterEnd = DateTime.ParseExact(Resp_obj.col3, "MMM-yy", CultureInfo.InvariantCulture).AddMonths(+1).AddDays(-1);
                                        obj_v1.CREATED_ON = DateTime.Now;

                                        obj_v1.Stock_Name = item.SecurityName;
                                        obj_v1.URL = Resp_obj.resultinS.FirstOrDefault().LLQ;
                                    }
                                    if (i == 2 || i == 5)
                                    {
                                        obj_v1.VType = "V3";



                                        if (Resp_obj.col4.Contains("FY"))
                                        {
                                            var year = GetFinancialYearStart(DateTime.Now);
                                            obj_v1.QuarterEnd = DateTime.ParseExact("03-" + year, "MM-yyyy", CultureInfo.InvariantCulture).AddMonths(+1).AddDays(-1);
                                            obj_v1.CREATED_ON = DateTime.Now;

                                            obj_v1.Stock_Name = item.SecurityName;
                                            obj_v1.URL = Resp_obj.resultinS.FirstOrDefault().FY;
                                        }
                                    }

                                    stock_Financial_Resultslistdata.Add(obj_v1);



                                }
                                {
                                    int k = 0;
                                    foreach (var res_item in Resp_obj.resultinCr)
                                    {

                                        if (k == 0)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[0].Revenue = Revenue_v1;
                                            stock_Financial_Resultslistdata[0].CurrencyIn = "CR";

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[1].Revenue = Revenue_v2;
                                            stock_Financial_Resultslistdata[1].CurrencyIn = "CR";

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[2].Revenue = Revenue_v3;
                                            stock_Financial_Resultslistdata[2].CurrencyIn = "CR";

                                            if (Revenue_v2 > 0)
                                                stock_Financial_Resultslistdata[0].RevenueIncrease = ((Revenue_v1 - Revenue_v2) / Revenue_v2) * 100;
                                            stock_Financial_Resultslistdata[0].RevenueDifference = ((Revenue_v1 - Revenue_v2));
                                            //stock_Financial_Resultslistdata[1].RevenueIncrease = ((Revenue_v2 - Revenue_v3) / Revenue_v3) * 100;
                                        }
                                        if (k == 1)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[0].NET_PROFIT = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[1].NET_PROFIT = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[2].NET_PROFIT = Revenue_v3;

                                            if (Revenue_v2 > 0)
                                                stock_Financial_Resultslistdata[0].Profit_Increase = ((Revenue_v1 - Revenue_v2) / Revenue_v2) * 100;
                                            stock_Financial_Resultslistdata[0].ProfitDifference = ((Revenue_v1 - Revenue_v2));
                                            //stock_Financial_Resultslistdata[1].Profit_Increase = ((Revenue_v2 - Revenue_v3) / Revenue_v3) * 100;
                                        }

                                        if (k == 2)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[0].EPS = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[1].EPS = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[2].EPS = Revenue_v3;

                                            if (Revenue_v2 > 0)
                                                stock_Financial_Resultslistdata[0].EPS_INcrease = ((Revenue_v1 - Revenue_v2) / Revenue_v2) * 100;
                                            stock_Financial_Resultslistdata[0].EPSDifference = ((Revenue_v1 - Revenue_v2));
                                            //stock_Financial_Resultslistdata[1].EPS_INcrease = ((Revenue_v2 - Revenue_v3) / Revenue_v3) * 100;
                                        }



                                        if (k == 3)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[0].Cash_EPS = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[1].Cash_EPS = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[2].Cash_EPS = Revenue_v3;
                                        }
                                        if (k == 4)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[0].OPM_Percentage = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[1].OPM_Percentage = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[2].OPM_Percentage = Revenue_v3;
                                        }
                                        if (k == 5)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[0].NPM_Percentage = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[1].NPM_Percentage = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[2].NPM_Percentage = Revenue_v3;
                                        }
                                        k = k + 1;
                                    }
                                }

                                {

                                    int k = 0;
                                    foreach (var res_item in Resp_obj.resultinM)
                                    {
                                        if (k == 0)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].Revenue = Revenue_v1;
                                            stock_Financial_Resultslistdata[3].CurrencyIn = "Millions";



                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].Revenue = Revenue_v2;
                                            stock_Financial_Resultslistdata[4].CurrencyIn = "Millions";


                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].Revenue = Revenue_v3;
                                            stock_Financial_Resultslistdata[5].CurrencyIn = "Millions";
                                            if (Revenue_v2 > 0)
                                                stock_Financial_Resultslistdata[3].RevenueIncrease = ((Revenue_v1 - Revenue_v2) / Revenue_v2) * 100;

                                            stock_Financial_Resultslistdata[3].RevenueDifference = ((Revenue_v1 - Revenue_v2));

                                        }
                                        if (k == 1)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].NET_PROFIT = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].NET_PROFIT = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].NET_PROFIT = Revenue_v3;

                                            if (Revenue_v2 > 0)
                                                stock_Financial_Resultslistdata[3].Profit_Increase = ((Revenue_v1 - Revenue_v2) / Revenue_v2) * 100;
                                            stock_Financial_Resultslistdata[3].ProfitDifference = ((Revenue_v1 - Revenue_v2));
                                        }

                                        if (k == 2)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].EPS = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].EPS = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].EPS = Revenue_v3;
                                            if (Revenue_v2 > 0)
                                                stock_Financial_Resultslistdata[3].EPS_INcrease = ((Revenue_v1 - Revenue_v2) / Revenue_v2) * 100;
                                            stock_Financial_Resultslistdata[3].EPSDifference = ((Revenue_v1 - Revenue_v2));
                                            // stock_Financial_Resultslistdata[4].EPS = ((Revenue_v2 - Revenue_v3) / Revenue_v3) * 100;
                                        }



                                        if (k == 3)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].Cash_EPS = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].Cash_EPS = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].Cash_EPS = Revenue_v3;
                                        }
                                        if (k == 4)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].OPM_Percentage = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].OPM_Percentage = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].OPM_Percentage = Revenue_v3;
                                        }
                                        if (k == 5)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].NPM_Percentage = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].NPM_Percentage = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].NPM_Percentage = Revenue_v3;
                                        }
                                        if (k == 6)
                                        {
                                            decimal.TryParse(res_item.v1, out var Revenue_v1);
                                            stock_Financial_Resultslistdata[3].PE_Ratio = Revenue_v1;

                                            decimal.TryParse(res_item.v2, out var Revenue_v2);
                                            stock_Financial_Resultslistdata[4].PE_Ratio = Revenue_v2;

                                            decimal.TryParse(res_item.v3, out var Revenue_v3);
                                            stock_Financial_Resultslistdata[5].PE_Ratio = Revenue_v3;
                                        }
                                        k = k + 1;
                                    }
                                }

                                var resultss = stock_Financial_Resultslistdata.Where(x => !Stock_Financial_Results_obj.Any(y => y.QuarterEnd.ToString() == x.QuarterEnd.ToString()));
                                if (resultss.Any())
                                {
                                    resultss.ToList().ForEach(x => x.UPDATED_ON = DateTime.Now);
                                    db.Stock_Financial_Results.AddRange(resultss);

                                    item.IsLatestQuaterUpdated = Resp_obj.col2.ToLower().ToString() == quatername.ToLower().ToString() ? true : false;
                                    item.FinancialUpdatedOn = DateTime.Now;

                                    db.SaveChanges();
                                }
                            }




                        }
                        catch (Exception ex)
                        {

                            stock_Financial_Resultslistdata_failed.Add(new Stock_Financial_Results { Symbol = item.Symbol });
                        }

                        //    )  "" )



                    }


                }
            }
            catch (Exception ex)
            {

            }
        }


        public static async Task getInitStocksFromSECID()
        {




            using (var db = new STOCKContext())
            {
                Console.WriteLine("Database Connected");
                Console.WriteLine();
                Console.WriteLine("Listing Category Sales For 1997s");
                var equites = db.Equitys.ToList().Where(x => x.MSN_SECID != null).Where(x => Convert.ToDateTime(x.UpdatedOn) != Convert.ToDateTime(DateTime.Now));


                int count = 0;
                foreach (var equity in equites)
                {
                    count++;
                    try
                    {


                        //System.Threading.Thread.Sleep(1000);
                        try
                        {
                            var stockresult = await getstockDetails(equity.MSN_SECID);
                            string PreviousRecommondation = equity.Recommondations;
                            string CurrentRecommondation = string.Empty;
                            if (stockresult.equity != null)
                            {
                                equity.Recommondations = stockresult.equity.analysis.estimate.recommendation ?? null;
                                CurrentRecommondation = equity.Recommondations;
                                equity.JsonData = JsonConvert.SerializeObject(stockresult);
                                Console.WriteLine(equity.MSN_SECID);
                            }
                            if (PreviousRecommondation != CurrentRecommondation)
                            {
                                bool isFavoriteAdded = PreviousRecommondation.Contains("buy");
                                bool isFavoriteRemoved = !isFavoriteAdded;

                                SqlParameter MSN_SECID = new SqlParameter();

                                MSN_SECID.ParameterName = "@MSN_SECID";
                                MSN_SECID.Value = equity.MSN_SECID;
                                MSN_SECID.DbType = System.Data.DbType.String;

                                SqlParameter FavoriteAdded = new SqlParameter();
                                FavoriteAdded.ParameterName = "@IsFavoriteAdded";
                                FavoriteAdded.Value = isFavoriteAdded;
                                FavoriteAdded.DbType = System.Data.DbType.Boolean;

                                SqlParameter FavoriteRemoved = new SqlParameter();
                                FavoriteRemoved.ParameterName = "@IsFavoriteRemoved";
                                FavoriteRemoved.Value = isFavoriteRemoved;
                                FavoriteRemoved.DbType = System.Data.DbType.Boolean;


                                db.Database.ExecuteSqlRaw("InsertOrUpdateMSN_Notification {0}, {1}, {2} ", MSN_SECID, FavoriteAdded, FavoriteRemoved);



                            }
                            // System.Threading.Thread.Sleep(1000);
                        }
                        catch (Exception)
                        {
                            System.Threading.Thread.Sleep(1000);

                        }


                    }
                    catch (Exception ex)
                    {

                        var result = await HttpHelper.Get<StockQuery>("https://services.bingapis.com/", string.Format("contentservices-finance.csautosuggest/api/v1/Query?query={0}&market=nse&count=1", equity.SecurityName.Replace("Limited", "").Replace("LTD.", "")));
                        var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
                        var stockresult = await getstockDetails(findresult.SecId);
                        equity.Recommondations = stockresult.equity.analysis.estimate.recommendation ?? "Null";
                    }
                    equity.UpdatedOn = DateTime.Now;
                    db.Entry(equity).State = EntityState.Modified;
                    db.SaveChanges();


                }
            }
        }

        public static async Task<MainResposne> getstockDetails(string sec)
        {
            //System.Threading.Thread.Sleep(1000);
            var url = "https://assets.msn.com/service/Finance/QuoteSummary?apikey=0QfOX3Vn51YCzitbLaRkTTBadtWpgTN8NZLW0C1SEM&activityId=864F7198-1833-446B-9102-18CD2BC73B98&ocid=finance-utils-peregrine&cm=en-in&it=edgeid&ids={0}&intents=Quotes,Exchanges,QuoteDetails&wrapodata=false";
            var result = await HttpHelper.Get<List<MainResposne>>(string.Format(url, sec), "");
            return result.FirstOrDefault();
            // var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
        }

        internal static Task InsertFromMicrosoft()
        {
            Console.WriteLine("InsertFromMicrosoft");
            using (var db = new STOCKContext())
            {
                db.Database.ExecuteSqlRaw("InsertFromMicrosoft");
            }
            return Task.CompletedTask;
        }

        internal static Task InsertMMSCompanies()
        {
            Console.WriteLine("INSERT_MMSN_Companies");
            using (var db = new STOCKContext())
            {
                db.Database.SetCommandTimeout(18000);
                db.Database.ExecuteSqlRaw("INSERT_MMSN_Companies");
            }
            return Task.CompletedTask;
        }

        internal static Task UpdownStcoks()
        {
            Console.WriteLine("MSNUpStocksSP");
            using (var db = new STOCKContext())
            {
                db.Database.ExecuteSqlRaw("MSNUpStocksSP");
                db.Database.ExecuteSqlRaw("MSNDownStocksSP");
            }
            return Task.CompletedTask;
        }
    }
}
