
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;

class Program
{


    static async Task Main(string[] args)
    {

        // string HUbUrl = "http://192.168.0.106:90/livefeedhub";
        // string HUbUrl = "http://localhost:48/livefeedhub";

        string HUbUrl = "https://localhost:7189/livefeedhub";
        try
        {
          Console.WriteLine(HUbUrl);

            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
            connection.KeepAliveInterval = TimeSpan.FromMinutes(1);
            Thread.Sleep(2000);

            await connection.StartAsync();
            Console.WriteLine(connection.ConnectionId.ToString());
            await connection.SendAsync("SendAlertsUpperCKT");
            Task.WaitAll();
            

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}