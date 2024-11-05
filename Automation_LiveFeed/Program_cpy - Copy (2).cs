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

//.Net Core 3.1
namespace ConsoleAppTestProject
{

    public class AppSettings
    {
        public string url { get; set; }
    }

    public class Program
    {


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


                try
                {
                    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(await breeze.subscribeFeedsAsync(
                           /* exchangeCode: */"NFO",
                           /* stockCode:*/ "NIFTY",
                           /* productType:*/ "options",
                           /* expiryDate: */ "07-Nov-2024",
                           /* strikePrice: */ "24000",
                           /* right: */ "Put",
                           /* getExchangeQuotes:*/ true,
                           /* getMarketDepth: */ false)
                       ));

                    var datas = await breeze.subscribeFeedsAsync("4.1!50381");
                    breeze.ticker((data) =>
                    {
                        Console.WriteLine("Ticker Data:" + System.Text.Json.JsonSerializer.Serialize(data));
                    });

                    var datsa = System.Text.Json.JsonSerializer.Deserialize<QuotesData>(System.Text.Json.JsonSerializer.Serialize(breeze.getQuotes("NIFTY", "NFO".ToString(),
                        "2024-11-07", "Options".ToString(), "Others".ToString(), "")));

                    var spotprice = datsa.Success.FirstOrDefault().spot_price;

                    var spotpriceselection = round_down(Convert.ToDouble(spotprice), 100);
                    //var data = await breeze.subscribeFeedsAsync(
                    //          exchangeCode: "NFO",
                    //          stockCode: "NIFTY",
                    //          productType: "Options",
                    //          expiryDate: datsa.Success.FirstOrDefault().expiry_date.ToString(),
                    //          strikePrice: spotpriceselection.ToString(),
                    //          right: "call",
                    //          getExchangeQuotes: true,
                    //         getMarketDepth: false
                    //          );

                   
                }
                catch (Exception ex)
                {

                    throw;
                }
               
                connection.On<List<string>>("SendGetTickDataForOptions", async param =>
                {
                    //var results = GetQuote(APISecret, token, data.Success.session_token, APIKEY, "NIFTY", "NFO", "2024-10-31", "options", "call");

                    var datsa = System.Text.Json.JsonSerializer.Deserialize<QuotesData>(System.Text.Json.JsonSerializer.Serialize(breeze.getQuotes(param[0].ToString(), "NFO".ToString(),
                        param[1].ToString(), "Options".ToString(), "Others".ToString(), "")));

                    var spotprice = datsa.Success.FirstOrDefault().spot_price;

                    var spotpriceselection =round_down(Convert.ToDouble(spotprice), 100);

                    //await breeze.subscribeFeedsAsync(
                    //     exchangeCode: "NFO",
                    //     stockCode: param[0].ToString(),
                    //     productType: "options",
                    //     expiryDate: datsa.Success.FirstOrDefault().expiry_date.ToString(),
                    //     strikePrice: spotpriceselection.ToString(),
                    //     right: "Call",
                    //     getExchangeQuotes: true,
                    //     getMarketDepth: true);

                    var datas=await breeze.subscribeFeedsAsync("4.2!50374");
                    //var data=await breeze.subscribeFeedsAsync(
                    //     exchangeCode: "NFO",
                    //     stockCode: param[0].ToString(),
                    //     productType: "Options",
                    //     expiryDate: "2024-11-07",
                    //     strikePrice: "24000",
                    //     right: "Call",true,true
                    //     );



                });

                try
                {
                    breeze.ticker(async (data) =>
                            {

                                try
                                {

                                    // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(data));
                                    if (connection.State == HubConnectionState.Connected)
                                    {
                                        //Console.WriteLine(JsonSerializer.Serialize(data));
                                        // Console.WriteLine("Ticker Data:" + System.Text.Json.JsonSerializer.Serialize(data));
                                        await connection.InvokeAsync("GetTickDataOption", System.Text.Json.JsonSerializer.Serialize(data));
                                    }

                                }






                                catch (Exception)
                                {


                                }






                            });
                }
                catch (Exception)
                {

                    throw;
                }

            


                //var data = await breeze.subscribeFeedsAsync(
                //     exchangeCode: "NFO",
                //     stockCode: "NIFTY",
                //     productType: "options",
                //     expiryDate: datsa.Success.FirstOrDefault().expiry_date.ToString(),
                //     strikePrice: spotpriceselection.ToString(),
                //     right: "Call", false, false
                //     );
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
    }
}