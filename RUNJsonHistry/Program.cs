
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;

class Program
{


    static async Task Main(string[] args)
    {

        //string HUbUrl = "https://localhost:7189/livefeedhub";
        string HUbUrl = "http://localhost:45/livefeedhub";
        try
        {


            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
            connection.KeepAliveInterval = TimeSpan.FromMinutes(5);


            await connection.StartAsync();
            Console.WriteLine(connection.ToString());
            await connection.SendAsync("ExportStocksToJson");
            await connection.SendAsync("StocksDays");

            Thread.Sleep(500);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}