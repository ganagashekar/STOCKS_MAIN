using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace StockCKT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<ListOfCKT> listOfCKTs = new List<ListOfCKT>();

            var data = GetDataFromDB();
            var results = data.ToList().GroupBy(x => x.symbol).ToList();
            foreach (var item in results)
            {
                ListOfCKT listOfCKT = new ListOfCKT();
                listOfCKT.Symbol = item.Key.ToString();
                listOfCKT.IsInLowerCKT = false;
                listOfCKT.Stock_name = item.FirstOrDefault().stock_name;
                int upperCktLm = 0;
                int lowerCktLm = 0;
                var accessData = item.OrderByDescending(x => x.LTT).Take(10).ToList().OrderBy(x => x.LTT).ToList();
                foreach (var item1 in accessData.OrderByDescending(x => x.LTT))
                {
                    if (item1.CurrentCHG == "upperCktLm")
                    {
                        upperCktLm = upperCktLm + 1;
                        if (upperCktLm > 1)
                        {
                            listOfCKT.IsInLowerCKT = false;
                            break;
                        }


                    }
                    else
                    {
                        lowerCktLm = lowerCktLm + 1;
                        listOfCKT.IsInLowerCKT = true;
                    }
                }
                listOfCKT.DaysUPR_CKT = upperCktLm;
                listOfCKT.DaysLWR_CKT = lowerCktLm;
                listOfCKT.DifferenceOfLWR_TO_UpperCKT = lowerCktLm - upperCktLm;
                listOfCKT.last = item.FirstOrDefault().last;
                listOfCKT.ltt = item.FirstOrDefault().LTT;
                listOfCKT.TTV = item.FirstOrDefault().ttv;
                listOfCKT.TTVC = item.FirstOrDefault().VolumeC;
                listOfCKTs.Add(listOfCKT);
                Console.WriteLine(listOfCKT.Symbol);
            }

            int i = 0;


            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                conn.Open();

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("Truncate table STOCK_CKT_DETAILS", conn);

                sqlComm.CommandType = CommandType.Text;
                sqlComm.ExecuteNonQuery();
                conn.Close();
            }

            var dt = ToDataTable(listOfCKTs);



            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                {
                    DestinationTableName = "dbo.STOCK_CKT_DETAILS",
                    BatchSize = dt.Rows.Count
                };
                conn.Open();
                bc.WriteToServer(dt);
                conn.Close();
                bc.Close();
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static List<STOCK_CKT> GetDataFromDB()
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("STOCK_CKT", conn);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            try
            {
                List<STOCK_CKT> items = ds.Tables[0].AsEnumerable().Select(row =>
                   new STOCK_CKT
                   {
                       CurrentCHG = (row.Field<string>("CurrentCHG")),
                       IsSame = (row.Field<int>("IsSame")),
                       last = row.Field<decimal>("last"),
                       LTT = row.Field<DateTime>("LTT"),
                       Previous_Change = row.Field<string>("Previous_Change"),
                       stock_name = row.Field<string>("stock_name"),
                       ttq = Convert.ToDecimal(row.Field<decimal>("ttq")),
                       ttv = Convert.ToDecimal(row.Field<double>("ttv")),
                       signal = row.Field<int>("signal"),
                       VolumeC = row.Field<string>("VolumeC"),
                       symbol = row.Field<string>("symbol"),

                   }).ToList();
                return items.ToList();
            }

            catch (Exception ex)
            {
                return new List<STOCK_CKT> { };
            }
        }
    }

}
