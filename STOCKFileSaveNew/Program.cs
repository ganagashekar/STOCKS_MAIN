using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;

class Program
{
    

    //https://localhost:5001
   static StringBuilder stringBuilder = new StringBuilder();
  
    static async Task Main(string[] args)
    {

        var builder = new ConfigurationBuilder() // .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var HUbUrl = config.GetSection("appSettings:url").Value;

        Console.WriteLine(HUbUrl);

        string filename = @"C:\Hosts\Files\" + DateTime.Now.Date.ToShortDateString() + ".txt";
        if (!File.Exists(filename))
        {
            System.IO.File.WriteAllText(filename, Environment.NewLine);
        }

        await using var connection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
        connection.KeepAliveInterval = TimeSpan.Zero;
        await connection.StartAsync();

        connection.On<string>("SendLiveData", async param => {

           await  RunFileSave(param);
        });


        string s = "";
        while (s != "kill")
            s = Console.ReadLine();

    }

    private static async Task RunFileSave(string param)
    {
        try
        {
            //Initialize SDK


         
            // connection.ServerTimeout.Add(TimeSpan.FromMinutes(120));

            
          

           
                Equities livedata = JsonSerializer.Deserialize<Equities>(param);

                string filename = @"C:\Hosts\Files\" + DateTime.Now.Date.ToShortDateString() + ".txt";
                var count = Regex.Matches(stringBuilder.ToString(), Environment.NewLine).Count();
                stringBuilder.AppendLine(livedata.ToString());

                if (count >= 600)
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

            

            

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
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
    public class Equities
    {
        public string symbol { get; set; }
        public double? open { get; set; }
        public double? last { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? change { get; set; }
        public double? bPrice { get; set; }
        public int bQty { get; set; }
        public double? sPrice { get; set; }
        public int sQty { get; set; }
        public int ltq { get; set; }
        public double? avgPrice { get; set; }
        public string quotes { get; set; }
        public int ttq { get; set; }
        public int totalBuyQt { get; set; }
        public int totalSellQ { get; set; }
        public string ttv { get; set; }
        public string trend { get; set; }
        public double? lowerCktLm { get; set; }
        public double? upperCktLm { get; set; }
        public string ltt { get; set; }
        public double? close { get; set; }
        public string exchange { get; set; }
        public string stock_name { get; set; }

        public string volumeC { get; set; }




        public override string ToString()
        {
            const DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces;


            //, CultureInfo.InvariantCulture,
            //               style, out var dt) ? dt : null as DateTime?;
            string[] test = this.ltt.Split(' ');
            string dateformat = string.Format("{0}-{1}-{2} {3}", test.Last(), test[1].ToString(), test[2].ToString(), test[3].ToString());
            var result = DateTime.TryParse(dateformat, out var dt);
            return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}", this.symbol, this.open, this.last, this.high, this.low, this.change, this.bPrice, this.bQty, this.sPrice, this.sQty, this.ltq, this.avgPrice, this.quotes, this.ttq, this.totalBuyQt, this.totalSellQ, this.ttv, this.trend, this.lowerCktLm, this.upperCktLm, dateformat, this.close, this.exchange, this.stock_name,this.volumeC);
        }
    }
}