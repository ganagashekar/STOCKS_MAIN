using Breeze;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    static async Task Main(string[] args)
    {



        //var builder = new ConfigurationBuilder() // .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        //   .SetBasePath(Directory.GetCurrentDirectory())
        //   .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);

       // IConfiguration config = builder.Build();

        var url = "http://localhost:8080/BreezeOperation"; ;/// config.GetSection("appSettings:url").Value;

        Console.WriteLine(url);

        var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");

        string[] lines = text.Split(
    new string[] { Environment.NewLine },
    StringSplitOptions.None
    );

        string HUbUrl = url;

        await using var connection = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();
        //connection.KeepAliveInterval = TimeSpan.FromSeconds(10);
        //connection.ServerTimeout.Add(TimeSpan.FromMinutes(120));

        Random r = new Random();

        Console.WriteLine(connection.ConnectionId);
        // string HUbUrl = "http://localhost/StockSignalRServer/livefeedhub";
        try
        {
            string APIKEY = string.Empty;
            string APISecret = string.Empty;
            string token = string.Empty;
            //Initialize SDK
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
            // Generate Session
            Console.WriteLine(arg);
            BreezeConnect breeze = new BreezeConnect(APIKEY);
            //Console.WriteLine(args[1].ToString());
            breeze.generateSessionAsPerVersion(APISecret, token);

            // Connect to WebSocket
            var responseObject = await breeze.wsConnectAsync();
            Console.WriteLine(JsonSerializer.Serialize(responseObject));

            await connection.StartAsync();
            //Console.WriteLine("GetAllStocksForLoad" + Convert.ToInt16(arg));
            //// await connection.SendAsync("GetAllStocksForLoad", Convert.ToInt16(arg));

            //await connection.SendAsync("GetAllStocksForLoadAll");
            breeze.subscribeFeedsAsync("4.1!51696");




            breeze.ticker(async (data) =>
            {

                try
                {


                    if (connection.State == HubConnectionState.Connected)
                    {
                        // Console.WriteLine(JsonSerializer.Serialize(data));
                        // Console.WriteLine("Ticker Data:" + JsonSerializer.Serialize(data));
                        await connection.InvokeAsync("CaptureLiveDataForBuyForAutomationNIFTYBANK", JsonSerializer.Serialize(data));

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