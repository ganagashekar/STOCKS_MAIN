﻿using STM_API.Model;
using System.Data.SqlClient;
using System.Data;
using STM_API.Model.BreezeAPIModel;
using Newtonsoft.Json;
using STM_API.AutomationModel;

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

        public IEnumerable<BuyStockAlertModel> GetBuyStockTriggers()
        {

            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("GetBuyStockTriggers", conn);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            try
            {
                List<BuyStockAlertModel> items = ds.Tables[0].AsEnumerable().Select(row =>
                   new BuyStockAlertModel
                   {
                       buyATPrice =Convert.ToDecimal(row.Field<decimal>("buyATPrice")),
                       buyATChange = Convert.ToDecimal(row.Field<decimal>("buyATChange")),
                       sellATPrice = row.Field<decimal>("sellAtPrice"),
                       symbol = row.Field<string>("Symbol"),
                       stockName= row.Field<string>("StockName"),
                       stockCode= row.Field<string>("StockCode"),
                   }).ToList();
                return items.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
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

        internal void SetBuyChangeAlter(string symbol, string price)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                SqlCommand sqlComm = new SqlCommand("AddOrUpdateStockChangeConfig", conn);
                sqlComm.Parameters.AddWithValue("@Symbol", (symbol));
                sqlComm.Parameters.AddWithValue("@Change", price);
                sqlComm.CommandType = CommandType.StoredProcedure;
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }
        }

        internal void BuyOrSellEquity(string symbol, int quanity, string exchange, string marketor_Limit, decimal buyprice, decimal stoploss, string stockcode, string buysell)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    SqlCommand sqlComm = new SqlCommand("InsertAUTO_BUY_EQUTIES", conn);
                    sqlComm.Parameters.AddWithValue("@StockCode", stockcode);
                    sqlComm.Parameters.AddWithValue("@Symbol", (symbol));
                    sqlComm.Parameters.AddWithValue("@quanity", quanity);
                    sqlComm.Parameters.AddWithValue("@marketor_Limit", (marketor_Limit));
                    sqlComm.Parameters.AddWithValue("@buyprice", buyprice);
                    sqlComm.Parameters.AddWithValue("@stoploss", (stoploss));
                    
                    sqlComm.Parameters.AddWithValue("@BuyorSell", (buysell));
                    sqlComm.Parameters.AddWithValue("@exchange", (exchange));
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal void SetForT3(string symbol, string change)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                SqlCommand sqlComm = new SqlCommand("AddOrUpdateForT3", conn);
                sqlComm.Parameters.AddWithValue("@Symbol", (symbol));
                sqlComm.Parameters.AddWithValue("@Value", change);
                sqlComm.CommandType = CommandType.StoredProcedure;
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }
        }

        internal void ExportAutomationLiveStocksToJson()
        {
            try
            {
                var results = GetExportAutomationLiveStocksToJson();


                var chubnkresulst = results.Chunk<Equities>(100000);
                int i = 0;
                foreach (var item in chubnkresulst)
                {
                    var json = JsonConvert.SerializeObject(item);
                    System.IO.File.WriteAllText(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoksForAutomation", i), json);
                    i++;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private List<Equities> GetExportAutomationLiveStocksToJson()
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("Select Symbol from StockPriceConfig", conn);

                sqlComm.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            try
            {
                List<Equities> items = ds.Tables[0].AsEnumerable().Select(row => new Equities
                {
                    symbol = row.Field<string>("symbol")
                }).ToList();
                return items.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

   
            public IEnumerable<Equities> SendAllStocksForLoad()
            {

                try
                {



                    //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                    List<Equities> stocks = new List<Equities>();
                    DataSet ds = new DataSet();

                    using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                    {

                        //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                        SqlCommand sqlComm = new SqlCommand("SP_GET_LIVE_STOCKS_BY_STOCK_LOAD", conn);
                        sqlComm.Parameters.AddWithValue("@Code", DBNull.Value);
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = sqlComm;

                        da.Fill(ds);
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var NewStock = ds.Tables[0].Rows.Cast<DataRow>();

                        foreach (var r in NewStock)
                        {
                            try
                            {
                                var _stokc = new Equities();
                                _stokc.symbol = r[0].ToString();
                                _stokc.open = Convert.ToDouble(r[1].ToString());
                                _stokc.last = Convert.ToDouble(r[2].ToString());
                                _stokc.high = Convert.ToDouble(r[3].ToString());
                                _stokc.low = Convert.ToDouble(r[4].ToString());
                                _stokc.change = Convert.ToDouble(r[5].ToString());
                                _stokc.bPrice = Convert.ToDouble(r[6].ToString());
                                _stokc.totalBuyQt = Convert.ToInt32(r[7]);
                                _stokc.sPrice = Convert.ToDouble(r[8].ToString());
                                _stokc.totalSellQ = Convert.ToInt32(r[9]);
                                _stokc.avgPrice = Convert.ToDouble(r[11].ToString());
                                _stokc.ttv = (r[16].ToString());//.Replace("L", "100000").Replace("C", "1000000"));
                                _stokc.lowerCktLm = Convert.ToDouble(r[18].ToString());
                                _stokc.upperCktLm = Convert.ToDouble(r[19].ToString());
                                _stokc.ltt = r[20].ToString();
                                _stokc.close = Convert.ToDouble(r[21].ToString());
                                _stokc.stock_name = r[23].ToString();
                                //_stokc.Week_min = !string.IsNullOrEmpty(r[25].ToString()) ? Convert.Todouble(r[25]) : default(double?);
                                //_stokc.Week_max = !string.IsNullOrEmpty(r[26].ToString()) ? Convert.Todouble(r[26]) : default(double?);
                                //_stokc.TwoWeeks_min = !string.IsNullOrEmpty(r[27].ToString()) ? Convert.Todouble(r[27]) : default(double?);
                                //_stokc.TwoWeeks_max = !string.IsNullOrEmpty(r[28].ToString()) ? Convert.Todouble(r[28]) : default(double?);
                                //_stokc.Month_min = !string.IsNullOrEmpty(r[29].ToString()) ? Convert.Todouble(r[29]) : default(double?);
                                //_stokc.Month_max = !string.IsNullOrEmpty(r[30].ToString()) ? Convert.Todouble(r[30]) : default(double?);
                                //_stokc.Three_month_min = !string.IsNullOrEmpty(r[31].ToString()) ? Convert.Todouble(r[31]) : default(double?);
                                //_stokc.Three_month_max = !string.IsNullOrEmpty(r[32].ToString()) ? Convert.Todouble(r[32]) : default(double?);
                                stocks.Add(_stokc);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        }

                        return stocks;//.Where(X => X.Open <= 120);
                    }


                }
                catch (Exception ex)
                {

                    throw;

                }
                return Enumerable.Empty<Equities>();

            }

        internal void SetForWacthList(string symbol, string change)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                SqlCommand sqlComm = new SqlCommand("AddOrUpdateForWatchList", conn);
                sqlComm.Parameters.AddWithValue("@Symbol", (symbol));
                sqlComm.Parameters.AddWithValue("@Value", change);
                sqlComm.CommandType = CommandType.StoredProcedure;
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
