public class Program
{
    static async Task Main(string[] args)
    {
        try
        {

            await MSNStocks.API_MSN_Library.LatestPublished();
         // await   MSNStocks.API_MSN_Library.LatestPublishedAdvanced();
            // await MSNStocks.getInitStocksFromSECIDForBSEResults


        }
        catch (Exception ex)
        {

            throw;
        }


    }
}