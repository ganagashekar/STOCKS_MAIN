
namespace EquitiesStats
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await MSNStocks.API_MSN_Library.GetEquitiesStats();
        }
    }
}
