using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skender.Stock.Indicators;
using STM_API.Extention;
using STM_API.Extentions;
using STM_API.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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
                            _stokc.symbol = r["symbol"].ToString();
                            _stokc.Date = Convert.ToDateTime(r["Date"].ToString());
                            _stokc.Id = Convert.ToInt64(r["Id"].ToString());
                            _stokc.STOCKName = Convert.ToString(r["STOCKName"].ToString());
                            _stokc.IsNotified = Convert.ToBoolean(r["IsNotified"].ToString());
                            _stokc.IsUppCKT = Convert.ToBoolean(r["IsUppCKT"].ToString());
                            _stokc.ISSell = Convert.ToBoolean(r["ISSell"].ToString());
                            _stokc.ISPrict = Convert.ToBoolean(r["ISPrict"].ToString());
                            _stokc.Change = Convert.ToDecimal(r["Change"].ToString());
                            _stokc.PO_KEY_NAME = Convert.ToString(r["PO_KEY"]);
                            _stokc.Last = Convert.ToDecimal(Convert.ToString(r["last"].ToString()));
                            _stokc.PO_KEY_TOKEN = Convert.ToString(r["PO_KEY_TOKEN"]);
                            _stokc.user = Convert.ToString(r["user"]);
                            _stokc.priority = Convert.ToInt16(r["priority"].ToString());
                            _stokc.title = Convert.ToString(r["title"].ToString());
                            _stokc.retry = Convert.ToInt16(r["retry"].ToString());
                            _stokc.expire = Convert.ToInt16(r["expire"].ToString());
                            _stokc.sound = Convert.ToString(r["sound"].ToString());
                            stocks.Add(_stokc);
                        }
                        catch (Exception ex)
                        {
                            //throw;
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
        public string ConvertDataTableToHTMLTableInOneLine(DataTable dt)
        {
            //Convert DataTable To HTML Table in one line
            return "<table class='tablewithborder' cellpadding='2' cellspacing='0' style='border: solid 2px black;font-size: 10pt;font-family:Arial' width='100%'>\n<tr>" + string.Join("", dt.Columns.Cast<DataColumn>().Select(dc => "<td style='border: dotted 2px black;'>" + dc.ColumnName + "</td>")) + "</tr>\n" +
            "<tr  style='border: dotted 2px black;'>" + string.Join("</tr>\n<tr>", dt.AsEnumerable().Select((Value, Index) => new { Value, Index }).Select(row => "<td style='border: dotted 2px black;'>" + string.Join("</td><td style='border: dotted 2px black;'>", row.Value.ItemArray) + "</td>").ToArray()) + "</tr>\n</table>";

        }

        private T[] GetArray<T>(IList<T> iList) where T : new()
        {
            var result = new T[iList.Count];

            iList.CopyTo(result, 0);

            return result;
        }

        private DataTable GetpivotTableWithCountAdd(DataTable dt)
        {

            int i = 0;
            dt.Columns.Add("Count", typeof(System.Decimal));
            foreach (DataRow myRow in dt.Rows)
            {
                int j = 0;
                try
                {

                    if (!string.IsNullOrEmpty(myRow[j].ToString()) && !string.IsNullOrEmpty(myRow[5].ToString()) && Convert.ToDecimal(myRow[5].ToString()) > 0)
                    {
                        j = dt.Columns.Count - 2;
                        var val = ((Convert.ToDecimal(myRow[j].ToString()) - Convert.ToDecimal(myRow[5].ToString())));
                        myRow["Count"] = Convert.ToDecimal(string.Format("{0:0.00}", val));

                    }

                }
                catch (Exception ex)
                {


                }
                i = i + 1;
            }
            return dt;
        }
        public string ExportDatatableToHtml(DataTable dtt, string Column)
        {
            dtt.Columns.Remove("symbol");
            DataTable dt = GetpivotTableWithCountAdd(dtt);

            dt.DefaultView.Sort = "count desc";
            dt = dt.DefaultView.ToTable();

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
            strHTML.Append(" Diff");
            strHTML.Append("</th></b>");
            strHTML.Append("</tr>");
            int i = 0;
            try
            {
                foreach (DataRow myRow in dt.Rows)
                {


                    try
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
                            try
                            {

                                grcolor = "";

                                if (j == 3 && !string.IsNullOrEmpty(myRow[j].ToString()))
                                {
                                    grcolor = (Convert.ToDecimal(myRow[j].ToString()) > 0) ? "#CCFFCD" : "#ffe8e7";
                                }

                                if (j > 5 && j != dt.Columns.Count - 1 && !string.IsNullOrEmpty(myRow[j].ToString()) && !string.IsNullOrEmpty(myRow[j - 1].ToString()))
                                {
                                    grcolor = (Convert.ToDecimal(myRow[j].ToString()) > Convert.ToDecimal(myRow[j - 1].ToString())) ? "#CCFFCD" : "#ffe8e7";
                                }
                                strHTML.Append("<td style='border: dotted 1px black;black;background-color:" + grcolor + "'' >");
                                if (j == 4)
                                {
                                    string arrrowcolor = result > 2 ? "<span style='font-size:20px;black;color:green;text-align: right'>&#x2191</span>" : "<span style='font-size:20px;black;color:red;text-align: right'>&#8595</span>";

                                    var anchortagDetails = string.IsNullOrEmpty(myRow[0].ToString()) ? myRow[myColumn.ColumnName] : string.Format("<a target=\"_blank\"  href=\"https://www.msn.com/en-in/money/stockdetails/fi-{0}?duration=5D\">{1}</a>", myRow[0], myRow[myColumn.ColumnName].ToString());
                                    var anchortagmain = string.IsNullOrEmpty(myRow[0].ToString()) ? myRow[myColumn.ColumnName] : string.Format("<a target=\"_blank\" href='/StockDetails?id={0}'>{1}</a>", myRow[0], "NS");
                                    var anchortag = anchortagDetails + " " + anchortagmain;
                                    strHTML.Append(j == 4 ? arrrowcolor + "<span style='background-color:" + Truefalsecolor + ";text-align: right'>" + result + "</span>&nbsp; &nbsp;  " + anchortag : (Column.ToLower() == "ttv" && j > 4) ? numDifferentiation(myRow[myColumn.ColumnName].ToString()) : myRow[myColumn.ColumnName].ToString());

                                }
                                else
                                {
                                    strHTML.Append(j == 0 ? "" : (Column.ToLower() == "ttv" && j > 4) ? numDifferentiation(myRow[myColumn.ColumnName].ToString()) : myRow[myColumn.ColumnName].ToString());

                                }
                                strHTML.Append("</td>");


                                pfcolor = "";

                                if (dt.Columns.Count - 2 == j)
                                {
                                    if (!string.IsNullOrEmpty(myRow[j].ToString()) && !string.IsNullOrEmpty(myRow[5].ToString()) && Convert.ToDecimal(myRow[5].ToString()) > 0)
                                    {

                                        var val = ((Convert.ToDecimal(myRow[j].ToString()) - Convert.ToDecimal(myRow[5].ToString())) / Convert.ToDecimal(myRow[5].ToString())) * 100;
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
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
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

        (int, int) canMakeItBandB(int n, decimal[] ar)
        {
            int incrementcount = 0;
            int decrement = 0;
            // Base Case
            if (n == 1)
                return (0, 0);
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
                        incrementcount++;
                    }

                    // Check for strictly
                    // decreasing condition
                    // & find the break point
                    while (i + 1 < n && ar[i] > ar[i + 1])
                    {
                        i++;
                        decrement++;
                    }

                    // If i is equal to
                    // length of array
                    //if (i >= n - 1)
                    //    return i;
                    //else
                    //    return 0;
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
                        incrementcount++;
                    }

                    // Check for strictly
                    // increasing condition
                    // & find the break point
                    while (i + 1 < n && ar[i] < ar[i + 1])
                    {
                        i++;
                        decrement++;
                    }

                    // If i is equal to
                    // length of array - 1

                }

                // Condition if ar[0] == ar[1]
                else
                {
                    for (int i = 2; i < n; i++)
                    {
                        if (ar[i - 1] <= ar[i])
                            return (0, 0);
                    }
                    return (0, 0);
                }
            }
            return (incrementcount, decrement);
        }
        public string GetPivotData(string Date, string Column, string groupName, string subGroup, string cKTNAME = "", string ConditionOperator = "", string dynamicminValue = "", string dynamicmaxValue = "", bool isWatchList = false)
        {
            DataSet ds = new DataSet();
            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();


                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("GetPivotData", conn);
                    sqlComm.Parameters.AddWithValue("@column", Column);
                    sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
                    sqlComm.Parameters.AddWithValue("@groupName", groupName);
                    sqlComm.Parameters.AddWithValue("@subGroup", subGroup);
                    sqlComm.Parameters.AddWithValue("@CKTName", cKTNAME);

                    sqlComm.Parameters.AddWithValue("@ConditionOperator", ConditionOperator);
                    sqlComm.Parameters.AddWithValue("@dynamicminValue", dynamicminValue);
                    sqlComm.Parameters.AddWithValue("@dynamicmaxValue", dynamicmaxValue);

                    sqlComm.Parameters.AddWithValue("@isWatchList", isWatchList);

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



        public IEnumerable<EquitiesHsitry> GetStocksList(bool isfavorite = false, bool isUpperCircuit = false, bool islowerCircuit = false,
            bool isEnabledForAutoTrade = false, bool IsNotifications = false, int dynamicminValue = 0, int dynamicmaxValue = 0,
            string TDays = "", string WatchList = "", bool isTarget = false, bool isBullish = false, bool isbearish = false,
            bool IsOrderbyVolume = false, bool IsAward = false, string orderby_obj = "", string order = "", int skip = 0, int take = 250,
            bool IsIncludeDeleted = false,string EC="All", string statsColumnRecords = null, string statsColumnCondition = null, int lastRecords = 0)
        {

          
            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<EquitiesHsitry> stocks = new List<EquitiesHsitry>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_LIVE_STOCKS_BY_STOCK", conn);
                    sqlComm.Parameters.AddWithValue("@Code", DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@isfavorite", isfavorite);
                    sqlComm.Parameters.AddWithValue("@isAutoTrade", isEnabledForAutoTrade);
                    sqlComm.Parameters.AddWithValue("@ShowNotification", IsNotifications);
                    sqlComm.Parameters.AddWithValue("@minvalue", dynamicminValue);
                    sqlComm.Parameters.AddWithValue("@maxvalue", dynamicmaxValue);
                    sqlComm.Parameters.AddWithValue("@Tdays", TDays);
                    sqlComm.Parameters.AddWithValue("@WatchList", WatchList);
                    sqlComm.Parameters.AddWithValue("@isUpperCircuit", isUpperCircuit);
                    sqlComm.Parameters.AddWithValue("@islowerCircuit", islowerCircuit);
                    sqlComm.Parameters.AddWithValue("@skip", skip);
                    sqlComm.Parameters.AddWithValue("@take", take);

                    sqlComm.Parameters.AddWithValue("@IsOrderbyVolume", IsOrderbyVolume);
                    sqlComm.Parameters.AddWithValue("@orderby_obj", orderby_obj);
                    sqlComm.Parameters.AddWithValue("@sortorder", order);


                    sqlComm.Parameters.AddWithValue("@isBullish", isBullish);
                    sqlComm.Parameters.AddWithValue("@isbearish", isbearish);
                    sqlComm.Parameters.AddWithValue("@IsAward", IsAward);

                    sqlComm.Parameters.AddWithValue("@isTarget", isTarget);
                    sqlComm.Parameters.AddWithValue("@IsIncludeDeleted", IsIncludeDeleted);
                    sqlComm.Parameters.AddWithValue("@EC", EC);
                    sqlComm.Parameters.AddWithValue("@change", lastRecords);

                    sqlComm.Parameters.AddWithValue("@StatsColumn", statsColumnRecords);
                    sqlComm.Parameters.AddWithValue("@Condition", statsColumnCondition);


                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                //List<EquitiesHsitry> people = ((IEnumerable)ds.Tables[0].Rows).Cast<DataRow>().Select(r => 
                
                
                //new EquitiesHsitry { 
                    
                //    //ID = (int)r["ID"], Name = (string)r["Name"]
                
                
                
                //}
                
                
                
                //).ToList();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var NewStock = ds.Tables[0].Rows.Cast<DataRow>();

                    foreach (var r in NewStock)
                    {
                        try
                        {
                            var quotelist=GetJsonFileHistryData(r[0].ToString());

                            var quotelistasc = quotelist.OrderBy(x => x.ltt);
                            List<Quote> quotesList = quotelist.Select(x => new Quote
                            {
                                Close = Convert.ToDecimal(x.last),
                                Open = Convert.ToDecimal(x.open),
                                Date = Convert.ToDateTime(x.ltt),
                                High = Convert.ToDecimal(x.high),
                                Low = Convert.ToDecimal(x.low),
                                Volume = Convert.ToDecimal(x.ttv)
                            }).OrderBy(x => x.Date).ToList();

                           
                            IEnumerable<MacdResult> macdresult = quotesList.GetMacd(12, 26, 9);
                            IEnumerable<WmaResult> wmaResults  = quotesList.GetWma(5);
                            IEnumerable<VolatilityStopResult> Volatilityresults = quotesList.GetVolatilityStop(7, 3);
                            IEnumerable<RsiResult> rsiResults = quotesList.GetObv().GetRsi(7);

                            IEnumerable<SuperTrendResult> Strend =quotesList.GetSuperTrend(10, 3);
                            var candleResult = quotesList.GetMarubozu(85);

                            var _stokc = new EquitiesHsitry();
                            _stokc.symbol = r["symbol"].ToString();
                            _stokc.open = Convert.ToDouble(r["open"].ToString());
                            _stokc.last = Convert.ToDouble(r["last"].ToString());
                            _stokc.high = Convert.ToDouble(r["high"].ToString());
                            _stokc.low = Convert.ToDouble(r["low"].ToString());
                            _stokc.change = Convert.ToDouble(r["change"].ToString());
                            _stokc.bPrice = Convert.ToDouble(r["bPrice"].ToString());
                            _stokc.totalBuyQt = Convert.ToInt32(r["totalBuyQt"]);
                            _stokc.sPrice = Convert.ToDouble(r["sPrice"].ToString());
                            _stokc.totalSellQ = Convert.ToInt32(r["totalSellQ"]);
                            _stokc.avgPrice = Convert.ToDouble(r["avgPrice"].ToString());
                            _stokc.ttv = Convert.ToDouble(r["ttv"] ?? 0);//.Replace("L", "100000").Replace("C", "1000000"));
                            _stokc.lowerCktLm = Convert.ToDouble(r["lowerCktLm"].ToString());
                            _stokc.upperCktLm = Convert.ToDouble(r["upperCktLm"].ToString());
                            _stokc.ltt = r["ltt"].ToString();
                            _stokc.close = Convert.ToDouble(r["close"].ToString());
                            _stokc.stock_name = r["stock_name"].ToString();
                            _stokc.Data = quotelist.Select(x => x.close.Value).ToList().AddValue(Convert.ToDouble(r["close"].ToString())).ToList();
                                                                                                  
                            _stokc.buyat = Convert.ToDouble(r["buyAt"] ?? 0);
                            _stokc.DataPoint= GetJsonFileHistryDataPoint(r["symbol"].ToString(), Convert.ToDouble(_stokc.last), _stokc.buyat);
                            _stokc.min = _stokc.Data.Any() ? _stokc.Data.Where(x => x > 0).Min(x => Convert.ToInt32(x)) :0;
                            _stokc.max = _stokc.Data.Any() ?_stokc.Data.Where(x => x > 0).Max(x => Convert.ToInt32(x)):0;
                            _stokc.href = string.Format("https://www.msn.com/en-in/money/stockdetails/fi-{0}?duration=5D>", r["MSN_SECID"].ToString());
                            _stokc.stockdetailshref = string.Format("/StockDetails?id={0}", r["MSN_SECID"].ToString());
                            _stokc.return1w = Convert.ToDouble(r["quote_return1Week"] ?? 0);
                            _stokc.return1m = Convert.ToDouble(r["quote_return1Month"] ?? 0);
                            _stokc.return3m = Convert.ToDouble(r["quote_return3Month"] ?? 0);
                            _stokc.return1d = Convert.ToDouble(r["quote_priceChange"] ?? 0);
                            _stokc.securityId = r["securityId"].ToString();
                            _stokc.SECId = r["MSN_SECID"].ToString();
                            _stokc.msn_secid = r["MSN_SECID"].ToString();
                            _stokc.recmdtn = (r["estimate_recommendation"] ?? "").ToString();
                            _stokc.noofrec = Convert.ToDouble(r["estimate_numberOfAnalysts"] ?? 0);
                            _stokc.beta = (r["beta"] ?? "").ToString();
                            _stokc.eps = (r["keyMetrics_eps"] ?? "").ToString();
                           
                            _stokc.target = (r["estimate_meanPriceTarget"] ?? "").ToString();
                            _stokc.isfavorite = !string.IsNullOrEmpty(r["isfavorite"].ToString()) ? Convert.ToBoolean(r["isfavorite"] ?? false): false;
                            _stokc.VolumeC = (r["VolumeC"] ?? "").ToString();
                            _stokc.return6m= Convert.ToDouble(r["quote_return6Month"] ?? 0);
                            _stokc.return1Year = Convert.ToDouble(r["quote_return1Year"] ?? 0);
                            _stokc.returnYTD = Convert.ToDouble(r["quote_returnYTD"] ?? 0);
                            _stokc.priceChange_Day = Convert.ToDouble(r["quote_priceChange"] ?? 0);

                            _stokc.priceChange_1w = Convert.ToDouble(r["quote_priceChange1Week"] ?? 0);
                            _stokc.priceChange_1m = Convert.ToDouble(r["quote_priceChange1Month"] ?? 0);
                            _stokc.priceChange_3m = Convert.ToDouble(r["quote_priceChange3Month"] ?? 0);

                            _stokc.priceChange_6m = Convert.ToDouble(r["quote_priceChange6Month"] ?? 0);
                            _stokc.priceChange_1year = Convert.ToDouble(r["quote_priceChange1Year"] ?? 0);
                            _stokc.priceChange_YTD = Convert.ToDouble(r["quote_priceChangeYTD"] ?? 0);
                            
                            _stokc.price52Weekshigh = Convert.ToDouble(r["quote_price52wHigh"] ?? 0);
                            _stokc.price52Weekslow = Convert.ToDouble(r["quote_price52wLow"] ?? 0);
                            _stokc.isenabledforautoTrade= Convert.ToBoolean(r["isAutoTrad"] ?? false);
                            _stokc.buyatChange= (r["buyAtChange"] ?? "").ToString();
                            _stokc.IsLowerCircuite = Convert.ToDouble(r["open"].ToString()) == _stokc.lowerCktLm;
                            _stokc.IsUpperCircuite = Convert.ToDouble(r["open"].ToString()) == _stokc.upperCktLm;
                            _stokc.tdays = Convert.ToString(r["TDay"] ?? 0);
                            _stokc.WacthList = Convert.ToString(r["watchlist"] ?? "");

                            _stokc.pr_change = string.Join(',', quotelist.Skip(quotelist.Count-30).Take(30).Select(x => x.change)); //// Convert.ToString(r[65] ?? "");
                            _stokc.pr_close = string.Join(',', quotelist.Skip(quotelist.Count - 30).Take(30).Select(x => x.close)); //Convert.ToString(r[66] ?? "");
                            _stokc.pr_open = string.Join(',', quotelist.Skip(quotelist.Count - 30).Take(30).Select(x => x.open)); //Convert.ToString(r[67] ?? "");
                            _stokc.pr_volume = string.Join(',', quotelist.Skip(quotelist.Count - 30).Take(30).Select(x => x.VolumeC));// Convert.ToString(r[68] ?? "");
                            _stokc.pr_date = string.Join(',', quotelist.Skip(quotelist.Count - 30).Take(30).Select(x => Convert.ToDateTime(x.ltt).ToShortDateString()));// Convert.ToString(r[69] ?? "");

                            _stokc.pr_Macresult = string.Join(",", macdresult.Skip(quotelist.Count - 30).Take(30).Select(x => x.Macd.HasValue ? Convert.ToDouble(x.Macd).ToString("F2") : ""));
                            _stokc.pr_RSI = string.Join(",", rsiResults.Skip(quotelist.Count - 30).Take(30).Select(x => x.Rsi.HasValue ? Convert.ToDouble(x.Rsi).ToString("F2") : ""));
                            _stokc.pr_Match = string.Join(",", candleResult.Skip(quotelist.Count - 30).Take(30).Select(x=>x.Match.ToString().Replace("Signal","")));
                            _stokc.pr_SuperTrend = string.Join(",", Strend.Skip(quotelist.Count - 30).Take(30).Select(x => x.SuperTrend.HasValue ? Convert.ToDouble(x.SuperTrend).ToString("F2") : ""));
                            
                            _stokc.Match = Convert.ToString(r["match"] ?? "");
                            _stokc.BullishCount = Convert.ToInt16(r["BulishCount"] ?? 0);
                            _stokc.BearishCount = Convert.ToInt16(r["BearishCount"] ?? 0);

                            _stokc.AwardCount = Convert.ToInt32(r["AwardCount"] ?? 0);

                           var resverse= quotelist.Skip(quotelist.Count - 10).Take(10).Select(x => Convert.ToDouble(x.change).ToString("N2")).Reverse();
                            _stokc.last7DaysChange = string.Join(',', resverse);

                            _stokc.fn_eps = Convert.ToDouble(r["fn_eps"] ?? 0);
                            _stokc.oPM_Percentage = Convert.ToDouble(r["OPM_Percentage"] ?? 0);
                            _stokc.nPM_Percentage = Convert.ToDouble(r["NPM_Percentage"] ?? 0);
                            _stokc.profit_Increase = Convert.ToDouble(r["Profit_Increase"] ?? 0);
                            _stokc.revenueIncrease = Convert.ToDouble(r["RevenueIncrease"] ?? 0);
                            _stokc.profitDifference = Convert.ToDouble(r["ProfitDifference"] ?? 0);
                            _stokc.revenueDifference = Convert.ToDouble(r["RevenueDifference"] ?? 0);
                            _stokc.quarterEnd = Convert.ToDateTime(r["QuarterEnd"].ToString() !="" ? Convert.ToDateTime(r["QuarterEnd"]).ToShortDateString(): null);
                            _stokc.FnUpdatedon = Convert.ToDateTime(r["FnUpdatedon"].ToString() !="" ? Convert.ToDateTime(r["FnUpdatedon"]).ToShortDateString(): null);
                            _stokc.last7DaysChange = Convert.ToString(string.IsNullOrEmpty(r["Last7DaysChange"].ToString()) ? _stokc.last7DaysChange :r["Last7DaysChange"].ToString());
                            _stokc.change = Convert.ToDouble(r["quote_priceChangePercent"].ToString())==0 ? _stokc.change: Convert.ToDouble(r["quote_priceChangePercent"].ToString());
                            _stokc.rowcount = Convert.ToInt32(r["counts"].ToString());
                            _stokc.futurePercentage = Convert.ToDouble(r["FuturePercentage"] ?? 0);
                            _stokc.quaterlyResults = Convert.ToString(r["QuaterlyResults"] ?? "");
                            _stokc.isIncludeDeleted = Convert.ToBoolean(r["IsExclude"] ?? false);

                            _stokc.past_PercentageChange = Convert.ToDouble(string.IsNullOrEmpty(r["Past_PercentageChange"].ToString()) ?  0 : r["Past_PercentageChange"].ToString());
                            _stokc.past_PriceChange = Convert.ToDouble(string.IsNullOrEmpty(r["Past_PriceChange"].ToString()) ? 0 : r["Past_PriceChange"].ToString()); 


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
            return Enumerable.Empty<EquitiesHsitry>();

        }
        public IEnumerable<Equities> GetAllStocks()
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("SP_GET_LIVE_STOCKS_BY_STOCK", conn);
                    sqlComm.Parameters.AddWithValue("@Code", DBNull.Value);
                    sqlComm.Parameters.AddWithValue("@isfavorite", false);
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

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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


        public IEnumerable<Stock_Days_Results> GET_StockDays(string Date, string Top = "10", string CKTName = "upperCktLm")
        {

            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Stock_Days_Results> stocks = new List<Stock_Days_Results>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("Get_STock_Days", conn);
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
                            var _stokc = new Stock_Days_Results();
                            _stokc.stock_name = r[0].ToString();
                            _stokc.open = Convert.ToDouble(r[1].ToString());
                            _stokc.close = Convert.ToDouble(r[2].ToString());
                            _stokc.change = Convert.ToDouble(r[3].ToString());
                            _stokc.Days = Convert.ToInt16(r[4].ToString());
                            _stokc.BearishCount = Convert.ToInt16(r[5].ToString());
                            _stokc.BullishCount = Convert.ToInt16(r[6].ToString());
                            _stokc.estimate_recommendation = (r[7].ToString());
                            _stokc.volumeC = r[8].ToString();
                            _stokc.msn_secid = r[9].ToString();
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
            return Enumerable.Empty<Stock_Days_Results>();

        }



        public List<Equities> GetHistryData()
        {

           
            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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
                            _stokc.msn_secid = r[22].ToString();
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

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
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


        internal void StockDays()
        {
            try
            {
                var results = SendAllStocksForLoad();
                var allstocks = results.GroupBy(x => x.symbol);
                List<Stock_Days> equities = new List<Stock_Days>();
                foreach (var items in allstocks)
                {
                    try
                    {
                        List<decimal> arrayofItem = new List<decimal>();
                        var Days = (GetJsonFileIndexWithArray(items.Key, Convert.ToDouble(items.FirstOrDefault().last), ref arrayofItem));

                        Stock_Days stock_Days = new Stock_Days();
                        stock_Days.Symbol = items.Key;
                        stock_Days.Days = (int)Days;
                        try
                        {
                            if (arrayofItem.Count() > 0)
                            {
                                var Count = canMakeItBandB(arrayofItem.Count(), (arrayofItem).ToArray());
                                stock_Days.BearishCount = Count.Item1;
                                stock_Days.BullishCount = Count.Item2;
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        equities.Add(stock_Days);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }


                }
                DataTable dt = new DataTable();
                dt = equities.ToDataTable<Stock_Days>();

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {


                    SqlCommand sqlComm = new SqlCommand("truncate table Stock_Days", conn);

                    sqlComm.CommandType = CommandType.Text;
                    conn.Open();
                    sqlComm.ExecuteNonQuery();
                    conn.Close();



                }

                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {
                    var bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null)
                    {
                        DestinationTableName = "dbo.Stock_Days",
                        BatchSize = dt.Rows.Count
                    };
                    conn.Open();
                    bc.WriteToServer(dt);
                    conn.Close();
                    bc.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal double GetJsonFileIndex(string Symvbol, double Last, out string link)
        {
            link = "";

            if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol)))
            {
                try
                {
                    var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                    var data = JsonConvert.DeserializeObject<List<Equities>>(text).OrderBy(x => Convert.ToDateTime(x.ltt));

                    var datalink = data.Where(x => x.msn_secid != null).Where(x=>!string.IsNullOrEmpty(x.msn_secid));

                    var sortlistgroup = data.GroupBy(x => Convert.ToDateTime(x.ltt).Date).Select(x => x).ToList();

                    var newList = data.GroupBy(x => new { Convert.ToDateTime(x.ltt).Date, x.open, x.close }).Select(y => new{ltt = y.Key.Date,open = y.Key.open,close = y.Key.close});


                    var open_closest = newList.Select(x => x.open.Value).Aggregate((x, y) => Math.Abs(Convert.ToDouble(x) - Last) < Math.Abs(Convert.ToDouble(y) - Last) ? x : y);
                    var close_closest = newList.Select(x => x.open.Value).Aggregate((x, y) => Math.Abs(Convert.ToDouble(x) - Last) < Math.Abs(Convert.ToDouble(y) - Last) ? x : y);

                    var nearest = newList.FirstOrDefault(x => x.open == open_closest);
                    var datass = (DateTime.Now.Date - nearest.ltt.Date).Days;

                    link += "on Date " + nearest.ltt.Date.ToShortDateString();
                    if (datalink.Any())
                    {
                        link += "  Link "+ string.Format("https://www.msn.com/en-in/money/stockdetails/fi-{0}?duration=5D",datalink.FirstOrDefault().msn_secid);
                    }
                    return datass;
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        internal double GetJsonFileIndexWithArray(string Symvbol, double Last, ref List<decimal> Arraysortlist)
        {

            if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol)))
            {
                try
                {
                    var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                    var data = JsonConvert.DeserializeObject<List<Equities>>(text).OrderBy(x => Convert.ToDateTime(x.ltt));

                    var sortlist = data.Select(x => x.close.Value);
                    Arraysortlist = sortlist.Select(x => Convert.ToDecimal(x)).ToList();
                    var nearest = sortlist.OrderBy(x => Math.Abs((long)x - Last)).First();

                    var index = sortlist.ToArray().IndexOf(nearest);

                    return data.Count() - index;
                    //return index;
                }
                catch (Exception)
                {

                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }


   
        public List<Equities> GetJsonFileHistryData(string Symvbol)
        {

            if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol)))
            {
                try
                {
                    var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                   return JsonConvert.DeserializeObject<List<Equities>>(text).OrderByDescending(x => Convert.ToDateTime(x.ltt)).Take(60).OrderBy(x => Convert.ToDateTime(x.ltt)).ToList();

                    //var sortlist = data.Select(x => x.close.Value).ToList();
                    //sortlist.Add(Last);
                    //return sortlist;



                    //Arraysortlist = sortlist.Select(x => Convert.ToDecimal(x)).ToList();
                    //var nearest = sortlist.OrderBy(x => Math.Abs((long)x - Last)).First();

                    //var index = sortlist.ToArray().IndexOf(nearest);

                    //return data.Count() - index;
                    //return index;
                }
                catch (Exception)
                {

                    return new List<Equities>();
                }
            }
            else
            {
                return new List<Equities>();
            }
        }

        public List<ChartData> GetJsonFileHistryDataPoint(string Symvbol, double Last,double buyat)
        {

            if (System.IO.File.Exists(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol)))
            {
                try
                {
                    var text = System.IO.File.ReadAllText(string.Format("{0}{1}.json", @"C:\Hosts\JsonFiles\", Symvbol));
                    var data = JsonConvert.DeserializeObject<List<Equities>>(text).ToList().OrderByDescending(x => Convert.ToDateTime(x.ltt)).Take(60).OrderBy(x => Convert.ToDateTime(x.ltt));

                    var sortlist = data.Select(x => new ChartData { value = x.close.Value, extremum = null }).ToList();
                    sortlist.Add(new ChartData { value=Last});
                    sortlist.Where(x=>x.value==buyat).ToList().ForEach(x=>x.extremum="Buy");

                    return sortlist;
                    //Arraysortlist = sortlist.Select(x => Convert.ToDecimal(x)).ToList();
                    //var nearest = sortlist.OrderBy(x => Math.Abs((long)x - Last)).First();

                    //var index = sortlist.ToArray().IndexOf(nearest);

                    //return data.Count() - index;
                    //return index;
                }
                catch (Exception)
                {

                    return new List<ChartData>();
                }
            }
            else
            {
                return new List<ChartData>();
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
                ExportAllLiveStocksToJson(results.ToList());

                var chubnkresulst = results.Chunk<Equities>(3000);
                int i = 0;
                foreach (var item in chubnkresulst)
                {
                    var json = JsonConvert.SerializeObject(item);
                    System.IO.File.WriteAllText(string.Format("{0}{1}{2}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoks", i), json);
                    i++;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void ExportAllLiveStocksToJson(List<Equities> list)
        {
            try
            {
                var results = list.ToList();

                
                var chubnkresulst = results;
                int i = 0;
                //foreach (var item in chubnkresulst)
                //{
                    var json = JsonConvert.SerializeObject(chubnkresulst);
                    System.IO.File.WriteAllText(string.Format("{0}{1}.json", @"C:\Hosts\JobStocksJson\", "LiveStcoks"), json);
                    i++;
                //}


            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void UpdateNotificationSend(string ids)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {
                SqlCommand sqlComm = new SqlCommand("UpdateNotiifcation", conn);
                sqlComm.Parameters.AddWithValue("@ids", (ids));
                sqlComm.CommandType = CommandType.StoredProcedure;
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();

            }
        }


        internal object GetStockDetailsBySymbol(string symbol)
        {
            DataSet ds = new DataSet();
            try
            {
                //  var NewStock = db.Live_Stocks.FromSql("Execute dbo.SP_GET_LIVE_STOCKS_BY_STOCK {0}", stock.Symbol).ToList(); //.FirstOrDefault(x => x.symbol == stock.Symbol);
                List<Equities> stocks = new List<Equities>();


                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("getStockDetails", conn);
                    sqlComm.Parameters.AddWithValue("@symbol", symbol);

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

            return ConvertDataTableToHTMLTableInOneLine(ds.Tables[0]);
        }

        internal bool SaveWatchList(string onDate, string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("AddOrUpdateWatchList", conn);
                    sqlComm.Parameters.AddWithValue("@symbol", id);
                    sqlComm.Parameters.AddWithValue("@Date", Convert.ToDateTime(onDate));
                    conn.Open();
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        internal object AddOrModifyFavorite(string mscid, int isFavorite)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                SqlCommand sqlComm = new SqlCommand("AddOrUpdateFavorites", conn);
                sqlComm.Parameters.AddWithValue("@msnId", mscid);
                sqlComm.Parameters.AddWithValue("@ifavorite", isFavorite == 1 ? true : false);
                conn.Open();
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
        }

        internal object AddOrModifyAutoTrade(string mscid, int action)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("AddOrUpdateAutoTrade", conn);
                    sqlComm.Parameters.AddWithValue("@msnId", mscid);
                    sqlComm.Parameters.AddWithValue("@isAutoTrade", action == 1 ? true : false);
                    conn.Open();
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal object AddOrModifyIsExclude(string mscid, int action)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    //using(SqlConnection conn = new SqlConnection("Server=103.21.58.192;Database=skyshwx7_;User ID=Honey;Password=K!cjn3376;TrustServerCertificate=false;Trusted_Connection=false;MultipleActiveResultSets=true;")) {
                    SqlCommand sqlComm = new SqlCommand("AddOrUpdateIsExclude", conn);
                    sqlComm.Parameters.AddWithValue("@msnId", mscid);
                    sqlComm.Parameters.AddWithValue("@IsExclude", action == 1 ? true : false);
                    conn.Open();
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal CancellationToken Equities_Stats()
        {
            throw new NotImplementedException();
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

