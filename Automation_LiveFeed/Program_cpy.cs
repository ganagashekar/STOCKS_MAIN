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

//.Net Core 3.1
namespace ConsoleAppTestProject
{

    public class AppSettings
    {
        public string url { get; set; }
    }

    public class Program
    {


        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            var url = "http://localhost:45/breezeOperation";
            Console.WriteLine(url);
            var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string HUbUrl = url;
            await using var connection = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();
            Random r = new Random();
            Console.WriteLine(connection.ConnectionId);
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
                        break;
                    case 1:
                        line = lines[1].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 2:
                        line = lines[2].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 3:
                        line = lines[3].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 4:
                        line = lines[4].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                }
                Console.WriteLine(arg);
                BreezeConnect breeze = new BreezeConnect(APIKEY);
                breeze.generateSessionAsPerVersion(APISecret, token);
                var responseObject = await breeze.wsConnectAsync();
                Console.WriteLine(JsonSerializer.Serialize(responseObject));

                await connection.StartAsync();
                Console.WriteLine("Conection" + connection.ConnectionId);
                Console.WriteLine("GetAllStocksForLoadAutomation" + Convert.ToInt16(arg));
                await connection.SendAsync("GetAllStocksForLoadAutomation", Convert.ToInt16(arg));
                connection.On<List<string>>("SendGetAllStocksForLoadAutomation", async param =>
                {
                    Console.WriteLine("Count" + param.Count);
                    Stopwatch stopwatch = new Stopwatch();
                    Console.WriteLine("Starting the stopwatch...");
                    stopwatch.Start();
                    foreach (var item in param.Select(x => x).Distinct().ToList())
                    {
                        try
                        {
                            await breeze.subscribeFeedsAsync(item.ToString());
                            Thread.Sleep(TimeSpan.FromMilliseconds(0.5));
                        }
                        catch (Exception ex)
                        {


                        }
                    }

                    stopwatch.Stop();
                    Console.WriteLine("Elapsed time: " + stopwatch.Elapsed);
                    stopwatch.Reset();

                });


                breeze.ticker(async (data) =>
                {

                    try
                    {


                        if (connection.State == HubConnectionState.Connected)
                        {
                            // Console.WriteLine(JsonSerializer.Serialize(data));
                            // Console.WriteLine("Ticker Data:" + JsonSerializer.Serialize(data));
                            await connection.InvokeAsync("CaptureLiveDataForAutomation", JsonSerializer.Serialize(data));

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