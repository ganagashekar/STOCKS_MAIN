using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STM_API.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace STM_API.Services
{
    public class StockTicker
    {





        public IEnumerable<STOCK_NTFCTN> GetNotification(bool IsUpper, bool IsSell)
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<STOCK_NTFCTN> stocks = new List<STOCK_NTFCTN>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("SEND_NOTIFICATION", conn);
                    sqlComm.Parameters.AddWithValue("@IsUpper", IsUpper);
                    sqlComm.Parameters.AddWithValue("@IsSell", IsSell);
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
                            var _stokc = new STOCK_NTFCTN();
                            _stokc.symbol = r[1].ToString();
                            _stokc.Date = Convert.ToDateTime(r[2].ToString());
                            
                            _stokc.STOCKName = Convert.ToString(r[3].ToString());
                            _stokc.IsNotified = Convert.ToBoolean(r[4].ToString());
                            _stokc.IsUppCKT = Convert.ToBoolean(r[5].ToString());
                            _stokc.ISSell = Convert.ToBoolean(r[6].ToString());
                            _stokc.ISPrict = Convert.ToBoolean(r[7].ToString());
                            _stokc.Change = Convert.ToDecimal(r[8].ToString());
                            _stokc.PO_KEY_NAME = Convert.ToString(r[12]);
                            _stokc.Last = Convert.ToDecimal(Convert.ToString(r[10].ToString()));
                            _stokc.PO_KEY_TOKEN = Convert.ToString(r[13]);
                            _stokc.user = Convert.ToString(r[15]);
                            _stokc.priority = Convert.ToInt16(r[16].ToString());
                            _stokc.title = Convert.ToString(r[17].ToString());
                            _stokc.retry = Convert.ToInt16(r[18].ToString());
                            _stokc.expire = Convert.ToInt16(r[19].ToString());
                            _stokc.sound = Convert.ToString(r[20].ToString());
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
            return Enumerable.Empty<STOCK_NTFCTN>();
        }

        public IEnumerable<Equities> SendAllStocksForLoad()
        {

            try
            {



                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        //public string ConvertDataTableToHTMLTableInOneLine(DataTable dt)
        //{
        //    //Convert DataTable To HTML Table in one line
        //    return "<table class='tablewithborder' cellpadding='2' cellspacing='0' style='border: solid 1px black;font-size: 9pt;font-family:Arial' width='100%'>\n<tr>" + string.Join("", dt.Columns.Cast<DataColumn>().Select(dc => "<td style='border: solid 1px black;'>" + dc.ColumnName + "</td>")) + "</tr>\n" +
        //    "<tr  style='border: solid 1px black;'>" + string.Join("</tr>\n<tr>", dt.AsEnumerable().Select((Value, Index) => new { Value, Index }).Select(row => "<td style='border: solid 1px black;'>" + string.Join("</td><td style='border: solid 1px black;'>", row.ItemArray) + "</td>").ToArray()) + "</tr>\n</table>";

        //}
        //public string ConvertDataTableToHTMLTableInOneLine(DataTable dt)
        //{
        //    //Convert DataTable To HTML Table in one line
        //    return "<table class='tablewithborder' cellpadding='2' cellspacing='0' style='border: solid 1px black;font-size: 9pt;font-family:Arial' width='100%'>\n<tr>" + string.Join("", dt.Columns.Cast<DataColumn>().Select(dc => "<td style='border: solid 1px black;'>" + dc.ColumnName + "</td>")) + "</tr>\n" +
        //    "<tr  style='border: solid 1px black;'>" + string.Join("</tr>\n<tr>", dt.AsEnumerable().Select((Value, Index) => new { Value, Index }).Select(row => "<td style='border: solid 1px black;'>" + string.Join("</td><td style='border: solid 1px black;'>", row.Value.ItemArray) + "</td>").ToArray()) + "</tr>\n</table>";

        //}

        private T[] GetArray<T>(IList<T> iList) where T : new()
        {
            var result = new T[iList.Count];

            iList.CopyTo(result, 0);

            return result;
        }

        public string ExportDatatableToHtml(DataTable dt, string Column)
        {
            dt.Columns.Remove("symbol");
            string bgcolor = "#fff";
            string grcolor = "";
            string pfcolor = "";
            StringBuilder strHTML = new StringBuilder();

            strHTML.Append("<table id= \"customers\"  class='tablewithborder' cellpadding='2' cellspacing='0' style='border: solid 1px black;font-size: 12pt;font-family:Arial' width='100%'>");
            strHTML.Append("<tr style='border: solid 1px'>");
            int k = 0;
            foreach (DataColumn myColumn in dt.Columns)
            {
                strHTML.Append("<th style='border: dotted 1px black;black;' ><b>");
                strHTML.Append(k == 0 ? "" : myColumn.ColumnName);
                strHTML.Append("</th></b>");
                k = k + 1;

            }
            strHTML.Append("<th style='border: dotted 1px black;black;' ><b>");
            strHTML.Append((dt.Rows.Count));
            strHTML.Append("</th></b>");
            strHTML.Append("</tr>");
            int i = 0;
            foreach (DataRow myRow in dt.Rows)
            {


                var ListTotalTradeVolume = myRow.ItemArray.Skip(5).Where(x => !string.IsNullOrEmpty(x.ToString())).ToList().Cast<decimal>().ToList();
                //var ListTotalTradeVolume = (IEnumerable<string>)dt.AsEnumerable().Select(r => r.ItemArray.Skip(4).Cast<string>()).ToList();

                var result = canMakeItCustom(ListTotalTradeVolume.Count(), (ListTotalTradeVolume).ToArray());
                string Truefalsecolor = result > 2 ? "#CCFFCD" : "#ffe8e7";
                int j = 0;
                bgcolor = (i % 2 == 0) ? "#EAEEE9" : "#FFFFFF";
                strHTML.Append("<tr style='border: dotted 1px black;background-color:" + bgcolor + "'>");
                foreach (DataColumn myColumn in dt.Columns)
                {

                    grcolor = "";

                    if (j == 3 && !string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                    {
                        grcolor = (Convert.ToDecimal(dt.Rows[i][j].ToString()) > 0) ? "#CCFFCD" : "#ffe8e7";
                    }

                    if (j > 5 && !string.IsNullOrEmpty(dt.Rows[i][j].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][j - 1].ToString()))
                    {
                        grcolor = (Convert.ToDecimal(dt.Rows[i][j].ToString()) > Convert.ToDecimal(dt.Rows[i][j - 1].ToString())) ? "#CCFFCD" : "#ffe8e7";
                    }
                    strHTML.Append("<td style='border: dotted 1px black;black;background-color:" + grcolor + "'' >");
                    if (j == 4)
                    {
                        string arrrowcolor = result > 2 ? "<span style='font-size:20px;black;color:green;text-align: right'>&#x2191</span>" : "<span style='font-size:20px;black;color:red;text-align: right'>&#8595</span>";

                        var anchortag = string.IsNullOrEmpty(dt.Rows[i][0].ToString()) ? myRow[myColumn.ColumnName] : string.Format("<a target=\"_blank\"  href=\"https://www.msn.com/en-in/money/stockdetails/fi-{0}?duration=5D\">{1}</a>", dt.Rows[i][0], myRow[myColumn.ColumnName].ToString());
                        strHTML.Append(j == 4 ? arrrowcolor + "<span style='background-color:" + Truefalsecolor + ";text-align: right'>" + result + "</span>&nbsp; &nbsp;  " + anchortag : (Column.ToLower() == "ttv" && j > 4) ? numDifferentiation(myRow[myColumn.ColumnName].ToString()) : myRow[myColumn.ColumnName].ToString());

                    }
                    else
                    {
                        strHTML.Append(j == 0 ? "" : (Column.ToLower() == "ttv" && j > 4) ? numDifferentiation(myRow[myColumn.ColumnName].ToString()) : myRow[myColumn.ColumnName].ToString());

                    }
                    strHTML.Append("</td>");

                    try
                    {
                        pfcolor = "";
                        if (dt.Columns.Count - 1 == j)
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][5].ToString()) && Convert.ToDecimal(dt.Rows[i][5].ToString()) > 0)
                            {

                                var val = ((Convert.ToDecimal(dt.Rows[i][j].ToString()) - Convert.ToDecimal(dt.Rows[i][5].ToString())) / Convert.ToDecimal(dt.Rows[i][5].ToString())) * 100;
                                pfcolor = val > 0 ? "#CCFFCD" : "#ffe8e7";

                                strHTML.Append("<td style='border: dotted 1px black;black;background-color:" + pfcolor + "'>" + Convert.ToDecimal(string.Format("{0:0.00}", val)) + "</td>");
                            }
                            else
                            {
                                strHTML.Append("<td style='border: solid 1px black;black;background-color:#fdff8f'></td>");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }



                    j = j + 1;
                }

                strHTML.Append("<td style='border: solid 1px black;black;background-color:" + Truefalsecolor + "'>" + result + "</td>");
                strHTML.Append("</tr>");
                i = i + 1;
            }
            strHTML.Append("</table>");


            string Htmltext = strHTML.ToString();
            return Htmltext;
        }
        static string numDifferentiation(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var val = Math.Abs(Convert.ToDecimal(value));
                    if (val >= 10000000) return Convert.ToDecimal((Convert.ToDecimal(value) / 10000000)).ToString("0.00") + "C";
                    if (val >= 100000) return Convert.ToDecimal((Convert.ToDecimal(value) / 100000)).ToString("0.00") + "L";

                    return Convert.ToDecimal((Convert.ToDecimal(value) / 100000)).ToString("0.00") + "L"; ;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static string ConvertDataTableToHTML(DataTable dt)
        {
            try
            {
                string bgcolor = "#fff";
                string grcolor = "";
                string pfcolor = "";
                string html = "<table class='tablewithborder' cellpadding='2' cellspacing='0' style='border: solid 1px black;font-size: 9pt;font-family:Arial' width='100%'>";
                //add header row
                html += "<tr  style='border: solid 1px black;'>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    html += i == 0 ? "<td style='border: solid 1px black;'>" + dt.Columns[i].ColumnName + "  " + dt.Rows.Count + " </td>" : "<td style='border: solid 1px black;'>" + dt.Columns[i].ColumnName + " </td>";

                }

                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bgcolor = (i % 2 == 0) ? "#EAEEE9" : "#FFFFFF";


                    html += "<tr style='border: solid 1px black;background-color:" + bgcolor + "'>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        grcolor = "";
                        if (j > 2 && !string.IsNullOrEmpty(dt.Rows[i][j].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][j - 1].ToString()))
                        {
                            grcolor = (Convert.ToDecimal(dt.Rows[i][j].ToString()) > Convert.ToDecimal(dt.Rows[i][j - 1].ToString())) ? "#CCFFCD" : "#ffe8e7";
                        }
                        html += "<td style='border: solid 1px black;black;background-color:" + grcolor + "''>" + numDifferentiation(dt.Rows[i][j].ToString()) + "</td>";
                        try
                        {
                            pfcolor = "";
                            if (dt.Columns.Count - 1 == j)
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][2].ToString()) && Convert.ToDecimal(dt.Rows[i][2].ToString()) > 0)
                                {

                                    var val = ((Convert.ToDecimal(dt.Rows[i][j].ToString()) - Convert.ToDecimal(dt.Rows[i][2].ToString())) / Convert.ToDecimal(dt.Rows[i][2].ToString())) * 100;
                                    pfcolor = val > 0 ? "#CCFFCD" : "#ffe8e7";

                                    html += "<td style='border: solid 1px black;black;background-color:" + pfcolor + "'>" + Convert.ToDecimal(string.Format("{0:0.00}", val)) + "</td>";
                                }
                                else
                                {
                                    html += "<td style='border: solid 1px black;black;background-color:#fdff8f'></td>";
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }



                    }
                    html += "</tr>";
                }
                html += "</table>";
                return html;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        int canMakeItCustom(int n, decimal[] ar)
        {
            int count = 0;


            for (int i = 1; i < n; i++)
            {
                decimal? previous = ar.ElementAtOrDefault(i - 1);
                decimal? current = ar.ElementAtOrDefault(i);
                //decimal? next = ar.ElementAtOrDefault(i + 1);
                if (current > previous) { if (count < 0) count = 0; count++; }
                else { if (count > 0) count = 0; count--; }
            }

            return count;
            //int k=0;
            //int i = 1;
            //if (n == 1)
            //    return 0;
            //foreach()
            //while (i < n && ar[(i) - 1] > ar[(i)])
            //{
            //    k--;
            //    i++;
            //}
            //int i = 1;
            //while (i < n && ar[(i + 1) - 1] < ar[(i + 1)])
            //{
            //    k++;
            //    i++;
            //}
            //return i;
        }

        int canMakeIt(int n, decimal[] ar)
        {
            // Base Case
            if (n == 1)
                return 0;
            else
            {

                // First subarray is
                // strictly increasing
                if (ar[0] < ar[1])
                {

                    int i = 1;

                    // Check for strictly
                    // increasing condition
                    // & find the break point
                    while (i < n && ar[i - 1] < ar[i])
                    {
                        i++;
                    }

                    // Check for strictly
                    // decreasing condition
                    // & find the break point
                    while (i + 1 < n && ar[i] > ar[i + 1])
                    {
                        i++;
                    }

                    // If i is equal to
                    // length of array
                    if (i >= n - 1)
                        return i;
                    else
                        return 0;
                }

                // First subarray is
                // strictly Decreasing
                else if (ar[0] > ar[1])
                {
                    int i = 1;

                    // Check for strictly
                    // increasing condition
                    // & find the break point
                    while (i < n && ar[i - 1] > ar[i])
                    {
                        i++;
                    }

                    // Check for strictly
                    // increasing condition
                    // & find the break point
                    while (i + 1 < n && ar[i] < ar[i + 1])
                    {
                        i++;
                    }

                    // If i is equal to
                    // length of array - 1
                    if (i >= n - 1)
                        return i;
                    else
                        return 0;
                }

                // Condition if ar[0] == ar[1]
                else
                {
                    for (int i = 2; i < n; i++)
                    {
                        if (ar[i - 1] <= ar[i])
                            return 0;
                    }
                    return 1;
                }
            }
        }
        public string GetPivotData(string Date, string Column, string groupName, string subGroup, string cKTNAME = "")
        {
            DataSet ds = new DataSet();
            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();


                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("GetPivotData", conn);
                    sqlComm.Parameters.AddWithValue("@column", Column);
                    sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
                    sqlComm.Parameters.AddWithValue("@groupName", groupName);
                    sqlComm.Parameters.AddWithValue("@subGroup", subGroup);
                    sqlComm.Parameters.AddWithValue("@CKTName", cKTNAME);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }



            }
            catch (Exception ex)
            {

                throw;

            }

            return ExportDatatableToHtml(ds.Tables[0], Column);
            //return ConvertDataTableToHTML(ds.Tables[0]);
        }
        public IEnumerable<Equities> GetAllStocks()
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_LIVE_STOCKS_BY_STOCK", conn);
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

        public IEnumerable<StockBuy> GetBuysStocks(string Date, string Top = "10")
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<StockBuy> stocks = new List<StockBuy>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("GetBuysStocks", conn);
                    sqlComm.Parameters.AddWithValue("@top", Convert.ToInt64(Top));
                    sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
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
                            var _stokc = new StockBuy();
                            _stokc.symbol = r[0].ToString();
                            _stokc.ltt = r[1].ToString();
                            _stokc.stock_name = r[2].ToString();
                            _stokc.last = Convert.ToDouble(r[3].ToString());
                            _stokc.open = Convert.ToDouble(r[4].ToString());
                            _stokc.close = Convert.ToDouble(r[5].ToString());
                            _stokc.high = Convert.ToDouble(r[6].ToString());
                            _stokc.ttv = Convert.ToDouble(r[7].ToString());
                            _stokc.low = Convert.ToDouble(r[8].ToString());
                            _stokc.change = Convert.ToDouble(r[9].ToString());
                            _stokc.avgPrice = Convert.ToDouble(r[10].ToString());
                            _stokc.bPrice = Convert.ToDouble(r[11].ToString());
                            _stokc.sPrice = Convert.ToDouble(r[12].ToString());
                            _stokc.prev = string.IsNullOrEmpty(r[13].ToString()) ? 0 : Convert.ToDouble(r[13].ToString());
                            _stokc.Ratio = Convert.ToDouble(r[14].ToString());
                            _stokc.VolumeC = r[15].ToString();
                            _stokc.INC = Convert.ToDouble(r[16].ToString());
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
            return Enumerable.Empty<StockBuy>();

        }


        public IEnumerable<Equities> GET_CKT(string Date, string Top = "10", string CKTName = "upperCktLm")
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("GET_CKT", conn);
                    sqlComm.Parameters.AddWithValue("@top", Convert.ToInt64(Top));
                    sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
                    sqlComm.Parameters.AddWithValue("@CKTName", CKTName);
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
                            _stokc.ltt = r[1].ToString();
                            _stokc.stock_name = r[2].ToString();
                            _stokc.last = Convert.ToDouble(r[3].ToString());
                            _stokc.open = Convert.ToDouble(r[4].ToString());
                            _stokc.close = Convert.ToDouble(r[5].ToString());
                            _stokc.high = Convert.ToDouble(r[6].ToString());
                            _stokc.ttv = Convert.ToDouble(r[7].ToString()).ToString();
                            _stokc.low = Convert.ToDouble(r[8].ToString());
                            _stokc.change = Convert.ToDouble(r[9].ToString());
                            _stokc.avgPrice = Convert.ToDouble(r[10].ToString());
                            _stokc.bPrice = Convert.ToDouble(r[11].ToString());
                            _stokc.sPrice = Convert.ToDouble(r[12].ToString());
                            double bqty;
                            double.TryParse(r[13].ToString(), out bqty);
                            _stokc.bQty = Convert.ToInt32(bqty);

                            double sqty;
                            double.TryParse(r[14].ToString(), out sqty);

                            _stokc.sQty = Convert.ToInt32(sqty);

                            double ltq;
                            double.TryParse(r[15].ToString(), out ltq);
                            _stokc.ltq = Convert.ToInt32(ltq);

                            double ttq;
                            double.TryParse(r[16].ToString(), out ttq);
                            _stokc.ttq = Convert.ToInt32(ttq);

                            double totalBuyQt;
                            double.TryParse(r[17].ToString(), out totalBuyQt);
                            _stokc.totalBuyQt = Convert.ToInt32(totalBuyQt);

                            double totalSellQ;
                            double.TryParse(r[18].ToString(), out totalSellQ);
                            _stokc.totalSellQ = Convert.ToInt32(totalSellQ);

                            double lowerCktLm;
                            double.TryParse(r[19].ToString(), out lowerCktLm);
                            _stokc.totalSellQ = Convert.ToInt32(lowerCktLm);

                            double upperCktLm;
                            double.TryParse(r[20].ToString(), out upperCktLm);
                            _stokc.totalSellQ = Convert.ToInt32(upperCktLm);
                            _stokc.msn_secid = r[24].ToString();


                            _stokc.VolumeC = (r[21].ToString());
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



        public List<Equities> GetHistryData()
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("ExportJsonData", conn);
                    //sqlComm.Parameters.AddWithValue("@top", Convert.ToInt64(Top));
                    //sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
                    //sqlComm.Parameters.AddWithValue("@CKTName", CKTName);
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
                            _stokc.ltt = r[1].ToString();
                            _stokc.stock_name = r[2].ToString();
                            _stokc.last = Convert.ToDouble(r[3].ToString());
                            _stokc.open = Convert.ToDouble(r[4].ToString());
                            _stokc.close = Convert.ToDouble(r[5].ToString());
                            _stokc.high = Convert.ToDouble(r[6].ToString());
                            _stokc.ttv = Convert.ToDouble(r[7].ToString()).ToString();
                            _stokc.low = Convert.ToDouble(r[8].ToString());
                            _stokc.change = Convert.ToDouble(r[9].ToString());
                            _stokc.avgPrice = Convert.ToDouble(r[10].ToString());
                            _stokc.bPrice = Convert.ToDouble(r[11].ToString());
                            _stokc.sPrice = Convert.ToDouble(r[12].ToString());
                            double bqty;
                            double.TryParse(r[13].ToString(), out bqty);
                            _stokc.bQty = Convert.ToInt32(bqty);

                            double sqty;
                            double.TryParse(r[14].ToString(), out sqty);

                            _stokc.sQty = Convert.ToInt32(sqty);

                            double ltq;
                            double.TryParse(r[15].ToString(), out ltq);
                            _stokc.ltq = Convert.ToInt32(ltq);

                            double ttq;
                            double.TryParse(r[16].ToString(), out ttq);
                            _stokc.ttq = Convert.ToInt32(ttq);

                            double totalBuyQt;
                            double.TryParse(r[17].ToString(), out totalBuyQt);
                            _stokc.totalBuyQt = Convert.ToInt32(totalBuyQt);

                            double totalSellQ;
                            double.TryParse(r[18].ToString(), out totalSellQ);
                            _stokc.totalSellQ = Convert.ToInt32(totalSellQ);

                            double lowerCktLm;
                            double.TryParse(r[19].ToString(), out lowerCktLm);
                            _stokc.lowerCktLm = Convert.ToInt32(lowerCktLm);

                            double upperCktLm;
                            double.TryParse(r[20].ToString(), out upperCktLm);
                            _stokc.upperCktLm = Convert.ToInt32(upperCktLm);

                            _stokc.exchange = "";
                            _stokc.trend = "";
                            _stokc.quotes = "";

                            _stokc.VolumeC = (r[21].ToString());
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
            return Enumerable.Empty<Equities>().ToList();

        }


        public IEnumerable<TopPerformerStocks> GetTopPerformerStocks(string Date, string Top = "10")
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.Date.ToShortDateString();
            }

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<TopPerformerStocks> stocks = new List<TopPerformerStocks>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("SP_GetTopPerformer", conn);
                    sqlComm.Parameters.AddWithValue("@top", Convert.ToInt64(Top));
                    sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
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
                            var _stokc = new TopPerformerStocks();

                            _stokc.stock_name = r[0].ToString();
                            _stokc.Volume = Convert.ToDouble(r[1].ToString());
                            _stokc.min_last = Convert.ToDouble(r[2].ToString());
                            _stokc.max_last = Convert.ToDouble(r[3].ToString());
                            _stokc.avg = Convert.ToDouble(r[4].ToString());
                            _stokc.Open = Convert.ToDouble(r[5].ToString());
                            _stokc.min_change = Convert.ToDouble(r[6].ToString());
                            _stokc.max_change = Convert.ToDouble(r[7].ToString());
                            _stokc.min_bPrice = Convert.ToDouble(r[8].ToString());
                            _stokc.max_bPrice = Convert.ToDouble(r[9].ToString());
                            _stokc.min_sPrice = Convert.ToDouble(r[10].ToString());
                            _stokc.max_sPrice = Convert.ToDouble(r[11].ToString());
                            _stokc.symbol = Convert.ToString(r[12].ToString());
                            _stokc.bPrice = Convert.ToDouble(r[13].ToString());
                            _stokc.sPrice = Convert.ToDouble(r[14].ToString());

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
            return Enumerable.Empty<TopPerformerStocks>();

        }

        internal List<DropdownSelect> GetSectorName()
        {
            try
            {
                List<DropdownSelect> stocks = new List<DropdownSelect>();
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_SectorName", conn);
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
                            var _stokc = new DropdownSelect();
                            _stokc.Text = r[0].ToString();
                            _stokc.Value = r[0].ToString();
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
            return Enumerable.Empty<DropdownSelect>().ToList();
        }

        internal List<DropdownSelect> GetIndustryNewName(string sectorName)
        {
            try
            {
                List<DropdownSelect> stocks = new List<DropdownSelect>();
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_IndustryNewName_By", conn);
                    sqlComm.Parameters.AddWithValue("@SectorName", (sectorName));
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
                            var _stokc = new DropdownSelect();
                            _stokc.Text = r[0].ToString();
                            _stokc.Value = r[0].ToString();
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
            return Enumerable.Empty<DropdownSelect>().ToList();
        }

        internal List<DropdownSelect> GetGroupName(string industryNewName)
        {
            try
            {
                List<DropdownSelect> stocks = new List<DropdownSelect>();
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_groupName_By", conn);
                    sqlComm.Parameters.AddWithValue("@IndustryNewName", (industryNewName));
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
                            var _stokc = new DropdownSelect();
                            _stokc.Text = r[0].ToString();
                            _stokc.Value = r[0].ToString();
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
            return Enumerable.Empty<DropdownSelect>().ToList();
        }

        internal List<DropdownSelect> GetSubgroupName(string groupName)
        {
            try
            {
                List<DropdownSelect> stocks = new List<DropdownSelect>();
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_SubgroupName_By", conn);
                    sqlComm.Parameters.AddWithValue("@groupName", (groupName));
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
                            var _stokc = new DropdownSelect();
                            _stokc.Text = r[0].ToString();
                            _stokc.Value = r[0].ToString();
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
            return Enumerable.Empty<DropdownSelect>().ToList();
        }

        internal void ExporttoJsonData()
        {
            try
            {
                ExportLiveStocksToJson();
                var results = GetHistryData().GroupBy(x => x.symbol);
                List<Equities> equities = new List<Equities>();
                foreach (var items in results)
                {
                    equities = new List<Equities>();

                    if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", items.Key)))
                    {
                        var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", items.Key));
                        equities = JsonConvert.DeserializeObject<List<Equities>>(text).OrderBy(x => x.ltt).ToList();
                    }
                    equities.AddRange(items.ToList());
                    string json = JsonConvert.SerializeObject(equities.OrderBy(x => Convert.ToDateTime(x.ltt)));
                    try
                    {
                        System.IO.File.WriteAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", items.Key), json);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal double GetJsonFileIndex(string Symvbol, double Last)
        {


            try
            {
                var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                var data = JsonConvert.DeserializeObject<List<Equities>>(text).OrderBy(x => x.ltt);

                var sortlist = data.Select(x => x.open.Value);

                var nearest = sortlist.OrderBy(x => Math.Abs((long)x - Last)).First();

                var index = sortlist.ToArray().IndexOf(nearest);

                return data.Count()- index;
                //return index;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void DownloadNSEData()
        {
            try
            {
                //var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                //var data = JsonConvert.DeserializeObject<List<Equities>>(text).OrderBy(x => x.ltt);

                //var sortlist = data.Select(x => x.open.Value);

                //var nearest = sortlist.OrderBy(x => Math.Abs((long)x - Last)).First();

                //var index = sortlist.ToArray().IndexOf(nearest);

                //return data.Count() - index;
                //return index;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void ExportLiveStocksToJson()
        {
            try
            {
                var results = SendAllStocksForLoad();
                var json = JsonConvert.SerializeObject(results);
                System.IO.File.WriteAllText(string.Format("{0}{1}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoks"), json);
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }
    }
}

