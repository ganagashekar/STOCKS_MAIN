
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

//.Net Core 3.1
namespace ConsoleAppTestProject
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string HUbUrl = "https://localhost:7189/livefeedhub";
            // string HUbUrl = "http://localhost/StockSignalRServer/livefeedhub";
            try
            {
                //Initialize SDK

                GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);

                // Wait a maximum of 30 seconds after a transport connection is lost
                // before raising the Disconnected event to terminate the SignalR connection.
                GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);

                // For transports other than long polling, send a keepalive packet every
                // 10 seconds. 
                // This value must be no more than 1/3 of the DisconnectTimeout value.
                GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);
                await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();

                Random r = new Random();
                await connection.StartAsync();

                connection.Closed += async (exception) =>
                {
                    Console.WriteLine("Resrtaed");
                    await connection.StartAsync();
                };
                await connection.SendAsync("ExportStocksToJson");

               
                



                string s = "";
                while (s != "kill")
                    s = Console.ReadLine();


                Console.WriteLine("");



                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}