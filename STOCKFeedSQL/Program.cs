
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;

class Program
{
    

    private static void ReadFile(string filePath)
    {
        const int MAX_BUFFER = 100; //1MB 
        byte[] buffer = new byte[MAX_BUFFER];
        int bytesRead;
        int noOfFiles = 0;
        using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
        using (BufferedStream bs = new BufferedStream(fs))
        {
            while ((bytesRead = bs.Read(buffer, 0, MAX_BUFFER)) != 0) //reading 1mb chunks at a time
            {
                noOfFiles++;
               
                File.WriteAllBytes($"Test/Test{noOfFiles}.txt", buffer);
            }
        }
    }
    static async Task Main(string[] args)
    {

        string HUbUrl = "http://127.0.0.1:5000/livefeedhub";
        // string HUbUrl = "http://localhost/StockSignalRServer/livefeedhub";
        try
        {


            string filename = @"C:\Users\Haadv\source\repos\STM\STM_API\STM_API\STOCKFileSaveNew\Livedata\" + DateTime.Now.Date + ".txt";

            ReadFile(filename);


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
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
        public DateTime ltt { get; set; }
        public double? close { get; set; }
        public string exchange { get; set; }
        public string stock_name { get; set; }

        //public override string ToString()
        //{
        //    const DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces;


        //    //, CultureInfo.InvariantCulture,
        //    //               style, out var dt) ? dt : null as DateTime?;
        //    string[] test = this.ltt.Split(' ');
        //    string dateformat = string.Format("{0}-{1}-{2} {3}", test.Last(), test[1].ToString(), test[3].ToString(), test[4].ToString());
        //    var result = DateTime.TryParse(dateformat, out var dt);
        //    return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}", this.symbol, this.open, this.last, this.high, this.low, this.change, this.bPrice, this.bQty, this.sPrice, this.sQty, this.ltq, this.avgPrice, this.quotes, this.ttq, this.totalBuyQt, this.totalSellQ, this.ttv, this.trend, this.lowerCktLm, this.upperCktLm, dt, this.close, this.exchange, this.stock_name);
        //}
    }
}