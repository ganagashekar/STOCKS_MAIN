using Breeze;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using STM_API.Model;
using System.Diagnostics;
using System.Text.Json;

class Program
{


    static async Task Main(string[] args)
    {



        var builder = new ConfigurationBuilder() // .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var url =config.GetSection("appSettings:url").Value;

        Console.WriteLine(url);

        var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\key.txt");

        string[] lines = text.Split(
new string[] { Environment.NewLine },
StringSplitOptions.None
);

        string HUbUrl = url;


        // string HUbUrl = "http://localhost/StockSignalRServer/livefeedhub";
        try
        {
            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().WithKeepAliveInterval(TimeSpan.FromMinutes(30)).Build();
            connection.KeepAliveInterval = TimeSpan.FromMinutes(30);
            connection.ServerTimeout.Add(TimeSpan.FromMinutes(120));

            Random r = new Random();
            await connection.StartAsync();

            Console.WriteLine(connection.ConnectionId);

            string APIKEY = "6N4Hj74vE@668970816zP9K307YZ58Ff";
            string APISecret = "iz1F27815220!290Ie8Ha459997J8376";
            string token = lines[0].ToString();
            //Initialize SDK
            //string[] line;
            //string arg = args[0];


            //switch (Convert.ToInt16(arg))
            //{
            //    case 0:
            //        line = lines[0].ToString().Split(',');
            //        APIKEY = line[0];
            //        APISecret = line[1];
            //        token = line[2];
            //        break;
            //    case 1:
            //        line = lines[1].ToString().Split(',');
            //        APIKEY = line[0];
            //        APISecret = line[1];
            //        token = line[2];
            //        break;
            //    case 2:
            //        line = lines[2].ToString().Split(',');
            //        APIKEY = line[0];
            //        APISecret = line[1];
            //        token = line[2];
            //        break;
            //    case 3:
            //        line = lines[3].ToString().Split(',');
            //        APIKEY = line[0];
            //        APISecret = line[1];
            //        token = line[2];
            //        break;
            //    case 4:
            //        line = lines[4].ToString().Split(',');
            //        APIKEY = line[0];
            //        APISecret = line[1];
            //        token = line[2];
            //        break;
            //}
            // Generate Session
            //Console.WriteLine(arg);
            BreezeConnect breeze = new BreezeConnect(APIKEY);
            //Console.WriteLine(args[1].ToString());
            breeze.generateSessionAsPerVersion(APISecret, token);

            // Connect to WebSocket
            var responseObject = await breeze.wsConnectAsync();
            Console.WriteLine(JsonSerializer.Serialize(responseObject));

            // breeze.subscribeFeedsAsync("NFO", "CNXBAN", "FUTURE", "29-Feb-24","0", "call",true,true);

           

            connection.Closed += async (exception) =>
            {
                await connection.StartAsync();
            };

            connection.On<List<string>>("SendGetAllStocksForLoadAutomation", async param =>
            {
                Console.WriteLine("Count" + param.Count);
                foreach (var item in param)
                {
                    Console.WriteLine(JsonSerializer.Serialize(breeze.subscribeFeedsAsync(item.ToString())));
                }

                // Callback to receive ticks.

            });

            await connection.SendAsync("GetAllStocksForLoadAutomation", 0);




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
                        await connection.StartAsync();
                    }



                }
                catch (Exception)
                {


                }





            });
            // Console.WriteLine(param);

            //await connection.On("SendAllStocks", data,()=> { })

            //await foreach (var date in connection.StreamAsync<DateTime>("CaptureLiveData"))
            //{y
            //    Console.WriteLine(date);
            //}

            // Console.WriteLine(JsonSerializer.Serialize(await breeze.subscribeFeedsAsync("1.1!539992")));


            //Callback to receive ticks.
            //breeze.ticker( async (data) =>
            //{
            //    if (connection.State == HubConnectionState.Connected)
            //    {
            //        Console.WriteLine("Ticker Data:" + JsonSerializer.Serialize(data));
            //        await connection.SendAsync("CaptureLiveData", JsonSerializer.Serialize(data)).ConfigureAwait(true);
            //        Thread.Sleep(50);
            //    }
            //});

            //// Subscribe stocks feeds
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.subscribeFeedsAsync((exchangeCode: "NFO", stockCode: "ICIBAN", productType: "options", expiryDate: "25-Aug-2022", strikePrice: "650", right: "Put", getExchangeQuotes: true, getMarketDepth: false))));

            // Subscribe stocks feeds by stock-token
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.subscribeFeedsAsync("4.1!49937", "5second")));

            Console.WriteLine("");

            // Subscribe order notification feeds to get order data
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.subscribeFeedsAsync(true)));

            // UnSubscribe order notification feeds
            // Console.WriteLine(JsonSerializer.Serialize(await breeze.unsubscribeFeedsAsync(true)));

            // Unsubscribe stocks feeds
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.unsubscribeFeedsAsync((exchangeCode: "NFO", stockCode: "ICIBAN", productType: "options", expiryDate: "25-Aug-2022", strikePrice: "650", right: "Put", getExchangeQuotes: true, getMarketDepth: false))));

            //// Unsubscribe stocks feeds by stock-token
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.unsubscribeFeedsAsync("4.1!49937")));

            //// subscribe to oneclick strategy
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.subscribeFeedsAsync("one_click_fno", true)));

            //// unsubscribe to oneclick strategy
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.unsubscribeFeedsAsync("one_click_fno", true)));

            //// subscribe to ohlc streaming
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.subscribeFeedsAsync("1.1!SENSEX", "1second")));

            //// unsubscribe to ohlc streaming
            //Console.WriteLine(JsonSerializer.Serialize(await breeze.unsubscribeFeedsAsync("1.1!SENSEX", "1second")));

            Console.ReadLine();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}