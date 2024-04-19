public class Program
{
    static async Task Main(string[] args)
    {
        try
        {
           
            await MSNStocks.API_MSN_Library.getInitStocksFromSECIDForNSEResults();
           // await MSNStocks.getInitStocksFromSECIDForBSEResults


        }
        catch (Exception ex)
        {

            throw;
        }


    }
}