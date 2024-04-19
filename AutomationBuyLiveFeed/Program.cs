using Breeze;
using Microsoft.AspNetCore.SignalR.Client;

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;


//.Net Core 3.1
namespace ConsoleAppTestProject
{

    public class AppSettings
    {
        public string url { get; set; }
    }

    public class Equities
    {
        public string? msn_secid { get; set; }

        public string symbol { get; set; }
        public double? open { get; set; }
        public double? last { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? change { get; set; }
        public double? bPrice { get; set; }
        public int? bQty { get; set; }
        public double? sPrice { get; set; }
        public int? sQty { get; set; }
        public int? ltq { get; set; }
        public double? avgPrice { get; set; }
        public string quotes { get; set; }
        public int? ttq { get; set; }
        public int? totalBuyQt { get; set; }
        public int? totalSellQ { get; set; }
        public string ttv { get; set; }
        public string trend { get; set; }
        public double? lowerCktLm { get; set; }
        public double? upperCktLm { get; set; }
        public string ltt { get; set; }
        public double? close { get; set; }
        public string exchange { get; set; }
        public string stock_name { get; set; }
        public string VolumeC { set; get; }
        public string OI { get; set; }
        public string CHNGOI { get; set; }
        public string product_type { get; set; }
        public string expiry_date { get; set; }
        public string strike_price { get; set; }
        public string right { get; set; }
        public string SecurityId { get; set; }
    }

    public class Program
    {


        static async Task Main(string[] args)
        {
           
            var url = "http://localhost:99/breezeOperation";
            //Console.WriteLine(url);
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
                switch (0)
                {
                    case 0:
                        line = lines[0].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:8080/BreezeOperation";
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
                    case 4:
                        line = lines[4].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        HUbUrl = "http://localhost:49/BreezeOperation";
                        break;
                }
              //  Console.WriteLine(arg);

                await using var connection = new HubConnectionBuilder().WithUrl("http://localhost:8080/BreezeOperation").WithAutomaticReconnect().Build();
                Random r = new Random();
                Console.WriteLine(connection.ConnectionId);
                BreezeConnect breeze = new BreezeConnect(APIKEY);
                breeze.generateSessionAsPerVersion(APISecret, token);
                var responseObject = await breeze.wsConnectAsync();
                Console.WriteLine(JsonSerializer.Serialize(responseObject));

                await connection.StartAsync();
                // connection.On<List<string>>("SendGetBuyForAutomation_Auto", async param =>
                connection.On<Equities[]>("SendGetBuyForAutomation_Auto", async param =>
                {
                    Console.WriteLine("Count" + param.Length);
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Starting the stopwatch...");
                    stopwatch.Start();
                    foreach (var item in param.Select(x => x.symbol).Distinct().ToList())
                    {
                        try
                        {
                            await breeze.subscribeFeedsAsync(item.ToString());
                            Thread.Sleep(TimeSpan.FromMilliseconds(0.2));
                        }
                        catch (Exception ex)
                        {


                        }
                    }

                    stopwatch.Stop();
                    Console.WriteLine("Elapsed time: " + stopwatch.Elapsed);
                    stopwatch.Reset();

                });
                Console.WriteLine("Conection" + connection.ConnectionId);
                Console.WriteLine("GetBuyForAutomation_Auto" + Convert.ToInt16(arg));
                await connection.SendAsync("GetBuyForAutomation_Auto");
                


                breeze.ticker(async (data) =>
                {

                    try
                    {


                        if (connection.State == HubConnectionState.Connected)
                        {
                            // Console.WriteLine(JsonSerializer.Serialize(data));
                            // Console.WriteLine("Ticker Data:" + JsonSerializer.Serialize(data));
                            await connection.InvokeAsync("CaptureLiveDataForAutomation_Auto", JsonSerializer.Serialize(data));

                        }
                        else
                        {

                        }




                    }
                    catch (Exception)
                    {


                    }





                });

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