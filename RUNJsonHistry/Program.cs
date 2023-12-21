
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;

class Program
{


    static async Task Main(string[] args)
    {

       // string HUbUrl = "http://192.168.0.106:90/livefeedhub";
        string HUbUrl = "https://localhost:90/livefeedhub";
        try
        {


            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
            connection.KeepAliveInterval = TimeSpan.Zero;


            await connection.StartAsync();
            Console.WriteLine(connection.ToString());
            await connection.SendAsync("ExportStocksToJson");
            await connection.SendAsync("StocksDays");



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}