
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSNStocks.Models;
using MSNStocks.Query;
using MSNStocks.Result;
using MSNStocks.WebApp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MSNStocks
{
    internal static class API_MSN_Library
    {


        public static async Task getInitStocks()
        {




            using (var db = new STOCKContext())
            {
                Console.WriteLine("Database Connected");
                Console.WriteLine();
                Console.WriteLine("Listing Category Sales For 1997s");
                var equites = db.Equitys.ToList().Where(x => x.MSN_SECID == null);



                foreach (var equity in equites)
                {

                    try
                    {
                        var result = await HttpHelper.Get<StockQuery>("https://services.bingapis.com/", string.Format("contentservices-finance.csautosuggest/api/v1/Query?query={0}&market=BSE&count=1", equity.SecurityCode));
                        if (result.data.stocks.Count > 0)
                        {



                            var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
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


                }
            }
        }


        public static async Task getInitStocksFromSECID()
        {




            using (var db = new STOCKContext())
            {
                Console.WriteLine("Database Connected");
                Console.WriteLine();
                Console.WriteLine("Listing Category Sales For 1997s");
                var equites = db.Equitys.ToList().Where(x => x.MSN_SECID != null).Where(x=>Convert.ToDateTime(x.UpdatedOn)!=Convert.ToDateTime(DateTime.Now));


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
                                CurrentRecommondation= equity.Recommondations;
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
