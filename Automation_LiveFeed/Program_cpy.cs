using Breeze;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using STM_API.Model;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using StocksBuy.model.BreezeModel.Quote;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

//.Net Core 3.1
namespace ConsoleAppTestProject
{
    public class EquitiesOptions
    {
        public string Symbol { set; get; }
        public string IssuerName { set; get; }
        public string SecurityName { set; get; }
    }

    public static class AppSettings
    {
        //public string url { get; set; }

        public static List<EquitiesOptions> GetEquities(string NiftyStrikePrice, string BankNifty)
        {

            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("GetEquitiesSymbolForOption", conn);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            try
            {
                List<EquitiesOptions> items = ds.Tables[0].AsEnumerable().Select(row => new EquitiesOptions
                {
                    Symbol = row.Field<string>("Symbol"),
                    IssuerName = row.Field<string>("IssuerName"),
                    SecurityName = row.Field<string>("SecurityName"),

                }).ToList();
                return items.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }



        }
    }



    public class Program
    {

        public List<Equities> equities = new List<Equities>();
        public Program()
        {
            equities = new List<Equities>();

        }


        public static double round_down(double num, int divisor)
        {
            return num - (num % divisor);

        }
        static async Task Main(string[] args)
        {


            var url = "";

            var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string HUbUrl = url;

            try
            {
                string APIKEY = string.Empty;
                string APISecret = string.Empty;
                string token = string.Empty;
                string[] line;
                string arg = "0";
                if (args.Any())
                    arg = args[0];
                switch (Convert.ToInt16(arg))
                {
                    case 0:
                        line = lines[0].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:90/BreezeOperation";
                        break;
                    case 1:
                        line = lines[1].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:91/BreezeOperation";
                        break;
                    case 2:
                        line = lines[2].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:92/BreezeOperation";
                        break;
                    case 3:
                        line = lines[3].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:99/BreezeOperation";
                        break;

                }
                BreezeConnect breeze = new BreezeConnect(APIKEY);
                breeze.generateSessionAsPerVersion(APISecret, token);
                var responseObject = await breeze.wsConnectAsync();
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(responseObject));
                Console.WriteLine(arg);
                Console.WriteLine(HUbUrl);
                await using var connection = new HubConnectionBuilder().WithUrl("http://localhost:48/BreezeOperation").WithAutomaticReconnect().Build();
                await connection.StartAsync();
                Console.WriteLine(connection.ConnectionId);

                

                var txt = System.IO.File.ReadAllText(@"C:\Hosts\ICICI_Key\NIftyTrader.txt").Split(Environment.NewLine);

                var nify = txt[0].Split(",");
                var bnify = txt[1].Split(",");


                connection.On<List<string>>("SendGetTickDataForOptions", async param =>
                {
                    //var results = GetQuote(APISecret, token, data.Success.session_token, APIKEY, "NIFTY", "NFO", "2024-10-31", "options", "call");

                    double Niftyspotpriceselection = GerQuoteData(nify[0], nify[1], breeze);
                    double Bankspotpriceselection = GerQuoteData("CNXBAN", bnify[1], breeze);

                    var symbol = AppSettings.GetEquities(Convert.ToInt32(Niftyspotpriceselection).ToString(), Convert.ToInt32(Bankspotpriceselection).ToString());

                    var BK_actual_symbols = symbol.ToList().Where(x => x.SecurityName.Contains("-PE")).ToList().Where(x => x.SecurityName.Contains(bnify[2].ToString())).ToList().Where(x => x.SecurityName.Contains(Bankspotpriceselection.ToString()));

                    var actual_symbols = symbol.ToList().Where(x=>x.SecurityName.Contains("-PE")).ToList().Where(x => x.SecurityName.Contains(nify[2].ToString())).ToList().Where(x=>x.SecurityName.Contains(Niftyspotpriceselection.ToString()));
                    foreach (var item in actual_symbols)
                    {
                        var result = await breeze.subscribeFeedsAsync(item.Symbol);
                    }
                    Thread.Sleep(10);
                    foreach (var item in BK_actual_symbols)
                    {
                        var result = await breeze.subscribeFeedsAsync(item.Symbol);
                    }




                });


                //Console.WriteLine(await breeze.subscribeFeedsAsync(exchangeCode: "NFO",
                //         stockCode: "NIFTY",
                //         productType: "Options",
                //         expiryDate: "21-Nov-2024",
                //         strikePrice: "23300",
                //         right: "call",
                //         false, true
                //         ));

               
                breeze.ticker(async (data) =>
                {

                    try
                    {

                        // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(data));
                        if (connection.State == HubConnectionState.Connected)
                        {
                            //Console.WriteLine(JsonSerializer.Serialize(data));
                            // Console.WriteLine("Ticker Data:" + System.Text.Json.JsonSerializer.Serialize(data));
                            System.IO.File.AppendAllText("C:\\Hosts\\ICICI_Key\\ticks.txt", System.Text.Json.JsonSerializer.Serialize(data));
                            await connection.InvokeAsync("GetTickDataOption", System.Text.Json.JsonSerializer.Serialize(data));
                        }

                    }






                    catch (Exception)
                    {


                    }






                });
                //BreezeConnect breeze = new BreezeConnect(APIKEY);
                //breeze.generateSessionAsPerVersion(APISecret, token);
                //var responseObject = await breeze.wsConnectAsync();
                //Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(responseObject));


                //Console.WriteLine("Conection" + connection.ConnectionId);
                //Console.WriteLine("GetAllStocksForLoadAutomation" + Convert.ToInt16(arg));
                //await connection.SendAsync("GetAllStocksForLoadAutomation", Convert.ToInt16(arg));











                //Console.WriteLine(await breeze.subscribeFeedsAsync(exchangeCode: "NFO",
                //         stockCode: "NIFTY",
                //         productType: "Options",
                //         expiryDate: "31-10-2024",
                //         strikePrice: "24000",
                //         right: "call",
                //         false,true
                //         ));

                //connection.On<List<string>>("SendGetAllStocksForLoadAutomation", async param =>
                //{
                //    Console.WriteLine("Count" + param.Count);
                //    Stopwatch stopwatch = new Stopwatch();
                //    Console.WriteLine("Starting the stopwatch...");
                //    stopwatch.Start();
                //    foreach (var item in param.Select(x => x).Distinct().ToList())
                //    {
                //        try
                //        {
                //           // await breeze.subscribeFeedsAsync(item.ToString());
                //            Thread.Sleep(TimeSpan.FromMilliseconds(0.5));
                //        }
                //        catch (Exception ex)
                //        {


                //        }
                //    }

                //    stopwatch.Stop();
                //    Console.WriteLine("Elapsed time: " + stopwatch.Elapsed);
                //    stopwatch.Reset();

                //});





                Console.WriteLine("");


                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static double GerQuoteData(string Stockcode, string expdate, BreezeConnect breeze)
        {
            var datsa = System.Text.Json.JsonSerializer.Deserialize<QuotesData>(System.Text.Json.JsonSerializer.Serialize(breeze.getQuotes(
                Stockcode, "NFO".ToString(),
                expdate.ToString(), "Options".ToString(), "Others".ToString(), "")));

            var spotprice = datsa.Success.FirstOrDefault().spot_price;

            var spotpriceselection = round_down(Convert.ToDouble(spotprice), 100);
            return spotpriceselection;
        }
    }
}