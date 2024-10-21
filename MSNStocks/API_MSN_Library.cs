
using AngelBroking;
using BSE_Financilas;
using ICICIVolumeBreakouts.Models;
using ICICIVolumeBreakouts.Models.Histrry;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSNStocks.Models;
using MSNStocks.Models.Deals;
using MSNStocks.Models.Insights;
using MSNStocks.Models.NiftyTrader;
using MSNStocks.Models.results;
using MSNStocks.Models.xml;
using MSNStocks.Models.xml2;
using MSNStocks.Models.xml3;
using MSNStocks.Models.xml4;
using MSNStocks.Models.xml5;
using MSNStocks.Models.xml6;
using MSNStocks.Models.xml7;
using MSNStocks.MSN;
using MSNStocks.NDTV.Latest;
using MSNStocks.Query;
using MSNStocks.Result;
using MSNStocks.WebApp;
using Newtonsoft.Json;
using NIFTYTraders.Models;
using NSEBlockDeals.HIGH;
using RestSharp;
using Skender.Stock.Indicators;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MSNStocks
{
    public static class API_MSN_Library
    {
        public static List<Equities> GetJsonFileHistryData(string Symvbol)
        {

            if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol)))
            {
                try
                {
                    var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                    return JsonConvert.DeserializeObject<List<Equities>>(text).OrderByDescending(x => Convert.ToDateTime(x.ltt)).Take(60).OrderBy(x => Convert.ToDateTime(x.ltt)).ToList();

                    //var sortlist = data.Select(x => x.open.Value).ToList();
                    //sortlist.Add(Last);
                    //return sortlist;



                    //Arraysortlist = sortlist.Select(x => Convert.ToDecimal(x)).ToList();
                    //var nearest = sortlist.OrderBy(x => Math.Abs((long)x - Last)).First();

                    //var index = sortlist.ToArray().IndexOf(nearest);

                    //return data.Count() - index;
                    //return index;
                }
                catch (Exception)
                {

                    return new List<Equities>();
                }
            }
            else
            {
                return new List<Equities>();
            }
        }

        public static async Task GetEquitiesStats()
        {
            try
            {
                using (var db = new STOCKContext())
                {
                    Console.WriteLine("Database Connected");
                    Console.WriteLine();
                    Console.WriteLine("Listing Category Sales For 1997s");
                    var equites = db.Equitys.ToList();
                    db.Equities_Stats.FromSqlRaw("truncate table Equities_Stats");
                    var TickerStocksHistries = db.TickerStocksHistries.ToList().Where(x => x.Ltt > DateTime.Now.AddDays(-45)).ToList();

                    var Equities_Ratings_list = db.Equities_Ratings.ToList();

                    foreach (var equity in equites)
                    {
                        Console.WriteLine(equity.SecurityName);
                        try
                        {
                            bool Isnew = false;
                            var Equities_Ratings = Equities_Ratings_list.FirstOrDefault(x => x.Symbol == equity.Symbol);
                            if (Equities_Ratings == null)
                            {
                                Equities_Ratings = new Equities_Ratings();
                                Isnew = true;
                            }

                            var filteredtxt = ExecuteCommandBatforStatas(equity.SecurityId).ToString();
                            var result = JsonConvert.DeserializeObject<Equities_Ratings>(filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString());
                            if (Isnew)
                            {
                                db.Equities_Ratings.Add(Equities_Ratings);
                            }
                            Equities_Ratings.FFI_INCREASE = 0;
                            var data = (result.Postive.Contains("FII") || result.Negative.Contains("FII")) ? result.Postive.Split("to") : null;
                            if (data != null && data.Count() > 1)
                            {
                                var FFIFrom = Convert.ToDecimal(data[0].Substring(data[0].Length - 6).Replace("%", string.Empty));
                                var FFIto = Convert.ToDecimal(data[1].Substring(0, 6).Replace("%", string.Empty));
                                Equities_Ratings.FFI_INCREASE = FFIto - FFIFrom;
                            }
                            Equities_Ratings.MF_Increase = 0;
                            var data_MU = (result.Postive.Contains("Mutual Funds") || result.Negative.Contains("Mutual Funds")) ? result.Postive.Split("to") : null;
                            if (data_MU != null && data_MU.Count() > 1)
                            {
                                var FFIFrom = Convert.ToDecimal(data_MU[0].Substring(data_MU[0].Length - 6).Replace("%", string.Empty));
                                var FFIto = Convert.ToDecimal(data_MU[1].Substring(0, 6).Replace("%", string.Empty));
                                Equities_Ratings.MF_Increase = FFIto - FFIFrom;
                            }

                            Equities_Ratings.MACD = result.MACD ?? 0;
                            Equities_Ratings.ROC125 = result.ROC125 ?? 0;
                            Equities_Ratings.ATR = result.ATR ?? 0;
                            Equities_Ratings.FS1 = result.FS1 ?? 0;
                            Equities_Ratings.FS2 = result.FS2 ?? 0;
                            Equities_Ratings.FS3 = result.FS3 ?? 0;
                            Equities_Ratings.FR1 = result.FS1 ?? 0;
                            Equities_Ratings.FR2 = result.FR2 ?? 0;
                            Equities_Ratings.FR3 = result.FR3 ?? 0;
                            Equities_Ratings.ADX = result.ADX ?? 0;
                            Equities_Ratings.ROC21 = result.ROC21 ?? 0;
                            Equities_Ratings.MCADSIG = result.MCADSIG ?? 0;
                            Equities_Ratings.RSI = result.RSI ?? 0; ;
                            Equities_Ratings.MFI = result.MFI ?? 0;
                            Equities_Ratings.Opportunity = result.Opportunity ?? 0;
                            Equities_Ratings.PIVOT = result.PIVOT ?? 0;
                            Equities_Ratings.Strengths = result.Strengths ?? 0;
                            Equities_Ratings.Weakness = result.Weakness ?? 0;
                            Equities_Ratings.Threats = result.Threats ?? 0;
                            Equities_Ratings.SecuirtyId = equity.SecurityId ?? "";
                            Equities_Ratings.Symbol = equity.Symbol ?? "";
                            Equities_Ratings.Postive = result.Postive ?? "";
                            Equities_Ratings.Negative = result.Negative ?? "";
                            Equities_Ratings.Williams = result.Williams;
                            Equities_Ratings.FII = result.Postive ?? "";
                            Equities_Ratings.MutaulFOunds = result.Negative ?? "";
                            Equities_Ratings.companyName = result.companyName;
                            Equities_Ratings.Updated_on = DateTime.Now;

                        }
                        catch (Exception)
                        {

                            continue;
                        }


                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {


            }
        }


        #region Volume

        private static async Task<string> Getdetails(string token, string APIKEY)
        {
            string hostname = "https://api.icicidirect.com/breezeapi/api/v1/";
            string endpoint = "customerdetails";
            string url = $"{hostname}{endpoint}";


            HttpClient client = new HttpClient();

            string jsonPayload = @"{
            ""SessionToken"": ""_token_"",
            ""AppKey"": ""_APIKEY_""
        }";

            StringContent content = new StringContent(jsonPayload.Replace("_token_", token).Replace("_APIKEY_", APIKEY), Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            //request.Headers.Add("Content-Type", "application/json");
            request.Content = content;

            Console.WriteLine("Request headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseBody);
            //return responseBody;
        }
        public static double ChangeBetweenPercentage(double v1, double v2)
        {
            var data = ((v1 - v2) / ((v1 + v2) / 2)) * 100;
            return data;
        }
        public static double PriceIncrease(double v1, double v2)
        {
            return ((v1 - v2));
        }

        public static double VolumneincreaseInCrores(double v1, double v2)
        {
            return (v1 - v2);
        }

        #endregion

        public static async Task RunEquitiesVolumneBSE()
        {
            try
            {
                using (var db = new STOCKContext())
                {
                    Console.WriteLine("Database Connected");
                    Console.WriteLine();
                    Console.WriteLine("Listing Category Sales For 1997s");
                    var equites = db.Equitys.ToList();
                    //  db.Equities_Stats.FromSqlRaw("truncate table Equities_Stats");
                    //  var TickerStocksHistries = db.TickerStocksHistries.ToList().Where(x => x.Ltt > DateTime.Now.AddDays(-45)).ToList();

                    var Equities_Ratings_list = db.Equities_Volume_Stats.ToList();

                    string APIKEY = string.Empty;
                    string APISecret = string.Empty;
                    string token = string.Empty;
                    var url = "http://localhost:99/breezeOperation";
                    var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    string HUbUrl = url;


                    string[] line;
                    string arg = "0";


                    switch (0)
                    {
                        case 0:
                            line = lines[0].ToString().Split(',');
                            APIKEY = line[0];
                            APISecret = line[1];
                            token = line[2];
                            HUbUrl = "http://localhost:8080/BreezeOperation";
                            break;
                    }
                    var bodyresponse = JsonConvert.DeserializeObject<Customer>(Getdetails(token, APIKEY).Result);

                    foreach (var equity in equites)
                    {
                        Console.WriteLine(equity.SecurityName);
                        try
                        {


                            #region working


                            // App related Secret Key
                            string secretKey = APISecret;

                            // Payload as JSON
                            var payload = System.Text.Json.JsonSerializer.Serialize(new
                            {
                                AppKey = APIKEY,
                                SessionToken = token
                            });

                            // Time stamp generation for request-headers
                            //string timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
                            string timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
                            Console.WriteLine(timeStamp);

                            // Checksum generation for request-headers
                            string checksumData = timeStamp + payload + secretKey;
                            using var sha256 = SHA256.Create();
                            byte[] checksumBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(checksumData));
                            string checksum = BitConverter.ToString(checksumBytes).Replace("-", "");
                            Console.WriteLine(checksum);



                            using var client = new HttpClient();

                            var queryString = "?stock_code=_stock_code_&exch_code=BSE&from_date=_FromDate_T00:00:00.000Z&to_date=_ToDate_T20:00:00.000Z&interval=1day&product_type=cash&expiry_date=&right=&strike_price=0";

                            queryString = queryString.Replace("_stock_code_", equity.IssuerName).Replace("_FromDate_", DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString()).Replace("_ToDate_", DateTime.Now.Date.ToString("yyyy-MM-dd").ToString());

                            //queryString = queryString.Replace("_stock_code_", "FLEINT").Replace("_FromDate_", "2024-08-12".ToString()).Replace("_ToDate_", "2024-08-14");


                            var request = new HttpRequestMessage
                            {
                                Method = HttpMethod.Get,
                                RequestUri = new Uri("https://breezeapi.icicidirect.com/api/v2/historicalcharts" + queryString),
                                Headers = { { "X-SessionToken", bodyresponse.Success.session_token.ToString() }, { "apikey", APIKEY } },
                                Content = null
                            };

                            var response = client.SendAsync(request).Result;
                            response.EnsureSuccessStatusCode();

                            var responseBody = await response.Content.ReadAsStringAsync();
                            var results = JsonConvert.DeserializeObject<Histry>(responseBody);
                            bool Isnew = false;
                            var Equities_Ratings = Equities_Ratings_list.FirstOrDefault(x => x.Symbol == equity.Symbol && Convert.ToDateTime(x.Updated).Date == DateTime.Now.Date);
                            if (Equities_Ratings == null)
                            {
                                Equities_Ratings = new Equities_Volume_Stats();
                                Isnew = true;
                            }
                            if (results.Success.Count() > 0)
                            {
                                var result_volue = results.Success.OrderByDescending(x => x.datetime).ToList().Take(2).OrderBy(x => x.datetime).ToList();


                                // var results = breeze.getHistoricalData(interval: "1day", fromDate: DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString(), toDate: DateTime.Now.Date.ToString("yyyy-MM-dd").ToString(), stockCode: "BHEL", exchangeCode: "NSE", productType: "cash", "", "", "");


                                //  List<HistoricalData> myDeserializedClass = JsonConvert.DeserializeObject<List<Histry>>(result.Success);
                                var First = result_volue[0];
                                var second = result_volue[1];
                                //var thrird = result[2];
                                //var Four = result[3];
                                //var fifth = result[4];

                                var firsdaychange = ChangeBetweenPercentage(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
                                var firsdaychangeVolume = ChangeBetweenPercentage(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
                                //var secondaychange = ChangeBetweenPercentage(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
                                //var thirdchange = ChangeBetweenPercentage(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
                                //var fourthchange = ChangeBetweenPercentage(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


                                var firsdaychange_INC = PriceIncrease(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
                                // var secondaychange_INC = PriceIncrease(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
                                //var thirdchange_INC = PriceIncrease(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
                                //var fourthchange_INC = PriceIncrease(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


                                var firsdaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
                                //var secondaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(thrird.volume), Convert.ToDouble(First.volume));
                                //var thirdchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(Four.volume), Convert.ToDouble(First.volume));
                                //var fourthchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(fifth.volume), Convert.ToDouble(First.volume));

                                //from var element in myDeserializedClass.GetNth(10) select element;
                                //var resulss= from var element in myDeserializedClass.get(10) select element;



                                #endregion




                                // var filteredtxt = ExecuteCommandBatforStatas(equity.SecurityId).ToString();
                                // var result = JsonConvert.DeserializeObject<Equities_Ratings>(filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString());
                                if (Isnew)
                                {
                                    Equities_Ratings.Symbol = equity.Symbol;
                                    db.Equities_Volume_Stats.Add(Equities_Ratings);
                                }
                                Equities_Ratings.VolumeUPBy_Percentage = (decimal?)firsdaychangeVolume;
                                Equities_Ratings.Yesterday_volume = result_volue[0].volume;
                                Equities_Ratings.Todays_volume = result_volue[1].volume;
                                Equities_Ratings.LTP = (decimal)result_volue[1].close;
                                Equities_Ratings.Stock_name = result_volue[1].stock_code;


                                Equities_Ratings.Updated = Convert.ToDateTime(results.Success.OrderByDescending(x => x.datetime).FirstOrDefault().datetime);
                                db.SaveChanges();
                                Thread.Sleep(20);
                            }
                            else
                            {
                                if (Isnew)
                                {
                                    Equities_Ratings.Symbol = equity.Symbol;
                                    db.Equities_Volume_Stats.Add(Equities_Ratings);
                                    Equities_Ratings.Updated = Convert.ToDateTime(DateTime.Now);
                                }
                                db.SaveChanges();
                            }

                        }
                        catch (Exception ex)
                        {
                            continue;

                        }


                    }

                }
            }
            catch (Exception ex)
            {


            }
        }


        public static async Task RunEquitiesVolumne()
        {

            string Client_code = "A60479487";  //YOUR CLIENT CODE
            string Password = "2711"; //YOUR PASSWORD
            string api_key = "E8kj0Y88";
            string JWTToken = "";  // optional
            string RefreshToken = ""; // optional

            var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\angel.txt");
            //string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            OutputBaseClass obj = new OutputBaseClass();
            var tokenresults = JsonConvert.DeserializeObject<AngelToken>(text);



            SmartApi connect = new SmartApi(api_key, tokenresults.jwtToken, tokenresults.refreshToken);



            ////Login by client code and password
            //obj = connect.GenerateSession(Client_code, Password, text);
            //AngelToken agr = obj.TokenResponse;

            //Console.WriteLine("------GenerateSession call output-------------");
            //Console.WriteLine(JsonConvert.SerializeObject(agr));
            //Console.WriteLine("----------------------------------------------");

            ////Get Token
            //obj = connect.GenerateToken();
            //agr = obj.TokenResponse;

            //Console.WriteLine("------GenerateToken call output-------------");
            //Console.WriteLine(JsonConvert.SerializeObject(agr));
            //Console.WriteLine("----------------------------------------------");



            //Get Profile
            //obj = connect.GetProfile();
            //GetProfileResponse gp = obj.GetProfileResponse;

            //Console.WriteLine("------GetProfile call output-------------");
            //Console.WriteLine(JsonConvert.SerializeObject(gp));
            //Console.WriteLine("----------------------------------------------");


            try
            {
                using (var db = new STOCKContext())
                {
                    Console.WriteLine("Database Connected");
                    Console.WriteLine();
                    Console.WriteLine("Listing Category Sales For 1997s");
                    var equites = db.Equitys.ToList();
                    db.Database.ExecuteSqlRaw("truncate table AOEquities;truncate table AO_Depth");
                    //  db.Equities_Stats.FromSqlRaw("truncate table Equities_Stats");
                    //  var TickerStocksHistries = db.TickerStocksHistries.ToList().Where(x => x.Ltt > DateTime.Now.AddDays(-45)).ToList();

                    var Equities_Ratings_list = db.Equities_Volume_Stats.ToList();

                    string APIKEY = string.Empty;
                    string APISecret = string.Empty;
                    string token = string.Empty;
                    var url = "http://localhost:99/breezeOperation";



                    //switch (0)
                    //{
                    //    case 0:
                    //        line = lines[0].ToString().Split(',');
                    //        APIKEY = line[0];
                    //        APISecret = line[1];
                    //        token = line[2];
                    //        HUbUrl = "http://localhost:8080/BreezeOperation";
                    //        break;
                    //}
                    var bodyresponse = JsonConvert.DeserializeObject<Customer>(Getdetails(token, APIKEY).Result);

                    foreach (var equity in equites)
                    {

                        try
                        {


                            #region working



                            var input = new CandleRequest()
                            {
                                exchange = "BSE",
                                fromdate = DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd 09:00"),
                                todate = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd 16:00"),
                                interval = "ONE_DAY",
                                symboltoken = equity.Symbol.Replace("1.1!", "")

                            };

                            var Quotes = connect.GetQuoteData(new QuoteData()
                            {
                                exchangeTokens = new ExchangeTokens()
                                {
                                    BSE = new List<string>() { equity.Symbol.Replace("1.1!", "") },
                                    NSE = new List<string>() { equity.Isinno != null ? equity.Isinno.Replace("1.1!", "") : "" },
                                },
                                mode = "FULL"

                            });
                            int i = 0;
                            if (Quotes.status && Quotes.data.fetched.Count > 1)
                            {
                                Console.WriteLine(equity.SecurityName);
                                var datas_result = Quotes.data.fetched.Where(x => x.depth.buy.FirstOrDefault().quantity > 0).ToList();


                                if (datas_result.Any())
                                {

                                    AOEquities ao = new AOEquities();
                                    ao.ltp = Convert.ToDecimal(datas_result.FirstOrDefault().ltp);
                                    ao.tradeVolume = Convert.ToDecimal(datas_result.FirstOrDefault().tradeVolume);
                                    ao.tradingSymbol = Convert.ToString(datas_result.FirstOrDefault().tradingSymbol);
                                    ao.NSECODE = equity.IssuerName;
                                    ao.opnInterest = Convert.ToDecimal(datas_result.FirstOrDefault().opnInterest);
                                    ao.lastTradeQty = Convert.ToDecimal(datas_result.FirstOrDefault().lastTradeQty);
                                    ao.avgPrice = Convert.ToDecimal(datas_result.FirstOrDefault().avgPrice);
                                    ao.open = Convert.ToDecimal(datas_result.FirstOrDefault().open);
                                    ao.low = Convert.ToDecimal(datas_result.FirstOrDefault().low);
                                    ao.high = Convert.ToDecimal(datas_result.FirstOrDefault().high);
                                    ao.close = Convert.ToDecimal(datas_result.FirstOrDefault().close);
                                    ao.lowerCircuit = Convert.ToDecimal(datas_result.FirstOrDefault().lowerCircuit);
                                    ao.upperCircuit = Convert.ToDecimal(datas_result.FirstOrDefault().upperCircuit);

                                    ao.netChange = Convert.ToDecimal(datas_result.FirstOrDefault().netChange);
                                    ao.percentChange = Convert.ToDecimal(datas_result.FirstOrDefault().percentChange);

                                    ao.symbolToken = Convert.ToString(datas_result.FirstOrDefault().symbolToken);
                                    ao.symbol = equity.Symbol;
                                    ao.WeekHigh52 = Convert.ToDecimal(datas_result.FirstOrDefault()._52WeekHigh);
                                    ao.WeekLow52 = Convert.ToDecimal(datas_result.FirstOrDefault()._52WeekLow);

                                    ao.exchange = datas_result.FirstOrDefault().exchange;
                                    ao.exchFeedTime = Convert.ToDateTime(datas_result.FirstOrDefault().exchFeedTime);
                                    ao.exchTradeTime = Convert.ToDateTime(datas_result.FirstOrDefault().exchTradeTime);
                                    ao.totBuyQuan = Convert.ToDecimal(datas_result.FirstOrDefault().totBuyQuan);
                                    ao.totSellQuan = Convert.ToDecimal(datas_result.FirstOrDefault().totSellQuan);
                                    db.AOEquities.Add(ao);
                                    int OrderID = 1;
                                    foreach (var depth in datas_result.FirstOrDefault().depth.buy)
                                    {
                                        AO_Depth aO_Depth = new AO_Depth();
                                        aO_Depth.price = Convert.ToDecimal(depth.price);
                                        aO_Depth.orders = Convert.ToInt64(depth.orders);
                                        aO_Depth.quantity = Convert.ToInt64(depth.quantity);
                                        aO_Depth.symbolToken = Convert.ToString(datas_result.FirstOrDefault().symbolToken);
                                        aO_Depth.Type = "BUY";
                                        aO_Depth.OrderID = OrderID;
                                        OrderID = OrderID + 1;
                                        db.AO_Depth.Add(aO_Depth);
                                    }
                                    OrderID = 1;
                                    foreach (var depth in datas_result.FirstOrDefault().depth.sell)
                                    {
                                        AO_Depth aO_Depth = new AO_Depth();
                                        aO_Depth.price = Convert.ToDecimal(depth.price);
                                        aO_Depth.orders = Convert.ToInt64(depth.orders);
                                        aO_Depth.quantity = Convert.ToInt64(depth.quantity);
                                        aO_Depth.symbolToken = Convert.ToString(datas_result.FirstOrDefault().symbolToken);
                                        aO_Depth.Type = "SELL";
                                        aO_Depth.OrderID = OrderID;
                                        OrderID = OrderID + 1;
                                        db.AO_Depth.Add(aO_Depth);
                                    }

                                    // db.SaveChanges();
                                }
                            }


                            var data = connect.GetCandleData(input);
                            var results = data.GetCandleDataResponse;

                            bool Isnew = false;

                            var Equities_Ratings = Equities_Ratings_list.FirstOrDefault(x => x.Symbol == equity.Symbol && Convert.ToDateTime(x.Updated).Date == DateTime.Now.Date);

                            if (Equities_Ratings == null)
                            {
                                Equities_Ratings = new Equities_Volume_Stats();
                                Isnew = true;
                            }
                            if (results == null)
                            {
                                Thread.Sleep(10);
                                data = connect.GetCandleData(input);
                                results = data.GetCandleDataResponse;
                            }
                            if (results == null)
                            {
                                Thread.Sleep(150);
                                data = connect.GetCandleData(input);
                                results = data.GetCandleDataResponse;
                            }

                            if (results == null)
                            {
                                Thread.Sleep(200);
                                data = connect.GetCandleData(input);
                                results = data.GetCandleDataResponse;
                            }
                            if (results == null)
                            {
                                Thread.Sleep(200);
                                data = connect.GetCandleData(input);
                                results = data.GetCandleDataResponse;
                            }
                            if (results.status & results.data.Any())
                            {
                                var result_volue = results.data.OrderByDescending(x => Convert.ToDateTime(x[0])).ToList().Take(2).OrderBy(x => Convert.ToDateTime(x[0])).ToList();


                                // var results = breeze.getHistoricalData(interval: "1day", fromDate: DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString(), toDate: DateTime.Now.Date.ToString("yyyy-MM-dd").ToString(), stockCode: "BHEL", exchangeCode: "NSE", productType: "cash", "", "", "");


                                //  List<HistoricalData> myDeserializedClass = JsonConvert.DeserializeObject<List<Histry>>(result.Success);
                                var First = result_volue[0];
                                var second = result_volue[1];
                                //var thrird = result[2];
                                //var Four = result[3];
                                //var fifth = result[4];

                                var firsdaychange = ChangeBetweenPercentage(Convert.ToDouble(second[4]), Convert.ToDouble(First[4]));
                                var firsdaychangeVolume = ChangeBetweenPercentage(Convert.ToDouble(second[5]), Convert.ToDouble(First[5]));

                                //var secondaychange = ChangeBetweenPercentage(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
                                //var thirdchange = ChangeBetweenPercentage(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
                                //var fourthchange = ChangeBetweenPercentage(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


                                var firsdaychange_INC = PriceIncrease(Convert.ToDouble(second[4]), Convert.ToDouble(First[4]));

                                // var secondaychange_INC = PriceIncrease(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
                                //var thirdchange_INC = PriceIncrease(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
                                //var fourthchange_INC = PriceIncrease(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


                                var firsdaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(second[5]), Convert.ToDouble(First[5]));


                                //var secondaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(thrird.volume), Convert.ToDouble(First.volume));
                                //var thirdchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(Four.volume), Convert.ToDouble(First.volume));
                                //var fourthchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(fifth.volume), Convert.ToDouble(First.volume));

                                //from var element in myDeserializedClass.GetNth(10) select element;
                                //var resulss= from var element in myDeserializedClass.get(10) select element;



                                #endregion




                                //var filteredtxt = ExecuteCommandBatforStatas(equity.SecurityId).ToString();
                                //var result = JsonConvert.DeserializeObject<Equities_Ratings>(filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString());

                                if (Isnew)
                                {
                                    Equities_Ratings.Symbol = equity.Symbol;
                                    db.Equities_Volume_Stats.Add(Equities_Ratings);
                                }
                                Equities_Ratings.VolumeUPBy_Percentage = (decimal?)firsdaychangeVolume;
                                Equities_Ratings.Yesterday_volume = Convert.ToDecimal(result_volue[0][5]);
                                Equities_Ratings.Todays_volume = Convert.ToDecimal(result_volue[1][5]);
                                Equities_Ratings.LTP = Convert.ToDecimal(result_volue.LastOrDefault()[4]);
                                Equities_Ratings.Stock_name = equity.IssuerName;


                                Equities_Ratings.Updated = Convert.ToDateTime(results.data.OrderByDescending(x => Convert.ToDateTime(x[0])).FirstOrDefault()[0]);
                                db.SaveChanges();
                                Thread.Sleep(50);
                            }
                            else
                            {
                                //if (Isnew)
                                //{
                                //    Equities_Ratings.Symbol = equity.Symbol;
                                //    db.Equities_Volume_Stats.Add(Equities_Ratings);
                                //    Equities_Ratings.Updated = Convert.ToDateTime(DateTime.Now);
                                //}
                                //db.SaveChanges();
                            }



                            //var responseBody = await response.Content.ReadAsStringAsync();
                            // var results = JsonConvert.DeserializeObject<Histry>(responseBody);
                            //var results = datatata;
                            //bool Isnew = false;
                            //var Equities_Ratings = Equities_Ratings_list.FirstOrDefault(x => x.Symbol == equity.Symbol && Convert.ToDateTime(x.Updated).Date == DateTime.Now.Date);
                            //if (Equities_Ratings == null)
                            //{
                            //    Equities_Ratings = new Equities_Volume_Stats();
                            //    Isnew = true;
                            //}
                            //if (results.status)
                            //{
                            //    var result_volue = results.data.OrderByDescending(x => x.datetime).ToList().Take(2).OrderBy(x => x.datetime).ToList();


                            //    // var results = breeze.getHistoricalData(interval: "1day", fromDate: DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString(), toDate: DateTime.Now.Date.ToString("yyyy-MM-dd").ToString(), stockCode: "BHEL", exchangeCode: "NSE", productType: "cash", "", "", "");


                            //    //  List<HistoricalData> myDeserializedClass = JsonConvert.DeserializeObject<List<Histry>>(result.Success);
                            //    var First = result_volue[0];
                            //    var second = result_volue[1];
                            //    //var thrird = result[2];
                            //    //var Four = result[3];
                            //    //var fifth = result[4];

                            //    var firsdaychange = ChangeBetweenPercentage(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
                            //    var firsdaychangeVolume = ChangeBetweenPercentage(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
                            //    //var secondaychange = ChangeBetweenPercentage(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
                            //    //var thirdchange = ChangeBetweenPercentage(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
                            //    //var fourthchange = ChangeBetweenPercentage(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


                            //    var firsdaychange_INC = PriceIncrease(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
                            //    // var secondaychange_INC = PriceIncrease(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
                            //    //var thirdchange_INC = PriceIncrease(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
                            //    //var fourthchange_INC = PriceIncrease(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


                            //    var firsdaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
                            //    //var secondaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(thrird.volume), Convert.ToDouble(First.volume));
                            //    //var thirdchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(Four.volume), Convert.ToDouble(First.volume));
                            //    //var fourthchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(fifth.volume), Convert.ToDouble(First.volume));

                            //    //from var element in myDeserializedClass.GetNth(10) select element;
                            //    //var resulss= from var element in myDeserializedClass.get(10) select element;



                            //    #endregion




                            //    // var filteredtxt = ExecuteCommandBatforStatas(equity.SecurityId).ToString();
                            //    // var result = JsonConvert.DeserializeObject<Equities_Ratings>(filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString());
                            //    if (Isnew)
                            //    {
                            //        Equities_Ratings.Symbol = equity.Symbol;
                            //        db.Equities_Volume_Stats.Add(Equities_Ratings);
                            //    }
                            //    Equities_Ratings.VolumeUPBy_Percentage = (decimal?)firsdaychangeVolume;
                            //    Equities_Ratings.Yesterday_volume = result_volue[0].volume;
                            //    Equities_Ratings.Todays_volume = result_volue[1].volume;
                            //    Equities_Ratings.LTP = (decimal)result_volue[1].close;
                            //    Equities_Ratings.Stock_name = result_volue[1].stock_code;


                            //    Equities_Ratings.Updated = Convert.ToDateTime(results.Success.OrderByDescending(x => x.datetime).FirstOrDefault().datetime);
                            //    db.SaveChanges();
                            //    Thread.Sleep(20);
                            //}
                            //else
                            //{
                            //    if (Isnew)
                            //    {
                            //        Equities_Ratings.Symbol = equity.Symbol;
                            //        db.Equities_Volume_Stats.Add(Equities_Ratings);
                            //        Equities_Ratings.Updated = Convert.ToDateTime(DateTime.Now);
                            //    }
                            //    db.SaveChanges();
                            //}

                        }
                        catch (Exception ex)
                        {
                            continue;

                        }


                    }

                }
            }
            catch (Exception ex)
            {


            }

        }

        private static object ExecuteCommandBatforStatas(string? securityId)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            //   string param = "https://nsearchives.nseindia.com/corporate/xbrl/NBFC_INDAS_104606_1102707_19042024080602.xml";
            startInfo.Arguments = @"/c C:\Hosts\Breeze\Equities_Stats.bat" + " " + securityId; ;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            //string errors = process.StandardError.ReadToEnd();
            return process.StandardOutput.ReadToEnd();
        }

        // public 
        public static async Task EquitiesStats()
        {




            using (var db = new STOCKContext())
            {
                Console.WriteLine("Database Connected");
                Console.WriteLine();
                Console.WriteLine("Listing Category Sales For 1997s");
                var equites = db.Equitys.ToList();
                db.Equities_Stats.FromSqlRaw("truncate table Equities_Stats");
                var TickerStocksHistries = db.TickerStocksHistries.ToList().Where(x => x.Ltt > DateTime.Now.AddDays(-45)).ToList();



                foreach (var equity in equites)
                {
                    List<Equities_Stats> stats = new List<Equities_Stats>();
                    try
                    {
                        Equities threedaydata = null;
                        Equities fivedaydata = null;
                        Equities sevdaydata = null;
                        Equities fifitendays = null;
                        Equities tendays = null;


                        var quotelist = GetJsonFileHistryData(equity.Symbol);
                        if (quotelist.Count == 0)
                        {
                            continue;
                        }
                        var quotelistasc = quotelist.OrderBy(x => x.ltt);
                        List<Skender.Stock.Indicators.Quote> quotesList = quotelist.Select(x => new Skender.Stock.Indicators.Quote
                        {
                            Close = Convert.ToDecimal(x.last),
                            Open = Convert.ToDecimal(x.open),
                            Date = Convert.ToDateTime(x.ltt),
                            High = Convert.ToDecimal(x.high),
                            Low = Convert.ToDecimal(x.low),
                            Volume = Convert.ToDecimal(x.ttv)
                        }).OrderBy(x => x.Date).ToList();


                        IEnumerable<MacdResult> macdresult = quotesList.GetMacd(12, 26, 9);
                        IEnumerable<WmaResult> wmaResults = quotesList.GetWma(5);
                        IEnumerable<VolatilityStopResult> Volatilityresults = quotesList.GetVolatilityStop(7, 3);
                        IEnumerable<RsiResult> rsiResults = quotesList.GetObv().GetRsi(7);

                        IEnumerable<SuperTrendResult> Strend = quotesList.GetSuperTrend(10, 3);
                        var candleResult = quotesList.GetMarubozu(90);

                        foreach (var itemx in quotelist.Skip(quotelist.Count - 30).Take(30))
                        {
                            try
                            {
                                var equities_Stats = new Equities_Stats();
                                equities_Stats.Symbol = itemx.symbol;
                                equities_Stats.StockCode = itemx.SecurityId;
                                equities_Stats.StockName = itemx.stock_name;
                                equities_Stats.Change = itemx.change;
                                equities_Stats.close = itemx.close;
                                equities_Stats.Open = itemx.open;
                                equities_Stats.Volume = Convert.ToDouble(itemx.ttv);
                                equities_Stats.LTT = Convert.ToDateTime(itemx.ltt);
                                equities_Stats.MACD = macdresult.Where(y => y.Date == Convert.ToDateTime(itemx.ltt)).Select(x => x.Macd.HasValue ? Convert.ToDouble(x.Macd) : 0).FirstOrDefault();
                                equities_Stats.RSI = rsiResults.Where(y => y.Date == Convert.ToDateTime(itemx.ltt)).Select(x => x.Rsi.HasValue ? Convert.ToDouble(x.Rsi) : 0).FirstOrDefault();
                                string? v = candleResult.Where(y => y.Date == Convert.ToDateTime(itemx.ltt)).Select(x => x.Match.ToString().Replace("Signal", "")).FirstOrDefault();
                                equities_Stats.Match = v;

                                threedaydata = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(itemx.ltt).AddDays(-2)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();

                                if (threedaydata != null)
                                {
                                    equities_Stats.ThreedaysChange = Math.Round(Convert.ToDouble(((itemx.open - threedaydata.open) / threedaydata.open) * 100), 2);
                                    equities_Stats.ThreedaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - threedaydata.open), 2);
                                }

                                if (threedaydata != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(threedaydata.ltt).AddDays(-2)))
                                {
                                    fivedaydata = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(threedaydata.ltt).AddDays(-2)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
                                    if (fivedaydata != null)
                                    {
                                        equities_Stats.FivedaysChange = Math.Round(Convert.ToDouble(((itemx.open - fivedaydata.open) / fivedaydata.open) * 100), 2);
                                        equities_Stats.FivedaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - fivedaydata.open), 2);
                                    }
                                }
                                if (fivedaydata != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(fivedaydata.ltt).AddDays(-2)))
                                {
                                    sevdaydata = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(fivedaydata.ltt).AddDays(-2)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
                                    if (sevdaydata != null)
                                    {
                                        equities_Stats.SevendaysChange = Math.Round(Convert.ToDouble(((itemx.open - sevdaydata.open) / sevdaydata.open) * 100), 2);
                                        equities_Stats.SevendaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - sevdaydata.open), 2);
                                    }
                                }
                                if (sevdaydata != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(sevdaydata.ltt).AddDays(-3)))
                                {
                                    tendays = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(sevdaydata.ltt).AddDays(-3)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
                                    if (tendays != null)
                                    {
                                        equities_Stats.TendaysChange = Math.Round(Convert.ToDouble(((itemx.open - tendays.open) / tendays.open) * 100), 2);
                                        equities_Stats.TendaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - tendays.open), 2);
                                    }
                                }
                                if (tendays != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(sevdaydata.ltt).AddDays(-4)))
                                {
                                    fifitendays = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(tendays.ltt).AddDays(-4)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
                                    if (fifitendays != null)
                                    {
                                        equities_Stats.FifteendaysChange = Math.Round(Convert.ToDouble(((itemx.open - fifitendays.open) / fifitendays.open) * 100), 2);
                                        equities_Stats.FifteendaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - fifitendays.open), 2);
                                    }
                                }

                                stats.Add(equities_Stats);


                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }

                        try
                        {
                            db.Equities_Stats.AddRange(stats);
                            //db.Entry(equity).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        //Console.WriteLine(equity.SecurityName);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    };
                    Console.WriteLine(equity.Id);

                }

            }
        }


        //public static async Task EquitiesStatsForLKT()
        //{




        //    using (var db = new STOCKContext())
        //    {
        //        Console.WriteLine("Database Connected");
        //        Console.WriteLine();
        //        Console.WriteLine("Listing Category Sales For 1997s");
        //        var equites = db.Equitys.ToList();
        //        db.Equities_Stats.FromSqlRaw("truncate table Equities_Stats");
        //        var TickerStocksHistries = db.TickerStocksHistries.ToList().Where(x => x.Ltt > DateTime.Now.AddDays(-45)).ToList();



        //        foreach (var equity in equites)
        //        {
        //            List<Equities_Stats> stats = new List<Equities_Stats>();
        //            try
        //            {
        //                Equities threedaydata = null;
        //                Equities fivedaydata = null;
        //                Equities sevdaydata = null;
        //                Equities fifitendays = null;
        //                Equities tendays = null;


        //                var quotelist = TickerStocksHistries.Where(x => x.Symbol == equity.Symbol).ToList().OrderByDescending(x => x.Ltt).Take(30).ToList();
        //                if (quotelist.Count == 0)
        //                {
        //                    continue;
        //                }


        //                foreach (var itemx in quotelist.Skip(quotelist.Count - 30).Take(7))
        //                {
        //                    try
        //                    {
        //                        var equities_Stats = new Equities_Stats();
        //                        equities_Stats.Symbol = itemx.Symbol;
        //                        equities_Stats.StockCode = equity.SecurityId;
        //                        equities_Stats.StockName = equity.SecurityName;
        //                        equities_Stats.Change = Convert.ToDouble(itemx.Change);
        //                        equities_Stats.close = Convert.ToDouble(itemx.Close);
        //                        equities_Stats.Open = itemx.open;
        //                        equities_Stats.Volume = Convert.ToDouble(itemx.ttv);
        //                        equities_Stats.LTT = Convert.ToDateTime(itemx.ltt);
        //                        equities_Stats.MACD = macdresult.Where(y => y.Date == Convert.ToDateTime(itemx.ltt)).Select(x => x.Macd.HasValue ? Convert.ToDouble(x.Macd) : 0).FirstOrDefault();
        //                        equities_Stats.RSI = rsiResults.Where(y => y.Date == Convert.ToDateTime(itemx.ltt)).Select(x => x.Rsi.HasValue ? Convert.ToDouble(x.Rsi) : 0).FirstOrDefault();
        //                        string? v = candleResult.Where(y => y.Date == Convert.ToDateTime(itemx.ltt)).Select(x => x.Match.ToString().Replace("Signal", "")).FirstOrDefault();
        //                        equities_Stats.Match = v;

        //                        threedaydata = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(itemx.ltt).AddDays(-2)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();

        //                        if (threedaydata != null)
        //                        {
        //                            equities_Stats.ThreedaysChange = Math.Round(Convert.ToDouble(((itemx.open - threedaydata.open) / threedaydata.open) * 100), 2);
        //                            equities_Stats.ThreedaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - threedaydata.open), 2);
        //                        }

        //                        if (threedaydata != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(threedaydata.ltt).AddDays(-2)))
        //                        {
        //                            fivedaydata = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(threedaydata.ltt).AddDays(-2)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
        //                            if (fivedaydata != null)
        //                            {
        //                                equities_Stats.FivedaysChange = Math.Round(Convert.ToDouble(((itemx.open - fivedaydata.open) / fivedaydata.open) * 100), 2);
        //                                equities_Stats.FivedaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - fivedaydata.open), 2);
        //                            }
        //                        }
        //                        if (fivedaydata != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(fivedaydata.ltt).AddDays(-2)))
        //                        {
        //                            sevdaydata = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(fivedaydata.ltt).AddDays(-2)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
        //                            if (sevdaydata != null)
        //                            {
        //                                equities_Stats.SevendaysChange = Math.Round(Convert.ToDouble(((itemx.open - sevdaydata.open) / sevdaydata.open) * 100), 2);
        //                                equities_Stats.SevendaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - sevdaydata.open), 2);
        //                            }
        //                        }
        //                        if (sevdaydata != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(sevdaydata.ltt).AddDays(-3)))
        //                        {
        //                            tendays = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(sevdaydata.ltt).AddDays(-3)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
        //                            if (tendays != null)
        //                            {
        //                                equities_Stats.TendaysChange = Math.Round(Convert.ToDouble(((itemx.open - tendays.open) / tendays.open) * 100), 2);
        //                                equities_Stats.TendaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - tendays.open), 2);
        //                            }
        //                        }
        //                        if (tendays != null && quotelist.Any(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(sevdaydata.ltt).AddDays(-4)))
        //                        {
        //                            fifitendays = quotelist.Where(z => Convert.ToDateTime(z.ltt) <= Convert.ToDateTime(tendays.ltt).AddDays(-4)).OrderByDescending(t => Convert.ToDateTime(t.ltt)).FirstOrDefault();
        //                            if (fifitendays != null)
        //                            {
        //                                equities_Stats.FifteendaysChange = Math.Round(Convert.ToDouble(((itemx.open - fifitendays.open) / fifitendays.open) * 100), 2);
        //                                equities_Stats.FifteendaysPriceChange = Math.Round(Convert.ToDouble(itemx.open - fifitendays.open), 2);
        //                            }
        //                        }

        //                        stats.Add(equities_Stats);


        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        throw;
        //                    }
        //                }

        //                try
        //                {
        //                    db.Equities_Stats.AddRange(stats);
        //                    //db.Entry(equity).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //                catch (Exception ex)
        //                {

        //                    throw;
        //                }

        //                //Console.WriteLine(equity.SecurityName);

        //            }
        //            catch (Exception ex)
        //            {

        //                throw;
        //            };
        //            Console.WriteLine(equity.Id);

        //        }

        //    }
        //}

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
                        var result = await HttpHelper.Get<StockQuery>("https://services.bingapis.com/", string.Format("contentservices-finance.csautosuggest/api/v1/Query?query={0}&market=BSE&count=1", equity.IssuerName));
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

                        try
                        {
                            var result = await HttpHelper.Get<StockQuery>("https://services.bingapis.com/", string.Format("contentservices-finance.csautosuggest/api/v1/Query?query={0}&market=nse&count=1", equity.SecurityName.Replace("Limited", "").Replace("LTD.", "")));
                            var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
                            var stockresult = await getstockDetails(findresult.SecId);
                            equity.Recommondations = stockresult.equity.analysis.estimate.recommendation ?? "Null";
                        }
                        catch (Exception)
                        {


                        }
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
                    var equites = db.Equitys.ToList()
                        // .Where(x=>x.Symbol== "1.1!500189")
                        //.Where(x=>x.FinancialUpdatedOn ==null).
                        .Where(x => x.IsLatestQuaterUpdated == null || x.IsLatestQuaterUpdated == false);
                    // Where(x => Convert.ToDateTime(x.UpdatedOn) != Convert.ToDateTime(DateTime.Now));
                    //db.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.[Stock_Financial_Results]");
                    foreach (var item in equites)
                    {
                        Console.WriteLine(item.Symbol);
                        Console.WriteLine(item.Id);
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

        static string ExecuteCommandNSExbrl(string filename, string param)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            //   string param = "https://nsearchives.nseindia.com/corporate/xbrl/NBFC_INDAS_104606_1102707_19042024080602.xml";
            startInfo.Arguments = @"/c C:\Hosts\Breeze\NSE_FIN_xbrl.bat" + " " + param; ;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            //string errors = process.StandardError.ReadToEnd();
            return process.StandardOutput.ReadToEnd();




        }
        static string ExecuteCommandNSEFirst(string filename)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            string param = "https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly";
            startInfo.Arguments = @"/c C:\Hosts\Breeze\NSE_FIN.bat" + " " + param; ;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();



        }

        static string ExecuteCommandNSEBlock(string filename, string Type)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            string param = "https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly";
            startInfo.Arguments = @"/c C:\Hosts\Breeze\NSE_Deal.bat" + " " + Type + " " + DateTime.Now.AddDays(-1).Date.ToString("dd-MM-yyyy") + " " + DateTime.Now.Date.ToString("dd-MM-yyyy"); ;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();



        }

        static string ExecuteCommandNSEWeek(string filename, string Type)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            string param = "https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly";
            startInfo.Arguments = @"/c C:\Hosts\Breeze\NSE_52weeks.bat" + " " + Type + " " + DateTime.Now.AddDays(-1).Date.ToString("dd-MM-yyyy") + " " + DateTime.Now.Date.ToString("dd-MM-yyyy"); ;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();



        }



        private static string RemoveEmptyLines(string lines)
        {
            return Regex.Replace(lines, @"^\s*$\n|\r", string.Empty, RegexOptions.Multiline).TrimEnd();
        }

        static decimal CalculateChange(decimal previous, decimal current)
        {
            if (previous == 0)
                throw new InvalidOperationException();

            var change = current - previous;
            return (decimal)change / previous;
        }
        public static async Task getInitStocksFromSECIDForNSEResults()
        {
            string param = "";

            try
            {


                var filteredtxt = ExecuteCommandNSEFirst(@"C:\Hosts\Breeze\NSE_FIN.bat");

                var data = filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString();

                List<Stock_Financial_Results_NSE> alldatafromtable = new List<Stock_Financial_Results_NSE>();
                using (var db = new STOCKContext())
                {
                    alldatafromtable = db.Stock_Financial_Results_NSE.ToList();


                    //string output = Getdate("https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly");
                    var result = JsonConvert.DeserializeObject<List<FinancialsModel>>(data);
                    foreach (var item in result.Where(x => Convert.ToDateTime(x.broadCastDate) > DateTime.Now.AddDays(-3)).ToList())
                    {
                        Console.WriteLine(item.companyName);
                        MainFinancialsResult mainFinancialsResult = null;
                        MainFinancialsResult2 mainFinancialsResult2 = null;
                        FinancialResultsFromXMl2 financialResultsFromXMl2 = null;
                        FinancialsResultsFromXml3 FinancialsResultsFromXml3 = null;
                        FinancialsResultsFromXml4 FinancialsResultsFromXml4 = null;
                        FinancialsResultsFromXml6 FinancialsResultsFromXml6 = null;
                        FinancialsResultsFromXml5 FinancialsResultsFromXml5 = null;
                        FinancialsResultsFromXml7 FinancialsResultsFromXml7 = null;





                        try
                        {
                            string inline_output = "";
                            param = item.xbrl;
                            inline_output = ExecuteCommandNSExbrl(@"C:\Hosts\Breeze\NSE_FIN_xbrl.bat", param).Split("GANGA").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString();
                            //  inline_output = Getdate(item.xbrl);
                            XmlDocument doc1 = new XmlDocument();
                            doc1.LoadXml(RemoveEmptyLines(inline_output.ToString().Trim()));

                            string jsonText = JsonConvert.SerializeXmlNode(doc1);
                            try
                            {
                                mainFinancialsResult = JsonConvert.DeserializeObject<MainFinancialsResult>(jsonText);
                            }
                            catch (Exception)
                            {

                                try
                                {
                                    mainFinancialsResult2 = JsonConvert.DeserializeObject<MainFinancialsResult2>(jsonText);
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        financialResultsFromXMl2 = JsonConvert.DeserializeObject<FinancialResultsFromXMl2>(jsonText);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            FinancialsResultsFromXml3 = JsonConvert.DeserializeObject<FinancialsResultsFromXml3>(jsonText);
                                        }
                                        catch (Exception)
                                        {


                                            try
                                            {
                                                FinancialsResultsFromXml4 = JsonConvert.DeserializeObject<FinancialsResultsFromXml4>(jsonText);
                                            }
                                            catch (Exception)
                                            {


                                                try
                                                {
                                                    FinancialsResultsFromXml6 = JsonConvert.DeserializeObject<FinancialsResultsFromXml6>(jsonText);
                                                }
                                                catch (Exception)
                                                {

                                                    try
                                                    {
                                                        FinancialsResultsFromXml5 = JsonConvert.DeserializeObject<FinancialsResultsFromXml5>(jsonText);
                                                    }
                                                    catch (Exception)
                                                    {

                                                        try
                                                        {
                                                            FinancialsResultsFromXml7 = JsonConvert.DeserializeObject<FinancialsResultsFromXml7>(jsonText);
                                                        }
                                                        catch (Exception)
                                                        {

                                                            await logexceptio(alldatafromtable, db, item);
                                                            continue;
                                                        }

                                                    }


                                                }


                                            }


                                        }










                                    }

                                }
                            }
                            bool isnew = false;

                            try
                            {

                                var Stock_Financial_Results_all = alldatafromtable.Where(x => x.Symbol == item.symbol);

                                var Stock_Financial_Results_obj = alldatafromtable.FirstOrDefault(x => x.Symbol == item.symbol && x.QuarterEnd == Convert.ToDateTime(item.toDate));
                                var Stock_Financial_Results_Previous = Stock_Financial_Results_all.OrderByDescending(x => x.QuarterEnd).FirstOrDefault(x => x.Symbol == item.symbol && x.QuarterEnd != Convert.ToDateTime(item.toDate));


                                if (Stock_Financial_Results_obj == null)
                                {
                                    isnew = true;
                                    Stock_Financial_Results_obj = new Stock_Financial_Results_NSE();
                                    Stock_Financial_Results_obj.CREATED_ON = DateTime.Now;
                                    Stock_Financial_Results_obj.Symbol = item.symbol;
                                    Stock_Financial_Results_obj.Stock_Name = item.companyName;
                                    db.Stock_Financial_Results_NSE.Add(Stock_Financial_Results_obj);
                                }
                                else
                                {
                                    db.Entry(Stock_Financial_Results_obj).State = EntityState.Modified;
                                }
                                if (mainFinancialsResult != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(mainFinancialsResult.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingOperations.FirstOrDefault().text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(mainFinancialsResult.xbrlixbrl.inbsefinComprehensiveIncomeForThePeriod.FirstOrDefault().text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(mainFinancialsResult.xbrlixbrl.inbsefinProfitLossForPeriod.FirstOrDefault().text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);



                                }
                                if (mainFinancialsResult2 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(mainFinancialsResult2.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingOperations.text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(mainFinancialsResult2.xbrlixbrl.inbsefinComprehensiveIncomeForThePeriod.text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(mainFinancialsResult2.xbrlixbrl.inbsefinProfitLossForPeriod.text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);

                                }
                                if (financialResultsFromXMl2 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(financialResultsFromXMl2.xbrlixbrl.inbsefinBasicEarningsPerShareAfterExtraordinaryItems.FirstOrDefault().text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(financialResultsFromXMl2.xbrlixbrl.inbsefinIncome.FirstOrDefault().text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(financialResultsFromXMl2.xbrlixbrl.inbsefinProfitLossForThePeriod.FirstOrDefault().text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);
                                }

                                if (FinancialsResultsFromXml3 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(FinancialsResultsFromXml3.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations.text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(FinancialsResultsFromXml3.xbrlixbrl.inbsefinIncome.text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(FinancialsResultsFromXml3.xbrlixbrl.inbsefinProfitLossForPeriod.text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);
                                }

                                if (FinancialsResultsFromXml4 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(FinancialsResultsFromXml4.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations.text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(FinancialsResultsFromXml4.xbrlixbrl.inbsefinIncome.text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(FinancialsResultsFromXml4.xbrlixbrl.inbsefinProfitLossForPeriod.text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);
                                }

                                if (FinancialsResultsFromXml6 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(FinancialsResultsFromXml6.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations.text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(FinancialsResultsFromXml6.xbrlixbrl.inbsefinIncome.text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(FinancialsResultsFromXml6.xbrlixbrl.inbsefinProfitLossForPeriod.text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);
                                }

                                if (FinancialsResultsFromXml5 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(FinancialsResultsFromXml5.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations.text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(FinancialsResultsFromXml5.xbrlixbrl.inbsefinIncome.text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(FinancialsResultsFromXml5.xbrlixbrl.inbsefinProfitLossForPeriod.text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);
                                }

                                if (FinancialsResultsFromXml7 != null)
                                {
                                    Stock_Financial_Results_obj.EPS = Convert.ToDecimal(FinancialsResultsFromXml7.xbrlixbrl.inbsefinBasicEarningsLossPerShareFromContinuingAndDiscontinuedOperations.text);
                                    Stock_Financial_Results_obj.Revenue = Convert.ToDecimal(FinancialsResultsFromXml7.xbrlixbrl.inbsefinIncome.text);
                                    Stock_Financial_Results_obj.NET_PROFIT = Convert.ToDecimal(FinancialsResultsFromXml7.xbrlixbrl.inbsefinProfitLossForPeriod.text);
                                    Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                                    Stock_Financial_Results_obj.UPDATED_ON = DateTime.Now;
                                    StatsProfits(Stock_Financial_Results_obj, Stock_Financial_Results_Previous);
                                }

                                Console.WriteLine(Stock_Financial_Results_obj.NET_PROFIT);
                                string strlastvalue = "";
                                var lastvalue = GetLastValue(Stock_Financial_Results_obj.Symbol);
                                if (lastvalue != null)
                                {
                                    strlastvalue = lastvalue.Last;
                                    Stock_Financial_Results_obj.LTP = Convert.ToDecimal(lastvalue.Last);
                                }

                                if (isnew)
                                {


                                    string revenueIncrease = "";
                                    string ChangeInPercenate = string.Empty;

                                    if (Stock_Financial_Results_obj.RevenueIncrease != 0)
                                    {
                                        revenueIncrease = (Stock_Financial_Results_obj.RevenueIncrease / 10000000).ToString();
                                        revenueIncrease = revenueIncrease + " In CR and % of Revenue Increase " + Convert.ToDouble(Stock_Financial_Results_obj.RevenueDifference).ToString("N2");
                                    }


                                    var parameters = new Dictionary<string, string>
                                    {
                                        ["token"] = "afxwjdnt1hq72zbi5p6c9ku8e8k9b3",
                                        ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                                        ["priority"] = "1",
                                        ["message"] = string.Format("Company {0} Result EPS {1},NetProfit {2} Crores  LTP is {3} and Revenue is {4}. Previous EPS is {5} Current EPS is {6}",
                                        item.companyName, Stock_Financial_Results_obj.EPS, Stock_Financial_Results_obj.NET_PROFIT / 10000000, strlastvalue.ToString(), revenueIncrease, Stock_Financial_Results_obj.EPS, Stock_Financial_Results_Previous?.EPS),
                                        ["title"] = "FinacialResult",
                                        ["retry"] = "30",
                                        ["expire"] = "300",
                                        ["html"] = "1",
                                        ["sound"] = "echo",
                                        ["device"] = "iphone"
                                    };

                                    using var client = new HttpClient();
                                    var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                                    FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
                                }




                            }
                            catch (Exception ex)
                            {

                                continue;

                            }
                        }
                        catch (Exception ex)
                        {

                            continue;

                        }
                        db.SaveChanges();
                    }
                    // Assert.IsTrue(output.Contains("StringToBeVerifiedInAUnitTest"));

                    //  string errors = process.StandardError.ReadToEnd();
                    //Assert.IsTrue(string.IsNullOrEmpty(errors));

                }
            }
            catch (Exception ex)
            {


            }


        }


        public static async Task getNSEBlockResults()
        {
            string param = "";

            try
            {


                var filteredtxt = ExecuteCommandNSEBlock(@"C:\Hosts\Breeze\NSE_Deals.bat", "block-deals");

                var data = filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString();

                //List<NSE_DEALS_DB> alldatafromtable = new List<NSE_DEALS_DB>();
                using (var db = new STOCKContext())
                {
                    var alldatafromtable = db.NSE_DEALS.ToList();


                    //string output = Getdate("https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly");
                    var result = JsonConvert.DeserializeObject<Deals>(data);
                    var groupbyScriptName = result.data.GroupBy(X => X.BD_SCRIP_NAME);

                    foreach (var itemS in groupbyScriptName)
                    {
                        foreach (var item in itemS)
                        {


                            var Stock_Financial_Results_obj = alldatafromtable.FirstOrDefault(x => x.Id == item._id && x.TIMESTAMP == item.TIMESTAMP);
                            if (Stock_Financial_Results_obj == null)
                            {
                                try
                                {
                                    Stock_Financial_Results_obj = new NSE_DEALS_DB();
                                    Stock_Financial_Results_obj.Id = item._id;
                                    Stock_Financial_Results_obj.BD_QTY_TRD = item.BD_QTY_TRD;
                                    Stock_Financial_Results_obj.BD_TP_WATP = Convert.ToDecimal(item.BD_TP_WATP);
                                    Stock_Financial_Results_obj.BD_BUY_SELL = item.BD_BUY_SELL;
                                    Stock_Financial_Results_obj.BD_SYMBOL = item.BD_SYMBOL;
                                    Stock_Financial_Results_obj.BD_DT_DATE = Convert.ToDateTime(item.BD_DT_DATE);
                                    Stock_Financial_Results_obj.BD_CLIENT_NAME = item.BD_CLIENT_NAME;
                                    Stock_Financial_Results_obj.BD_REMARKS = item.BD_REMARKS?.ToString();
                                    Stock_Financial_Results_obj.BD_SCRIP_NAME = item.BD_SCRIP_NAME;
                                    Stock_Financial_Results_obj.createdAt = item.createdAt;
                                    Stock_Financial_Results_obj.updatedAt = item.updatedAt;
                                    Stock_Financial_Results_obj.TIMESTAMP = item.TIMESTAMP;
                                    Stock_Financial_Results_obj.mTIMESTAMP = item.mTIMESTAMP;
                                    Stock_Financial_Results_obj.DEALTYPE = "block-deals";
                                }
                                catch
                                {


                                }




                                db.NSE_DEALS.Add(Stock_Financial_Results_obj);



                                db.SaveChanges();
                                if (Stock_Financial_Results_obj.BD_BUY_SELL.ToLower().Contains("buy"))
                                {

                                    var parameters = new Dictionary<string, string>
                                    {
                                        ["token"] = "a6qsrxr7i2nqzfyzqofuys4hm3rwax",
                                        ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                                        ["priority"] = "1",
                                        ["message"] = string.Format("Block Deal by {0} on Company {1} with QTY {2} at PRICE {3} and No of clients Booked {4}",
                                        item.BD_CLIENT_NAME, item.BD_SCRIP_NAME, item.BD_QTY_TRD, item.BD_TP_WATP, itemS.Count()),
                                        ["title"] = "BLOCK DEALS",
                                        ["retry"] = "30",
                                        ["expire"] = "300",
                                        ["html"] = "1",
                                        ["sound"] = "echo",
                                        ["device"] = "iphone"
                                    };
                                    using var client = new HttpClient();
                                    var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                                    FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
                                }


                            }
                        }


                    }
                    // Assert.IsTrue(output.Contains("StringToBeVerifiedInAUnitTest"));

                    //  string errors = process.StandardError.ReadToEnd();
                    //Assert.IsTrue(string.IsNullOrEmpty(errors));

                }
            }
            catch (Exception ex)
            {


            }


        }

        public static async Task getNSEBulkResults()
        {
            string param = "";

            try
            {


                var filteredtxt = ExecuteCommandNSEBlock(@"C:\Hosts\Breeze\NSE_Deals.bat", "bulk-deals");

                var data = filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString();

                //List<NSE_DEALS_DB> alldatafromtable = new List<NSE_DEALS_DB>();
                using (var db = new STOCKContext())
                {
                    var alldatafromtable = db.NSE_DEALS.AsQueryable();


                    //string output = Getdate("https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly");
                    var result = JsonConvert.DeserializeObject<Deals>(data);
                    var groupbyScriptName = result.data.GroupBy(X => X.BD_SCRIP_NAME);

                    foreach (var itemS in groupbyScriptName)
                    {
                        foreach (var item in itemS)
                        {

                            var Stock_Financial_Results_obj = alldatafromtable.FirstOrDefault(x => x.Id == item._id && x.TIMESTAMP == item.TIMESTAMP);
                            if (Stock_Financial_Results_obj == null)
                            {
                                try
                                {
                                    Stock_Financial_Results_obj = new NSE_DEALS_DB();
                                    Stock_Financial_Results_obj.Id = item._id;
                                    Stock_Financial_Results_obj.BD_QTY_TRD = item.BD_QTY_TRD;
                                    Stock_Financial_Results_obj.BD_TP_WATP = Convert.ToDecimal(item.BD_TP_WATP);
                                    Stock_Financial_Results_obj.BD_BUY_SELL = item.BD_BUY_SELL;
                                    Stock_Financial_Results_obj.BD_SYMBOL = item.BD_SYMBOL;
                                    Stock_Financial_Results_obj.BD_DT_DATE = Convert.ToDateTime(item.BD_DT_DATE);
                                    Stock_Financial_Results_obj.BD_CLIENT_NAME = item.BD_CLIENT_NAME;
                                    Stock_Financial_Results_obj.BD_REMARKS = item.BD_REMARKS?.ToString();
                                    Stock_Financial_Results_obj.BD_SCRIP_NAME = item.BD_SCRIP_NAME;
                                    Stock_Financial_Results_obj.createdAt = item.createdAt;
                                    Stock_Financial_Results_obj.updatedAt = item.updatedAt;
                                    Stock_Financial_Results_obj.TIMESTAMP = item.TIMESTAMP;
                                    Stock_Financial_Results_obj.mTIMESTAMP = item.mTIMESTAMP;
                                    Stock_Financial_Results_obj.DEALTYPE = "bulk-deals";
                                }
                                catch
                                {


                                }




                                db.NSE_DEALS.Add(Stock_Financial_Results_obj);



                                db.SaveChanges();
                                if (Stock_Financial_Results_obj.BD_BUY_SELL.ToLower().Contains("buy"))
                                {

                                    var parameters = new Dictionary<string, string>
                                    {
                                        ["token"] = "a6qsrxr7i2nqzfyzqofuys4hm3rwax",
                                        ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                                        ["priority"] = "1",
                                        ["message"] = string.Format("bulk-deals by {0} on Company {1} with QTY {2} at PRICE {3} and No of clients Booked {4}",
                                        item.BD_CLIENT_NAME, item.BD_SCRIP_NAME, item.BD_QTY_TRD, item.BD_TP_WATP, itemS.Count()),
                                        ["title"] = "BLOCK DEALS",
                                        ["retry"] = "30",
                                        ["expire"] = "300",
                                        ["html"] = "1",
                                        ["sound"] = "echo",
                                        ["device"] = "iphone"
                                    };
                                    using var client = new HttpClient();
                                    var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                                    FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
                                }


                            }
                        }


                    }
                    // Assert.IsTrue(output.Contains("StringToBeVerifiedInAUnitTest"));

                    //  string errors = process.StandardError.ReadToEnd();
                    //Assert.IsTrue(string.IsNullOrEmpty(errors));

                }
            }
            catch (Exception ex)
            {


            }


        }

        private static void StatsProfits(Stock_Financial_Results_NSE? Stock_Financial_Results_obj, Stock_Financial_Results_NSE? Stock_Financial_Results_Previous)
        {
            Stock_Financial_Results_obj.RevenueIncrease = Convert.ToDecimal(Stock_Financial_Results_Previous != null ? Stock_Financial_Results_obj.Revenue - Stock_Financial_Results_Previous.Revenue : 0);
            Stock_Financial_Results_obj.Profit_Increase = Convert.ToDecimal(Stock_Financial_Results_Previous != null ? Stock_Financial_Results_obj.NET_PROFIT - Stock_Financial_Results_Previous.NET_PROFIT : 0);
            Stock_Financial_Results_obj.RevenueDifference = Convert.ToDecimal(Stock_Financial_Results_Previous != null ? CalculateChange(Stock_Financial_Results_Previous.Revenue.Value, Stock_Financial_Results_obj.Revenue.Value) : 0);
            Stock_Financial_Results_obj.ProfitDifference = Convert.ToDecimal(Stock_Financial_Results_Previous != null ? CalculateChange(Stock_Financial_Results_Previous.NET_PROFIT.Value, Stock_Financial_Results_obj.NET_PROFIT.Value) : 0);


            Stock_Financial_Results_obj.EPS_INcrease = Convert.ToDecimal(Stock_Financial_Results_Previous != null ? Stock_Financial_Results_obj.EPS - Stock_Financial_Results_Previous.EPS : 0);
            Stock_Financial_Results_obj.EPSDifference = Convert.ToDecimal(Stock_Financial_Results_Previous != null ? CalculateChange(Stock_Financial_Results_Previous.EPS.Value, Stock_Financial_Results_obj.EPS.Value) : 0);
        }

        private static async Task logexceptio(List<Stock_Financial_Results_NSE> alldatafromtable, STOCKContext db, FinancialsModel? item)
        {
            var Stock_Financial_Results_obj = alldatafromtable.FirstOrDefault(x => x.Symbol == item.symbol && x.QuarterEnd == Convert.ToDateTime(item.toDate));
            if (Stock_Financial_Results_obj == null)
            {



                Stock_Financial_Results_obj = new Stock_Financial_Results_NSE();
                Stock_Financial_Results_obj.CREATED_ON = DateTime.Now;
                Stock_Financial_Results_obj.Symbol = item.symbol;
                Stock_Financial_Results_obj.QuarterEnd = Convert.ToDateTime(item.toDate);
                Stock_Financial_Results_obj.Stock_Name = item.companyName;
                db.Stock_Financial_Results_NSE.Add(Stock_Financial_Results_obj);
                var parameters = new Dictionary<string, string>
                {
                    ["token"] = "afxwjdnt1hq72zbi5p6c9ku8e8k9b3",
                    ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                    ["priority"] = "1",
                    ["message"] = string.Format("Company {0} Error while fetching Issues", item.companyName),
                    ["title"] = "FinacialResult",
                    ["retry"] = "30",
                    ["expire"] = "300",
                    ["html"] = "1",
                    ["sound"] = "echo",
                    ["device"] = "iphone"
                };

                using var client = new HttpClient();
                var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();

            }
        }

        public static Stock_Financial_Results_NSE GetLastValue(string Symbol)
        {
            var ds = new DataSet();
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("GetLatestFiancialsDetails", conn);
                sqlComm.Parameters.AddWithValue("@symbol", Symbol);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                var NewStock = ds.Tables[0].Rows.Cast<DataRow>();

                foreach (var r in NewStock)
                {
                    try
                    {
                        var _stokc = new Stock_Financial_Results_NSE();
                        _stokc.Last = (r["Last"].ToString()).ToString();



                        return _stokc;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

                return null;//.Where(X => X.Open <= 120);
            }

            return null;
        }
        private static string Getdate(string param)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";

            startInfo.Arguments = @"/c powershell -executionpolicy unrestricted " + "C:\\Ganga\\tst.ps1" + " " + param;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();



        }

       //public static  decimal CalculateChange(decimal previous, decimal current)
       // {
       //     if (previous == 0)
       //         return 0;

       //     if (current == 0)
       //         return -100;

       //     var change = ((current - previous) / previous) * 100;

       //     return change;
       // }

        public static async Task getInitStocksFromSECID()
        {




            using (var db = new STOCKContext())
            {
                Console.WriteLine("Database Connected");
                Console.WriteLine();
                Console.WriteLine("Listing Category Sales For 1997s");
                var equites = db.Equitys.AsNoTracking().ToList().Where(x => x.MSN_SECID != null).Where(x => Convert.ToDateTime(x.UpdatedOn) != Convert.ToDateTime(DateTime.Now));


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
                            var stockinsights = await getstockDetailsinsights(equity.MSN_SECID);


                            equity.Industry = stockresult.equity.company.industry ?? string.Empty;
                            equity.SectorName = stockresult.equity.company.sector ?? string.Empty;
                            equity.MSN_Growth = stockinsights.MSN_Growth;
                            equity.MSN_Health = stockinsights.MSN_Health;
                            equity.MSN_Performance = stockinsights.MSN_Performance;
                            equity.MSN_Valuation = stockinsights.MSN_Valuation;
                            equity.MSN_Earnings = stockinsights.MSN_Earnings;
                            equity.Week52High = Convert.ToDecimal(stockresult.quote.price52wHigh);
                            equity.Week52low= Convert.ToDecimal(stockresult.quote.price52wLow);
                            equity.Is52High = Convert.ToDecimal(stockresult.quote.priceDayHigh) > Convert.ToDecimal(stockresult.quote.price52wHigh);
                            equity.Is52Low = Convert.ToDecimal(stockresult.quote.priceDayLow) > Convert.ToDecimal(stockresult.quote.priceDayLow);
                            equity.LTP = Convert.ToDecimal(stockresult.quote.price);
                            equity.LTT = DateTime.Now;//stockresult.quote.timeLastTraded;
                            equity.ChangeOfNow =Convert.ToDecimal(CalculateChange(Convert.ToDecimal(stockresult.quote.price52wHigh), Convert.ToDecimal(stockresult.quote.price)))*100;

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
                            //System.Threading.Thread.Sleep(1000);

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

        public static async Task<MNS_valuations> getstockDetailsinsights(string sec)
        {
            MNS_valuations mNS_Valuations = new MNS_valuations();
            ////System.Threading.Thread.Sleep(1000);
            var url = "https://api.msn.com/msn/v0/pages/finance/insights?apikey=0QfOX3Vn51YCzitbLaRkTTBadtWpgTN8NZLW0C1SEM&activityId=E4BA41B3-D3A3-4A2A-8382-714F85D9AD2F&ocid=finance-utils-peregrine&cm=en-us&it=web&scn=ANON&ids={0}&wrapodata=false";
            var result = await HttpHelper.Get<List<MSNInsights>>(string.Format(url, sec), "");
            mNS_Valuations.MSN_Valuation = GetAIInsights(result, "Valuation", "good");
            mNS_Valuations.MSN_Growth = GetAIInsights(result, "growth", "good");
            mNS_Valuations.MSN_Performance = GetAIInsights(result, "performance", "good");
            mNS_Valuations.MSN_Earnings = GetAIInsights(result, "earnings", "good");
            mNS_Valuations.MSN_Health = GetAIInsights(result, "health", "good");




            return mNS_Valuations;
            //// var findresult = JsonConvert.DeserializeObject<StockQueryFirst>(result.data.stocks.FirstOrDefault().ToString());
            ///


            //try
            //{
            //    var client = new HttpClient();
            //    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.msn.com/msn/v0/pages/finance/insights?apikey=0QfOX3Vn51YCzitbLaRkTTBadtWpgTN8NZLW0C1SEM&activityId=E4BA41B3-D3A3-4A2A-8382-714F85D9AD2F&ocid=finance-utils-peregrine&cm=en-us&it=web&scn=ANON&ids=ahh5pr&wrapodata=false");
            //    // request.Headers.Add("Cookie", "_C_Auth=; MUID=3DFCE38B7B936AE335B3F69D7ABA6B4C; _C_ETH=1; _EDGE_S=F=1&SID=1B36B047CE366059153FA551CF1F612B; _EDGE_V=1; MUIDB=3DFCE38B7B936AE335B3F69D7ABA6B4C");
            //    var response = await client.SendAsync(request);
            //    response.EnsureSuccessStatusCode();
            //    string body = await response.Content.ReadAsStringAsync();
            //    //Console.WriteLine(await response.Content.ReadAsStringAsync());
            //    var resultss = JsonConvert.DeserializeObject<List<MSNInsights>>(body);
            //    return null;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private static int GetAIInsights(List<MSNInsights> result, string category, string good)
        {
            var Valuation_data = result.FirstOrDefault()?.insights.Where(x => x.category.ToLower() == category.ToLower()).ToList();
            var Valuation_data_good = Valuation_data.Where(x => x.details.evaluationStatus.ToLower() == "good").Count();
            //var valuationcount = Convert.ToInt32(Valuation_data_good + 0.14285714285714288 + 0.14285714285714285);
            return Convert.ToInt32(Valuation_data_good + 0.14285714285714288 + 0.14285714285714285);
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

        public static async Task researchreportsTopNav()
        {
            var options = new RestClientOptions("")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://www.ndtvprofit.com/api/v1/collections/research-reports?sort=latest-published", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
        }

        public static async Task LatestPublished()
        {


            var result = await HttpHelper.Get<LatestNewFromNDTV>(string.Format("https://www.ndtvprofit.com/api/v1/collections/research-reports?sort=latest-published", ""), "");
            List<NDTVNews> nDTVNews = new List<NDTVNews>();

            using (var db = new STOCKContext())
            {
                nDTVNews = db.NDTVNews.AsNoTracking().ToList();
                foreach (var s in result.items)
                {

                    NDTVNews news = nDTVNews.ToList().FirstOrDefault(x => x.ContentId == s.id.ToString());

                    if (news == null)
                    {

                        news = new NDTVNews();
                        news.CreatedOn = DateTime.Now;
                        news.Headline = s.story.headline;
                        news.ContentId = s.id.ToString();
                        news.Url = s.story.url;
                        news.MSNID = s.id.ToString();
                        db.NDTVNews.Add(news);


                        var parameters = new Dictionary<string, string>
                        {
                            ["token"] = "a7ae6r4ojf3eiywvptzy1u718eh15a",
                            ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                            ["priority"] = "1",
                            ["message"] = string.Format("Head line {0} url {1}", news.Headline, news.Url),
                            ["title"] = "NDTV",
                            ["retry"] = "30",
                            ["expire"] = "300",
                            ["html"] = "1",
                            ["sound"] = "echo",
                            ["device"] = "iphone"
                        };

                        using var client = new HttpClient();
                        var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                                    FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();

                    }
                }
                db.SaveChanges();
            }


        }



        public static async Task LatestPublishedMSN()
        {


            var result = await HttpHelper.Get<ef_finance_trending_growth>("https://assets.msn.com/service/MSN/Feed/me?$top=11&DisableTypeSerialization=true&activityId=B4517126-63BD-4D29-9DE9-AA33F41FAEAF&apikey=0QfOX3Vn51YCzitbLaRkTTBadtWpgTN8NZLW0C1SEM&cipenabled=false&cm=en-in&contentType=article,video,slideshow&infopaneCount=5&it=edgeid&location=17.3751|78.3994&ocid=finance-verthp-feeds&query=ef_finance_trending_growth&queryType=entityfeed&responseSchema=cardview&scn=APP_ANON&timeOut=3000&user=m-31D8CAFFBBD869022E35DE94BAB868BF&wrapodata=false", "");
            List<NDTVNews> nDTVNews = new List<NDTVNews>();
            List<NDTVNews> savenDTVNews = new List<NDTVNews>();
            using (var db = new STOCKContext())
            {
                foreach (var cards in result.subCards)
                {
                    if (cards.subCards == null)
                    {
                        continue;
                    }

                    nDTVNews = db.NDTVNews.AsNoTracking().ToList();
                    foreach (var s in cards.subCards)
                    {

                        NDTVNews news = nDTVNews.ToList().FirstOrDefault(x => x.ContentId == s.id.ToString());

                        if (news == null)
                        {

                            news = new NDTVNews();
                            news.CreatedOn = DateTime.Now;
                            news.Headline = s.title;
                            news.ContentId = s.id.ToString();
                            news.Url = s.url;
                            news.MSNID = s.financeMetadata.stocks.Any() ? s.financeMetadata.stocks.First().stockId.ToString() : "";
                            savenDTVNews.Add(news);


                            var parameters = new Dictionary<string, string>
                            {
                                ["token"] = "aday3s2vefpndctqyyee9tv1392m1q",
                                ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                                ["priority"] = "1",
                                ["message"] = string.Format("Head line {0} url {1}", news.Headline, news.Url),
                                ["title"] = "MSN_TREND_TOP",
                                ["retry"] = "30",
                                ["expire"] = "300",
                                ["html"] = "1",
                                ["sound"] = "echo",
                                ["device"] = "iphone"
                            };

                            using var client = new HttpClient();
                            var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                                        FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();

                        }
                    }




                }
                db.NDTVNews.AddRange(savenDTVNews);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception EX)
                {


                }
            }

        }



        public static async Task LatestPublishedMSNTOP()
        {


            var resultss = await HttpHelper.Get<List<MSNExchangeStatisticsModel>>("https://assets.msn.com/service/Finance/ExchangeStatistics?apikey=0QfOX3Vn51YCzitbLaRkTTBadtWpgTN8NZLW0C1SEM&activityId=4AE92C05-AB7E-4866-8581-CBF3BD26B942&ocid=finance-utils-peregrine&cm=en-in&it=edgeid&scn=APP_ANON&ids=r6dwoq&wrapodata=false", "");
            var result = resultss.FirstOrDefault();
            List<string> active = new List<string>();
            List<string> gainer = new List<string>();
            List<string> losers = new List<string>();
            List<MSNExchangeStatistics> statistics = new List<MSNExchangeStatistics>();
            using (var db = new STOCKContext())
            {
                statistics = db.MSNExchangeStatistics.AsNoTracking().ToList();

                List<MSNExchangeStatistics> activestatistics = new List<MSNExchangeStatistics>();
                foreach (var s in result.active)
                {
                    var outputdata = (LatestPublishedMSNTOP_Save(statistics, active, s.displayName, s.symbol, s.instrumentId, "Active", result.timeLastUpdated));
                    if (outputdata != null)
                    {

                        active.Add(s.displayName);
                        activestatistics.Add(outputdata);
                    }


                }
                db.MSNExchangeStatistics.AddRange(activestatistics);


                if (active.Any())
                {
                    var parameters = new Dictionary<string, string>
                    {
                        ["token"] = "a99jvsqx7jhzc6ntojmx4c2e5raedz",
                        ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                        ["priority"] = "1",
                        ["message"] = string.Format("Active {0}", string.Join(",", active)),
                        ["title"] = "MSN_ACTIVE",
                        ["retry"] = "30",
                        ["expire"] = "300",
                        ["html"] = "1",
                        ["sound"] = "echo",
                        ["device"] = "iphone"
                    };
                    using var client = new HttpClient();
                    var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
                                FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
                }

                List<MSNExchangeStatistics> gainertatistics = new List<MSNExchangeStatistics>();
                foreach (var gaibner in result.gainers)
                {
                    //(LatestPublishedMSNTOP_Save(statistics, active, gaibner.displayName, gaibner.symbol, gaibner.instrumentId, "Gainer"));

                    var outputdata = (LatestPublishedMSNTOP_Save(statistics, active, gaibner.displayName, gaibner.symbol, gaibner.instrumentId, "Gainer", result.timeLastUpdated));
                    if (outputdata != null)
                    {

                        gainer.Add(gaibner.displayName);
                        gainertatistics.Add(outputdata);
                    }

                }

                db.MSNExchangeStatistics.AddRange(gainertatistics);
                //  db.SaveChanges();

                if (gainer.Any())
                {
                    var parameters = new Dictionary<string, string>
                    {
                        ["token"] = "a99jvsqx7jhzc6ntojmx4c2e5raedz",
                        ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                        ["priority"] = "1",
                        ["message"] = string.Format("gainer {0}", string.Join(",", gainer)),
                        ["title"] = "MSN_Gain",
                        ["retry"] = "30",
                        ["expire"] = "300",
                        ["html"] = "1",
                        ["sound"] = "echo",
                        ["device"] = "iphone"
                    };

                    using var clientg = new HttpClient();
                    var responseg = await clientg.PostAsync("https://api.pushover.net/1/messages.json", new
                                FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
                }

                List<MSNExchangeStatistics> loserstatistics = new List<MSNExchangeStatistics>();
                foreach (var gaibner in result.losers)
                {
                    // losers.AddRange(LatestPublishedMSNTOP_Save(statistics, active, gaibner.displayName, gaibner.symbol, gaibner.instrumentId, "Losers"));

                    var outputdata = (LatestPublishedMSNTOP_Save(statistics, active, gaibner.displayName, gaibner.symbol, gaibner.instrumentId, "Losers", result.timeLastUpdated));
                    if (outputdata != null)
                    {

                        losers.Add(gaibner.displayName);
                        loserstatistics.Add(outputdata);
                    }

                }
                db.MSNExchangeStatistics.AddRange(loserstatistics);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            if (losers.Any())
            {

                //var parameters = new Dictionary<string, string>
                //{
                //    ["token"] = "a99jvsqx7jhzc6ntojmx4c2e5raedz",
                //    ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                //    ["priority"] = "1",
                //    ["message"] = string.Format("Losers {0}", string.Join(",", losers)),
                //    ["title"] = "MSN_LOSER",
                //    ["retry"] = "30",
                //    ["expire"] = "300",
                //    ["html"] = "1",
                //    ["sound"] = "echo",
                //    ["device"] = "iphone"
                //};

                //using var clientl = new HttpClient();
                //var responsel = await clientl.PostAsync("https://api.pushover.net/1/messages.json", new
                //            FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
            }


        }

        private static MSNExchangeStatistics LatestPublishedMSNTOP_Save(List<MSNExchangeStatistics> MSNExchangeStatisticslist, List<string> active,
            string CompanyName, string symbol, string MSNID, string status, DateTime result)
        {
            // List<string> result = new List<string>();

            var news = MSNExchangeStatisticslist.ToList().LastOrDefault(x => x.symbol == symbol.ToString() && Convert.ToDateTime(x.CreatedOn).ToString() == Convert.ToDateTime(result).ToString());

            if (news == null)
            {
                //using (var db = new STOCKContext())
                //{
                news = new MSNExchangeStatistics();
                news.CreatedOn = Convert.ToDateTime(result);
                news.CompanyName = CompanyName;
                news.symbol = symbol;
                news.MSNID = MSNID;
                news.Type = status;

                //db.MSNExchangeStatistics.Add(news);
                //db.SaveChanges();
                // result.Add(CompanyName);
                // }
                // db.SaveChanges();
            }
            else
            {
                return null;
            }
            return news;
        }

        public static async Task LatestPublishedAdvanced()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.ndtvprofit.com/route-data.json?path=%2Fthe-latest&src=topnav");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            var result = JsonConvert.DeserializeObject<topnav>(await response.Content.ReadAsStringAsync());
            //  var result = await HttpHelper.Get<topnav>(string.Format("https://www.ndtvprofit.com/route-data.json?path=%2Fthe-latest&src=topnav", ""), "");

            //foreach (var testitem in result.items)
            //{
            using (var db = new STOCKContext())
            {
                foreach (var s in result.items)
                {

                    NDTVNews news = db.NDTVNews.ToList().FirstOrDefault(x => x.ContentId == s.id.ToString());

                    if (news == null)
                    {

                        news = new NDTVNews();
                        news.CreatedOn = DateTime.Now;
                        news.Headline = s.story.headline;
                        news.ContentId = s.id.ToString();
                        news.Url = "https://www.ndtvprofit.com/" + s.story.slug;
                        db.NDTVNews.Add(news);
                        db.SaveChanges();

                        var parameters = new Dictionary<string, string>
                        {
                            ["token"] = "a7ae6r4ojf3eiywvptzy1u718eh15a",
                            ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                            ["priority"] = "1",
                            ["message"] = string.Format("Head line {0} url {1}", news.Headline, news.Url),
                            ["title"] = "NDTV",
                            ["retry"] = "30",
                            ["expire"] = "300",
                            ["html"] = "1",
                            ["sound"] = "echo",
                            ["device"] = "iphone"
                        };

                        using var clients = new HttpClient();
                        var responses = await clients.PostAsync("https://api.pushover.net/1/messages.json", new
                                    FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();

                    }
                }
            }

        }

        public static void CalculateXIRR()
        {
            throw new NotImplementedException();
        }

        public static async Task GenerateNiftyTraderAsync(string expiryDate, string Code)
        {
            // string ExpiryDate = "2024-10-17";
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync($"https://webapi.niftytrader.in/webapi/option/option-chain-data?symbol={Code}&exchange=nse&expiryDate={expiryDate}&atmBelow=0&atmAbove=0");
                    result.EnsureSuccessStatusCode();
                    string resultContentString = await result.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(resultContentString))
                    {
                        return;
                    }
                    NiftyTrader resultContent = JsonConvert.DeserializeObject<NiftyTrader>(resultContentString);

                    var dt = ToDataTable(resultContent.resultData.opDatas.ToList());
                    var opTotals = resultContent.resultData.opTotals;

                    List<NIFTYTRader_ITM_OTM> nIFTYTRader_ITM_OTMs = new List<NIFTYTRader_ITM_OTM>
                    {
                        new NIFTYTRader_ITM_OTM()
                        {

                            CreatedDate = resultContent.resultData.opDatas.FirstOrDefault().created_at,
                            itm_total_calls_change_oi = opTotals.itm_total_calls.itm_total_calls_change_oi,
                            itm_total_calls_change_oi_value = opTotals.itm_total_calls.itm_total_calls_change_oi_value,
                            itm_total_calls_oi = opTotals.itm_total_calls.itm_total_calls_oi,
                            itm_total_calls_oi_value = opTotals.itm_total_calls.itm_total_calls_oi_value,
                            itm_total_calls_volume = opTotals.itm_total_calls.itm_total_calls_volume,

                            itm_total_puts_change_oi = opTotals.itm_total_puts.itm_total_puts_change_oi,
                            itm_total_puts_change_oi_value = opTotals.itm_total_puts.itm_total_puts_change_oi_value,
                            itm_total_puts_oi = opTotals.itm_total_puts.itm_total_puts_oi,
                            itm_total_puts_oi_value = opTotals.itm_total_puts.itm_total_puts_oi_value,
                            itm_total_puts_volume = opTotals.itm_total_puts.itm_total_puts_volume,

                            otm_total_calls_change_oi = opTotals.otm_total_calls.otm_total_calls_change_oi,
                            otm_total_calls_change_oi_value = opTotals.otm_total_calls.otm_total_calls_change_oi_value,
                            otm_total_calls_oi = opTotals.otm_total_calls.otm_total_calls_oi,
                            otm_total_calls_oi_value = opTotals.otm_total_calls.otm_total_calls_oi_value,
                            otm_total_calls_volume = opTotals.otm_total_calls.otm_total_calls_volume,

                            otm_total_puts_change_oi = opTotals.otm_total_puts.otm_total_puts_change_oi,
                            otm_total_puts_change_oi_value = opTotals.otm_total_puts.otm_total_puts_change_oi_value,
                            otm_total_puts_oi = opTotals.otm_total_puts.otm_total_puts_oi,
                            otm_total_puts_oi_value = opTotals.otm_total_puts.otm_total_puts_oi_value,
                            otm_total_puts_volume = opTotals.otm_total_puts.otm_total_puts_volume,

                            total_calls_change_oi = opTotals.total_calls_puts.total_calls_change_oi,
                            total_calls_change_oi_value = opTotals.total_calls_puts.total_puts_change_oi_value,
                            total_calls_oi = opTotals.total_calls_puts.total_calls_oi,
                            total_puts_change_oi = opTotals.total_calls_puts.total_puts_change_oi,
                            total_calls_oi_value = opTotals.total_calls_puts.total_calls_oi_value,


                            total_calls_volume = opTotals.total_calls_puts.total_calls_volume,
                            total_puts_change_oi_value = opTotals.total_calls_puts.total_puts_change_oi_value,
                            total_puts_oi_value = opTotals.total_calls_puts.total_puts_oi_value,
                            total_puts_oi = opTotals.total_calls_puts.total_puts_oi,
                            total_puts_volume = opTotals.total_calls_puts.total_puts_volume

                        }
                    };

                    var dt2 = ToDataTable(nIFTYTRader_ITM_OTMs.ToList());

                    using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                    {
                        var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                        {
                            DestinationTableName = "dbo.NIFTYTRADER",
                            BatchSize = dt.Rows.Count
                        };
                        conn.Open();
                        bc.WriteToServer(dt);
                        conn.Close();
                        bc.Close();
                    }

                    using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                    {
                        var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                        {
                            DestinationTableName = "dbo.NIFTYTRader_ITM_OTM",
                            BatchSize = dt.Rows.Count
                        };
                        conn.Open();
                        bc.WriteToServer(dt2);
                        conn.Close();
                        bc.Close();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static async Task GenerateBankNiftyTraderAsync()
        {
            string ExpiryDate = "2024-10-17";
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"https://webapi.niftytrader.in/webapi/option/option-chain-data?symbol=banknifty&exchange=nse&expiryDate={ExpiryDate}&atmBelow=0&atmAbove=0");
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                NiftyTrader resultContent = JsonConvert.DeserializeObject<NiftyTrader>(resultContentString);

                var dt = ToDataTable(resultContent.resultData.opDatas.ToList());

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                    {
                        DestinationTableName = "dbo.Ticker_Stocks",
                        BatchSize = dt.Rows.Count
                    };
                    conn.Open();
                    bc.WriteToServer(dt);
                    conn.Close();
                    bc.Close();
                }

            }

        }



        public static async Task Get52weeklow()
        {
            string param = "";

            try
            {


                var filteredtxt = ExecuteCommandNSEWeek(@"C:\Hosts\Breeze\NSE_Deals.bat", "live-analysis-data-52weeklowstock");

                var datalow = filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString();
                var resultlow = JsonConvert.DeserializeObject<NSE_lowHigh>(datalow);
                var groupbyScriptName = resultlow.data.GroupBy(X => X.symbol);

                //List<NSE_DEALS_DB> alldatafromtable = new List<NSE_DEALS_DB>();
                using (var db = new STOCKContext())
                {
                    var alldatafromtable = db.NSELOWHIGH.AsQueryable();


                    //string output = Getdate("https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly");


                    foreach (var itemS in groupbyScriptName)
                    {
                        foreach (var item in itemS)
                        {

                            var Stock_Financial_Results_obj = alldatafromtable.FirstOrDefault(x => x.symbol == item.symbol && x.CreatedON == resultlow.timestamp);
                            if (Stock_Financial_Results_obj == null)
                            {
                                try
                                {
                                    Stock_Financial_Results_obj = new NSELOWHIGH();
                                    Stock_Financial_Results_obj.pChange = item.pChange;
                                    Stock_Financial_Results_obj.comapnyName = item.comapnyName;
                                    Stock_Financial_Results_obj.prevClose = Convert.ToDecimal(item.prevClose);
                                    Stock_Financial_Results_obj.ltp = item.ltp;
                                    Stock_Financial_Results_obj.prev52WHL = item.prev52WHL;
                                    Stock_Financial_Results_obj.CreatedON = resultlow.timestamp;
                                    Stock_Financial_Results_obj.new52WHL = item.new52WHL;
                                    Stock_Financial_Results_obj.pChange = item.pChange;
                                    Stock_Financial_Results_obj.change = item.change;
                                    Stock_Financial_Results_obj.symbol = item.symbol;
                                    Stock_Financial_Results_obj.series = item.series;
                                    Stock_Financial_Results_obj.Type = "Low";
                                    Stock_Financial_Results_obj.prevHLDate = item.prevHLDate;





                                }
                                catch
                                {


                                }




                                db.NSELOWHIGH.Add(Stock_Financial_Results_obj);



                                db.SaveChanges();



                            }
                        }


                    }
                    // Assert.IsTrue(output.Contains("StringToBeVerifiedInAUnitTest"));

                    //  string errors = process.StandardError.ReadToEnd();
                    //Assert.IsTrue(string.IsNullOrEmpty(errors));


                    db.Equitys.FromSqlRaw(

                       @"Update Equitys set Is52High=1 where IssuerName in (

Select e.IssuerName from Equitys E
inner join [NSELOWHIGH] HL on e.IssuerName=HL.symbol and Type='HIGH' and CAST(HL.CreatedON as Date)=CAST(GETDATE() as Date))


Update Equitys set Is52Low=1 where IssuerName in (

Select e.IssuerName from Equitys E
inner join [NSELOWHIGH] HL on e.IssuerName=HL.symbol and Type='LOW' and CAST(HL.CreatedON as Date)=CAST(GETDATE() as Date))");
                }
            }
            catch (Exception ex)
            {


            }


        }


        public static async Task Get52weekhigh()
        {
            string param = "";

            try
            {


                var filteredtxt = ExecuteCommandNSEWeek(@"C:\Hosts\Breeze\NSE_Deals.bat", "live-analysis-data-52weekhighstock");

                var datalow = filteredtxt.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).LastOrDefault().ToString();
                var resultlow = JsonConvert.DeserializeObject<NSE_lowHigh>(datalow);
                var groupbyScriptName = resultlow.data.GroupBy(X => X.symbol);

                //List<NSE_DEALS_DB> alldatafromtable = new List<NSE_DEALS_DB>();
                using (var db = new STOCKContext())
                {
                    var alldatafromtable = db.NSELOWHIGH.AsQueryable();


                    //string output = Getdate("https://www.nseindia.com/api/corporates-financial-results?index=equities&period=Quarterly");


                    foreach (var itemS in groupbyScriptName)
                    {
                        foreach (var item in itemS)
                        {

                            var Stock_Financial_Results_obj = alldatafromtable.FirstOrDefault(x => x.symbol == item.symbol && x.CreatedON == resultlow.timestamp);
                            if (Stock_Financial_Results_obj == null)
                            {
                                try
                                {
                                    Stock_Financial_Results_obj = new NSELOWHIGH();
                                    Stock_Financial_Results_obj.pChange = item.pChange;
                                    Stock_Financial_Results_obj.comapnyName = item.comapnyName;
                                    Stock_Financial_Results_obj.prevClose = Convert.ToDecimal(item.prevClose);
                                    Stock_Financial_Results_obj.ltp = item.ltp;
                                    Stock_Financial_Results_obj.prev52WHL = item.prev52WHL;
                                    Stock_Financial_Results_obj.CreatedON = resultlow.timestamp;
                                    Stock_Financial_Results_obj.new52WHL = item.new52WHL;
                                    Stock_Financial_Results_obj.pChange = item.pChange;
                                    Stock_Financial_Results_obj.change = item.change;
                                    Stock_Financial_Results_obj.symbol = item.symbol;
                                    Stock_Financial_Results_obj.series = item.series;
                                    Stock_Financial_Results_obj.prevHLDate = item.prevHLDate;
                                    Stock_Financial_Results_obj.Type = "High";





                                }
                                catch
                                {


                                }




                                db.NSELOWHIGH.Add(Stock_Financial_Results_obj);



                                db.SaveChanges();



                            }
                        }


                    }
                    // Assert.IsTrue(output.Contains("StringToBeVerifiedInAUnitTest"));

                    //  string errors = process.StandardError.ReadToEnd();
                    //Assert.IsTrue(string.IsNullOrEmpty(errors));

                    db.Equitys.FromSqlRaw(

                        @"Update Equitys set Is52High=1 where IssuerName in (

Select e.IssuerName from Equitys E
inner join [NSELOWHIGH] HL on e.IssuerName=HL.symbol and Type='HIGH' and CAST(HL.CreatedON as Date)=CAST(GETDATE() as Date))


Update Equitys set Is52Low=1 where IssuerName in (

Select e.IssuerName from Equitys E
inner join [NSELOWHIGH] HL on e.IssuerName=HL.symbol and Type='LOW' and CAST(HL.CreatedON as Date)=CAST(GETDATE() as Date))");
                }
            }
            catch (Exception ex)
            {


            }


        }
    }

}
