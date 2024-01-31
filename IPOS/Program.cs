using IPOS.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    static void Main()
    {

        Thread.Sleep(5000);
        Process proc = new Process();
        proc.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        proc.StartInfo.Arguments = " --new-window --window-size=50,50 https://www.nseindia.com/";
        proc.Start();


        Thread.Sleep(5000);
         var proc1 = new Process();
        proc1.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        proc1.StartInfo.Arguments = " --new-window --window-size=50,50 https://www.nseindia.com/api/all-upcoming-issues?category=ipo";
        proc1.Start();

        Thread.Sleep(5000);
        var proc2 = new Process();
        proc2.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        proc2.StartInfo.Arguments = " --new-window --window-size=50,50 https://www.nseindia.com/api/ipo-current-issue";
        proc2.Start();

        string Url = "https://www.nseindia.com/api/all-upcoming-issues?category=ipo";
        CookieContainer cookieJar = new CookieContainer();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        request.CookieContainer = cookieJar;

        request.Accept = @"text/html, application/xhtml+xml, */*";
        request.Referer = @"http://www.nseindia.com/";
        request.Headers.Add("Accept-Language", "en-GB");
        //request.UserAgent = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";
        request.Host = @"www.nseindia.com";
        request.UseDefaultCredentials = false;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        String htmlString;
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            htmlString = reader.ReadToEnd();
        }


        string CurrentUrl = "https://www.nseindia.com/api/ipo-current-issue";
        CookieContainer cr_cookieJar = new CookieContainer();
        HttpWebRequest cr_request = (HttpWebRequest)WebRequest.Create(CurrentUrl);
        cr_request.CookieContainer = cr_cookieJar;

        cr_request.Accept = @"text/html, application/xhtml+xml, */*";
        cr_request.Referer = @"http://www.nseindia.com/";
        cr_request.Headers.Add("Accept-Language", "en-GB");
        //request.UserAgent = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";
        cr_request.Host = @"www.nseindia.com";
        cr_request.UseDefaultCredentials = false;
        HttpWebResponse cr_response = (HttpWebResponse)cr_request.GetResponse();
        String cr_htmlString;
        using (var reader = new StreamReader(cr_response.GetResponseStream()))
        {
            cr_htmlString = reader.ReadToEnd();
        }

        Thread.Sleep(5000);
        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + "TaskKill /im \"msedge.exe\" /f");

        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;

        // wrap IDisposable into using (in order to release hProcess) 
        using (Process process = new Process())
        {
            process.StartInfo = procStartInfo;
            process.Start();

            // Add this: wait until process does its work
            process.WaitForExit();

            // and only then read the result
            string result = process.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
        }
        Thread.Sleep(5000);
        
        var Resp_obj = JsonConvert.DeserializeObject<List<ipo_Upcomming>>(htmlString);

        foreach (var r in Resp_obj)
        {
            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
            {

                try
                {
                    SqlCommand sqlComm = new SqlCommand("Insertipo_Upcomming", conn);
                    sqlComm.Parameters.AddWithValue("@symbol", r.symbol.ToString() ?? "");
                    sqlComm.Parameters.AddWithValue("@companyName", r.companyName.ToString() ?? "");
                    sqlComm.Parameters.AddWithValue("@series", r.series.ToString() ?? "");
                    sqlComm.Parameters.AddWithValue("@issueStartDate ", r.issueStartDate ?? "");
                    sqlComm.Parameters.AddWithValue("@issueEndDate", r.issueEndDate ?? "");
                    sqlComm.Parameters.AddWithValue("@status", r.status ?? "");
                    sqlComm.Parameters.AddWithValue("@issueSize", r.issueSize ?? 0);
                    sqlComm.Parameters.AddWithValue("@issuePrice", r.issuePrice ?? "");
                    sqlComm.Parameters.AddWithValue("@sr_no", Convert.ToInt16(r.sr_no ?? 0));
                    sqlComm.Parameters.AddWithValue("@isBse", r.isBse=="1" ?true:false);
                    sqlComm.Parameters.AddWithValue("@lotSize", r.lotSize ?? 0);
                    sqlComm.Parameters.AddWithValue("@priceBand", Convert.ToString(r.priceBand) ?? "");
                    
                    sqlComm.Parameters.AddWithValue("@filename", Convert.ToString(r.filename) ?? "");
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }



        if (cr_htmlString != null)
        {
            var CR_Resp_obj = JsonConvert.DeserializeObject<List<ipo_current_issue>>(cr_htmlString);

            foreach (var r in CR_Resp_obj)
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    try
                    {
                        SqlCommand sqlComm = new SqlCommand("Insertipo_Current", conn);
                        sqlComm.Parameters.AddWithValue("@symbol", r.symbol.ToString() ?? "");
                        sqlComm.Parameters.AddWithValue("@companyName", r.companyName.ToString() ?? "");
                        sqlComm.Parameters.AddWithValue("@series", r.series.ToString() ?? "");
                        sqlComm.Parameters.AddWithValue("@issueStartDate ", r.issueStartDate ?? "");
                        sqlComm.Parameters.AddWithValue("@issueEndDate", r.issueEndDate ?? "");
                        sqlComm.Parameters.AddWithValue("@status", r.status ?? "");
                        sqlComm.Parameters.AddWithValue("@issueSize", r.issueSize ?? 0);
                        sqlComm.Parameters.AddWithValue("@issuePrice", r.issuePrice ?? "");
                        sqlComm.Parameters.AddWithValue("@sr_no", r.srNo ?? "");
                        sqlComm.Parameters.AddWithValue("@isBse", r.isBse == "1" ? true : false);
                        sqlComm.Parameters.AddWithValue("@lotSize", 0);
                        sqlComm.Parameters.AddWithValue("@priceBand", "");

                        sqlComm.Parameters.AddWithValue("@filename", "");
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        sqlComm.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
        }
    }


    public static string ExecuteCurl(string curlCommand, int timeoutInSeconds = 60)
    {



        using (var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "curl.exe",
                Arguments = curlCommand.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Environment.SystemDirectory
            }
        })
        {
            proc.Start();

            //proc.WaitForExit(timeoutInSeconds * 1000);

            return proc.StandardOutput.ReadToEnd();
        }
    }
}