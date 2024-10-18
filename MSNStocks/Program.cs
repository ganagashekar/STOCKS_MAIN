using MSNStocks;

public class Program
{
    static async Task Main(string[] args)
    {
        try
        {


            //await API_MSN_Library.getInitStocks();
           // await API_MSN_Library.InsertFromMicrosoft();
            await API_MSN_Library.getInitStocksFromSECID();
            await API_MSN_Library.InsertMMSCompanies();
            await API_MSN_Library.UpdownStcoks();
            



        }
        catch (Exception ex)
        {

            throw;
        }


    }
}