using iText.Kernel.Pdf;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

public class Program
{
    static void Main()
    {
        var cmd = @"curl ""https://api.bseindia.com/BseIndiaAPI/api/TabResults_PAR/w?scripcode=500103&tabtype=RESULTS"" ^
  -H ""authority: api.bseindia.com"" ^
  -H ""accept: application/json, text/plain, */*"" ^
  -H ""accept-language: en-US,en;q=0.9"" ^
  -H ""origin: https://www.bseindia.com"" ^
  -H ""referer: https://www.bseindia.com/"" ^
  -H ""sec-ch-ua: ^\^""Not A(Brand^\^"";v=^\^""99^\^"", ^\^""Microsoft Edge^\^"";v=^\^""121^\^"", ^\^""Chromium^\^"";v=^\^""121^\^"""" ^
  -H ""sec-ch-ua-mobile: ?0"" ^
  -H ""sec-ch-ua-platform: ^\^""Windows^\^"""" ^
  -H ""sec-fetch-dest: empty"" ^
  -H ""sec-fetch-mode: cors"" ^
  -H ""sec-fetch-site: same-site"" ^
  -H ""user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36 Edg/121.0.0.0"" ^";


        //var output = ExecuteCurl(string.Format(cmd,1, 20231026, 20231026));

        var output = ExecuteCurl(string.Format(cmd, 1, DateTime.Now.Date.ToString("yyyyMMdd"), DateTime.Now.Date.ToString("yyyyMMdd")));
        //var Resp_obj = JsonConvert.DeserializeObject<BSE_NEWS>(output);

        //int loop = 1;
        //int total = Resp_obj.Table1.FirstOrDefault().ROWCNT;
        //int numberofpage = 2 + total / 50;

        //for (int k = 1; k <= numberofpage; k++)
        //{
        //    output = ExecuteCurl(string.Format(cmd, k, DateTime.Now.Date.ToString("yyyyMMdd"), DateTime.Now.Date.ToString("yyyyMMdd")));

        //    if (output != null)
        //    {
        //        Resp_obj = JsonConvert.DeserializeObject<BSE_NEWS>(output);



        //        foreach (var r in Resp_obj.Table)
        //        {
        //            using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
        //            {

        //                try
        //                {
        //                    SqlCommand sqlComm = new SqlCommand("Insert_BSE_NEWS", conn);
        //                    sqlComm.Parameters.AddWithValue("@NEWSID", r.NEWSID.ToString() ?? "");
        //                    sqlComm.Parameters.AddWithValue("@SCRIP_CD", Convert.ToInt64(r.SCRIP_CD));
        //                    sqlComm.Parameters.AddWithValue("@XML_NAME", r.XML_NAME ?? "");
        //                    sqlComm.Parameters.AddWithValue("@NEWSSUB", r.NEWSSUB ?? "");
        //                    sqlComm.Parameters.AddWithValue("@DT_TM", Convert.ToDateTime(r.DT_TM));
        //                    sqlComm.Parameters.AddWithValue("@NEWS_DT", Convert.ToDateTime(r.NEWS_DT));
        //                    sqlComm.Parameters.AddWithValue("@CRITICALNEWS", Convert.ToBoolean(r.CRITICALNEWS));
        //                    sqlComm.Parameters.AddWithValue("@ANNOUNCEMENT_TYPE", r.ANNOUNCEMENT_TYPE ?? "");
        //                    sqlComm.Parameters.AddWithValue("@QUARTER_ID", Convert.ToInt16(r.QUARTER_ID));
        //                    sqlComm.Parameters.AddWithValue("@FILESTATUS", r.FILESTATUS ?? "");
        //                    sqlComm.Parameters.AddWithValue("@ATTACHMENTNAME", string.IsNullOrEmpty(r.ATTACHMENTNAME) ? "" : "https://www.bseindia.com/xml-data/corpfiling/AttachLive/" + r.ATTACHMENTNAME);
        //                    sqlComm.Parameters.AddWithValue("@MORE", Convert.ToString(r.MORE) ?? "");

        //                    //var name = Convert.ToString(r.HEADLINE) ?? "";
        //                    //if (name.Length > 1000)
        //                    //    name = name.Substring(1, 1000);

        //                    sqlComm.Parameters.AddWithValue("@HEADLINE", Convert.ToString(r.HEADLINE) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@CATEGORYNAME", Convert.ToString(r.CATEGORYNAME) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@OLD", Convert.ToBoolean(r.OLD));
        //                    sqlComm.Parameters.AddWithValue("@RN", Convert.ToBoolean(r.RN));
        //                    sqlComm.Parameters.AddWithValue("@PDFFLAG", Convert.ToBoolean(r.PDFFLAG));
        //                    sqlComm.Parameters.AddWithValue("@NSURL", Convert.ToString(r.NSURL) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@SLONGNAME", Convert.ToString(r.SLONGNAME) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@AGENDA_ID", Convert.ToInt32(r.AGENDA_ID));
        //                    sqlComm.Parameters.AddWithValue("@TotalPageCnt", Convert.ToInt32(r.TotalPageCnt));
        //                    sqlComm.Parameters.AddWithValue("@News_submission_dt", r.News_submission_dt.HasValue ? Convert.ToDateTime(r.News_submission_dt) : DBNull.Value);
        //                    sqlComm.Parameters.AddWithValue("@DissemDT", Convert.ToDateTime(r.DissemDT));
        //                    sqlComm.Parameters.AddWithValue("@TimeDiff", Convert.ToString(r.TimeDiff) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@Fld_Attachsize", Convert.ToInt32(r.Fld_Attachsize));
        //                    sqlComm.Parameters.AddWithValue("@SUBCATNAME", Convert.ToString(r.SUBCATNAME) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@AUDIOFILE", Convert.ToString(r.AUDIO_VIDEO_FILE) ?? "");
        //                    sqlComm.Parameters.AddWithValue("@BaseURL", "https://www.bseindia.com/xml-data/corpfiling/AttachLive/");
        //                    sqlComm.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
        //                    sqlComm.CommandType = CommandType.StoredProcedure;
        //                    conn.Open();
        //                    sqlComm.ExecuteNonQuery();
        //                    conn.Close();
        //                }
        //                catch (Exception ex)
        //                {

        //                    throw;
        //                }
        //            }
        //        }
        //    }

        //}
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