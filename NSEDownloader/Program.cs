using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using HtmlAgilityPack;
using Microsoft.VisualBasic.FileIO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {

            Process proc1 = new Process();
            proc1.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            proc1.StartInfo.Arguments = " --new-window --window-size=50,50 https://www.nseindia.com";
            proc1.Start();

            Thread.Sleep(5000);
            Process proc = new Process();
            proc.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            proc.StartInfo.Arguments = " --new-window --window-size=50,50 https://www.nseindia.com/api/corporate-announcements?index=equities&csv=true";
            proc.Start();

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

            var files = new DirectoryInfo("C:\\Hosts\\Down\\").GetFiles().OrderByDescending(o => o.LastWriteTime).FirstOrDefault();

            DataTable dataTable = new DataTable();
            using (TextFieldParser parser = new TextFieldParser(files.FullName))
            {
                parser.SetDelimiters(",");
                string[] headers = parser.ReadFields();
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }
                while (!parser.EndOfData)
                {
                    string[] rows = parser.ReadFields();
                    dataTable.Rows.Add(rows);
                }
            }


            //var data = ConvertCSVtoDataTable(files.FullName);

            System.IO.File.Delete(files.FullName);
            var NewStock = dataTable.Rows.Cast<DataRow>();
            foreach (var r in NewStock)
            {
                using (SqlConnection conn = new SqlConnection("Server=HAADVISRI\\AGS;Database=STOCK;User ID=sa;Password=240149;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true;"))
                {

                    SqlCommand sqlComm = new SqlCommand("InsertNSENews", conn);
                    sqlComm.Parameters.AddWithValue("@Symbol", r[0].ToString());
                    sqlComm.Parameters.AddWithValue("@CompanyName", r[1].ToString());
                    sqlComm.Parameters.AddWithValue("@Subject", r[2].ToString());
                    sqlComm.Parameters.AddWithValue("@Details", r[3].ToString());
                    sqlComm.Parameters.AddWithValue("@BroadcastDateTime", Convert.ToDateTime(r[4].ToString()));
                    sqlComm.Parameters.AddWithValue("@Receipt", r[5].ToString());
                    sqlComm.Parameters.AddWithValue("@DISSEMINATION", r[6].ToString());
                    sqlComm.Parameters.AddWithValue("@Difference", r[7].ToString());
                    sqlComm.Parameters.AddWithValue("@Attachement", r[8].ToString());

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlComm.ExecuteNonQuery();
                    conn.Close();
                }
            }


        }






        //private static string DownloadFile(string MainSite, string action, string Nodes)
        //{
        //    var options = new ChromeOptions
        //    {
        //        BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe",

        //    };

        //    options.AddArgument("--start-maximized");
        //    options.AddArgument("--ignore-certificate-errors");
        //    //options.AddArgument("----incognito");


        //    //options.AddArguments("headless");

        //    var chrome = new ChromeDriver(options);
        //    chrome.Navigate().GoToUrl("http://www.nseindia.com/");
        //    //string html = chrome.PageSource;
        //    // chrome.CloseDevToolsSession();
        //    Thread.Sleep(5000);
        //    //  chrome.Navigate().GoToUrl("http://www.nseindia.com/api/corporate-announcements?index=equities&csv=true");

        //    chrome.Navigate().GoToUrl("http://www.nseindia.com/companies-listing/corporate-filings-announcements");
        //    Thread.Sleep(1000);
        //    chrome.Close();
        //    chrome.Quit();
        //    //chrome.Dispose();
        //    return "";

        //}

        //private static string GetHtml(string MainSite, string action, string Nodes)
        //{
        //    var options = new ChromeOptions
        //    {
        //        BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
        //    };

        //    //options.AddArguments("headless");

        //    var chrome = new ChromeDriver(options);
        //    chrome.Navigate().GoToUrl("https://www.nseindia.com/companies-listing/corporate-filings-announcements");
        //    string html = chrome.PageSource;
        //    // chrome.CloseDevToolsSession();

        //    chrome.Close();
        //    chrome.Quit();
        //    //chrome.Dispose();
        //    return html;

        //}

        //private static string LinkParseHtmlUsingHtmlAgilityPackNSE(string html,
        //    string MainSite, string action, string Nodes)
        //{
        //    var htmlDoc = new HtmlDocument();
        //    htmlDoc.LoadHtml(html);

        //    var repositories =
        //       htmlDoc.DocumentNode
        //           .SelectNodes(Nodes);

        //    foreach (var item in repositories)
        //    {

        //        if (item != null)
        //        {

        //            string link = item.Attributes["href"].Value;
        //            return link;
        //        }
        //    }

        //    return null;
        //}

        //private static List<(string RepositoryName, string Description, string info, string litext)> ParseHtmlUsingHtmlAgilityPack(string html,
        //string MainSite, string action, string Nodes)
        //{
        //    var htmlDoc = new HtmlDocument();
        //    htmlDoc.LoadHtml(html);

        //    var repositories =
        //        htmlDoc.DocumentNode
        //            .SelectNodes(Nodes);

        //    List<(string RepositoryName, string Description, string Date, string litext)> data = new();
        //    if (Nodes.Contains("table"))
        //    {
        //        foreach (var nodes in repositories)
        //        {



        //            var doc = new HtmlAgilityPack.HtmlDocument();
        //            doc.LoadHtml(nodes.InnerHtml);

        //            foreach (var item in doc.DocumentNode.SelectNodes("//a"))
        //            {

        //                if (item != null)
        //                {

        //                    string link = item.Attributes["href"].Value;
        //                    var name = item.InnerText.ToString();
        //                    string info = item.SelectSingleNode("//a").InnerHtml.ToString();
        //                    string litext = item.SelectSingleNode("//a").InnerHtml.ToString();

        //                    data.Add((name, MainSite + link, info, litext));
        //                }
        //            }


        //        }
        //    }
        //    if (Nodes.Contains("ol") || Nodes.Contains("ul"))
        //    {
        //        foreach (var repo in repositories)
        //        {
        //            foreach (var nodes in repo.SelectNodes("li"))
        //            {
        //                var doc = new HtmlAgilityPack.HtmlDocument();
        //                doc.LoadHtml(nodes.InnerHtml);
        //                if (doc.DocumentNode.SelectSingleNode("//a") != null)
        //                {

        //                    string link = doc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value;
        //                    var name = doc.DocumentNode.SelectSingleNode("//a").InnerText.ToString();
        //                    string info = doc.DocumentNode.SelectSingleNode("//a").InnerHtml.ToString();
        //                    string litext = nodes.InnerHtml.ToString();

        //                    data.Add((name, MainSite + link, info, litext));
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var repo in repositories)
        //        {
        //            try
        //            {
        //                try
        //                {
        //                    foreach (var nodes in repo.SelectNodes("p"))
        //                    {
        //                        var doc = new HtmlAgilityPack.HtmlDocument();
        //                        doc.LoadHtml(nodes.InnerHtml);
        //                        if (doc.DocumentNode.SelectSingleNode("//a") != null)
        //                        {

        //                            string link = doc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value;
        //                            var name = doc.DocumentNode.SelectSingleNode("//a").InnerText.ToString();
        //                            string info = doc.DocumentNode.SelectSingleNode("//a").InnerHtml.ToString();
        //                            string litext = nodes.InnerHtml.ToString();

        //                            data.Add((name, MainSite + link, info, litext));
        //                        }
        //                    }
        //                }
        //                catch (Exception)
        //                {


        //                    string litext = repo.InnerHtml;

        //                    data.Add((MainSite, MainSite + action, litext, litext));


        //                }
        //            }
        //            catch (Exception)
        //            {


        //            }
        //        }
        //    }

        //    return data;
        //}
    }
}