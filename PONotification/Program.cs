
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text.Json;

class Program
{


    static async Task Main(string[] args)
    {

       // string HUbUrl = "http://192.168.0.106:90/livefeedhub";
        string HUbUrl = "http://localhost:90/livefeedhub";
        try
        {
          

            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
            connection.KeepAliveInterval = TimeSpan.FromMinutes(1);


            await connection.StartAsync();
            Console.WriteLine(connection.ToString());
            await connection.SendAsync("SendAlertsUpperCKT");
            Thread.Sleep(10000);
            

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}