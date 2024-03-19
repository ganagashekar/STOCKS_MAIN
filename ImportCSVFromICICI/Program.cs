using System.Data.SqlClient;
using System.Data;

namespace ImportCSVFromICICI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string result = client.GetStringAsync("https://traderweb.icicidirect.com/Content/File/txtFile/ScripFile/StockScriptNew.csv").Result;
            var lines = result.Split(Environment.NewLine);
            if (lines.Count() == 0) return;
            var columns = lines[0].Split(',');
            var table = new DataTable();
            foreach (var c in columns)
                table.Columns.Add(c);

            for (int i = 1; i < lines.Count() - 1; i++)
                table.Rows.Add(lines[i].Split(','));

            Console.WriteLine(table.Rows.Count);

            //var connection = @"your connection string";
            //var sqlBulk = new SqlBulkCopy(connection);
            //sqlBulk.DestinationTableName = "Table1";
            //sqlBulk.WriteToServer(table);

            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))

            {
                using (SqlCommand cmd = new SqlCommand("TruncateBeforeICICILoad", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = txtFirstName.Text;
                    //cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = txtLastName.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("TruncateBeforeICICILoad");

            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                var bc = new System.Data.SqlClient.SqlBulkCopy(conn, System.Data.SqlClient.SqlBulkCopyOptions.TableLock, null)
                {
                    DestinationTableName = "dbo.StockScriptNew",
                    BatchSize = table.Rows.Count
                };
                conn.Open();
                bc.WriteToServer(table);

                conn.Close();
                bc.Close();
            }
            Console.WriteLine("StockScriptNew");
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))

            {
                using (SqlCommand cmd = new SqlCommand("InsertFromScriptnew", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = txtFirstName.Text;
                    //cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = txtLastName.Text;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("InsertFromScriptnew");
        }
    }
}
