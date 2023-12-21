using STM_API.Model;
using System.Data.SqlClient;
using System.Data;
using STM_API.Model.BreezeAPIModel;
using Newtonsoft.Json;

namespace STM_API.Services
{
    public class BreezapiServices
    {

        public PortfolioPositions GetPositions()
        {
            string JsonString = "";

            try
            {
                
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("SELECT *  FROM [STOCK].[dbo].[Portfolio_Positions]", conn);
                    sqlComm.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    da.Fill(ds);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                   JsonString = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return JsonConvert.DeserializeObject<PortfolioPositions>(JsonString);
        }

        internal void SetBuyPriceAlter(string symbol, double price)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                SqlCommand sqlComm = new SqlCommand("AddOrUpdateStockPriceConfig", conn);
                sqlComm.Parameters.AddWithValue("@Symbol", (symbol));
                sqlComm.Parameters.AddWithValue("@Price", price);
                sqlComm.CommandType = CommandType.StoredProcedure;
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
