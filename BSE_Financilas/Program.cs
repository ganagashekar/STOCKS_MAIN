public class Program
{
    static async Task Main(string[] args)
    {
        try
        {
           
            await MSNStocks.API_MSN_Library.getInitStocksFromSECIDForBSEResults();
           


        }
        catch (Exception ex)
        {

            throw;
        }


    }
}