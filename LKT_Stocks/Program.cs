using MSNStocks;

namespace LKT_Stocks
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await API_MSN_Library.EquitiesStats();
        }
    }
}
