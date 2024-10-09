using MSNStocks;

namespace NIFTYTraders
{
    public class Program
    {
        static void Main(string[] args)

        {
            var txt = System.IO.File.ReadAllText(@"C:\Hosts\ICICI_Key\NIftyTrader.txt").Split(Environment.NewLine);

            var nify = txt[0].Split(",");
            var result = API_MSN_Library.GenerateNiftyTraderAsync(nify[1], nify[0]);
            var bnify = txt[1].Split(",");
            var result2 = API_MSN_Library.GenerateNiftyTraderAsync(bnify[1], bnify[0]);
            Task.WaitAll(result, result2);


        }
    }
}
