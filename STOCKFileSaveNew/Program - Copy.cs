using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).UseWindowsService().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            });


}

public class Worker : BackgroundService
{

    public async Task STATAsync()
    {
        string HUbUrl = "http://127.0.0.1:5000/livefeedhub";
        // string HUbUrl = "http://localhost/StockSignalRServer/livefeedhub";
        try
        {
            //Initialize SDK


            await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
            connection.KeepAliveInterval = TimeSpan.Zero;
            await connection.StartAsync();
            // connection.ServerTimeout.Add(TimeSpan.FromMinutes(120));

            StringBuilder stringBuilder = new StringBuilder();
            List<dynamic> stock = new List<dynamic>();

            connection.On<string>("SendLiveData", param =>
            {
                Equities livedata = JsonSerializer.Deserialize<Equities>(param);

                string filename = @"C:\Users\Haadv\source\repos\STM\STM_API\STM_API\STOCKFileSaveNew\Livedata\" + DateTime.Now.Date + ".txt";
                var count = Regex.Matches(stringBuilder.ToString(), Environment.NewLine).Count();
                stringBuilder.AppendLine(livedata.ToString());

                if (count >= 1000)
                {
                    File.AppendAllLines(filename, new[] { stringBuilder.ToString() });
                    stringBuilder.Length = 0;
                    stringBuilder.Capacity = 0;
                    Thread.Sleep(5000);
                    //System.IO.File.WriteAllText(filename, "");
                }
                //File.AppendAllLines(filename, new[] { livedata.ToString() });
                //dynamic livedata = JsonSerializer.Deserialize<dynamic>(param);


                // Callback to receive ticks.

            });

            Stopwatch s = new Stopwatch();
            s.Start();



            Console.ReadLine();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await STATAsync();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}