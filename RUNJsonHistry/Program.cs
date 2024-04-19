
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;

class Program
{


    static async Task Main(string[] args)
    {

        string HUbUrl = "https://localhost:7189/livefeedhub";
       // string HUbUrl = "http://localhost:48/livefeedhub";
        try
        {
            Console.WriteLine(HUbUrl);

            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
            connection.KeepAliveInterval = TimeSpan.FromMinutes(5);


            await connection.StartAsync();
            Console.WriteLine(connection.ConnectionId.ToString());
            await connection.SendAsync("ExportStocksToJson");
            await connection.SendAsync("StocksDays");

           

            Task.WaitAll();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}