public class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            try
            {
                await MSNStocks.API_MSN_Library.LatestPublishedMSNTOP();
            }
            catch (Exception ex)
            {

               
            }
            //try
            //{
            //    await MSNStocks.API_MSN_Library.LatestPublished();
            //}
            //catch (Exception ex)
            //{

                
            //}

            //try
            //{
            //    await MSNStocks.API_MSN_Library.LatestPublishedMSN();
            //}
            //catch (Exception ex)
            //{

            //}


            // await   MSNStocks.API_MSN_Library.LatestPublishedAdvanced();
            // await MSNStocks.getInitStocksFromSECIDForBSEResults


        }
        catch (Exception ex)
        {

            throw;
        }


    }
}