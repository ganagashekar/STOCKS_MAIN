
namespace NSEBlockDeals
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var t3 = MSNStocks.API_MSN_Library.Get52weekhigh();
            var t4 = MSNStocks.API_MSN_Library.Get52weeklow();
            var t2=MSNStocks.API_MSN_Library.getNSEBulkResults();
            var t1 = MSNStocks.API_MSN_Library.getNSEBlockResults();
    
            Task.WaitAll(t1, t2,t3,t4);
        }
    }
}
