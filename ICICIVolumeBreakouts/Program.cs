using System.Text.Json;
using System.Text;
using System.Security.Cryptography;
using Breeze;
using RestSharp;
using System.Diagnostics;
using System;
using static System.Collections.Specialized.BitVector32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;
using ICICIVolumeBreakouts.Models;
using ICICIVolumeBreakouts.Models.Histrry;
using MSNStocks;

namespace ICICIVolumeBreakouts
{

    //public static class MyListExtensions
    //{
    //    public static IEnumerable<T> GetNth<T>(this List<T> list, int n)
    //    {
    //        for (int i = 0; i < list.Count; i += n)
    //            yield return list[i];
    //    }
    //}
    //public class Program
    //{

    //    public static double ChangeBetweenPercentage(double v1, double v2)
    //    {
    //        return ((v1 - v2) / Math.Abs(v1)) * 100;
    //    }
    //    public static double PriceIncrease(double v1, double v2)
    //    {
    //        return ((v1 - v2));
    //    }

    //    public static double VolumneincreaseInCrores(double v1, double v2)
    //    {
    //        return (v1- v2) / 10000000;
    //    }

    //    static async Task Main(string[] args)
    //    {
    //        try
    //        {
    //            string APIKEY = string.Empty;
    //            string APISecret = string.Empty;
    //            string token = string.Empty;
    //            var url = "http://localhost:99/breezeOperation";
    //            var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
    //            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
    //            string HUbUrl = url;


    //            string[] line;
    //            string arg = "0";
    //            if (args.Any())
    //                arg = args[0];
    //            switch (0)
    //            {
    //                case 0:
    //                    line = lines[0].ToString().Split(',');
    //                    APIKEY = line[0];
    //                    APISecret = line[1];
    //                    token = line[2];
    //                    HUbUrl = "http://localhost:8080/BreezeOperation";
    //                    break;
    //                case 1:
    //                    line = lines[1].ToString().Split(',');
    //                    APIKEY = line[0];
    //                    APISecret = line[1];
    //                    token = line[2];
    //                    HUbUrl = "http://localhost:91/BreezeOperation";
    //                    break;
    //                case 2:
    //                    line = lines[2].ToString().Split(',');
    //                    APIKEY = line[0];
    //                    APISecret = line[1];
    //                    token = line[2];
    //                    HUbUrl = "http://localhost:92/BreezeOperation";
    //                    break;
    //                case 3:
    //                    line = lines[3].ToString().Split(',');
    //                    APIKEY = line[0];
    //                    APISecret = line[1];
    //                    token = line[2];
    //                    HUbUrl = "http://localhost:99/BreezeOperation";
    //                    break;
    //                case 4:
    //                    line = lines[4].ToString().Split(',');
    //                    APIKEY = line[0];
    //                    APISecret = line[1];
    //                    token = line[2];
    //                    HUbUrl = "http://localhost:49/BreezeOperation";
    //                    break;
    //            }


    //                BreezeConnect breeze = new BreezeConnect(APIKEY);

    //            breeze.generateSessionAsPerVersion(APISecret, token);
    //            //var responseObject = await breeze.wsConnectAsync();
    //            //var responseObject=breeze.wsConnectAsyncOhlcv();
    //            //Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(responseObject));
    //            //await breeze.subscribeFeedsAsync("1.1!533022");



    //            //breeze.ticker(async (data) =>
    //            //{
    //            //    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(data));
    //            //});
    //            //Console.ReadLine();

    //            var datatas=(breeze.getHistoricalData(interval: "1minute", fromDate: DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString(), toDate: DateTime.Now.Date.ToString("yyyy-MM-dd").ToString(), stockCode: "LOTUSCHO", exchangeCode: "BSE", productType: "", expiryDate: "2", right: "others", strikePrice: "0"));

    //            var results = breeze.getHistoricalData(interval: "1day", fromDate: DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString(), toDate: DateTime.Now.Date.ToString("yyyy-MM-dd").ToString(), stockCode: "BHEL", exchangeCode: "NSE", productType: "cash", "", "", "");
    //            if (results.FirstOrDefault().Key == "Success")
    //            {
    //                List<HistoricalData> myDeserializedClass = JsonConvert.DeserializeObject<List<HistoricalData>>(results.FirstOrDefault().Value.ToString());
    //                var First = myDeserializedClass[0];
    //                var second = myDeserializedClass[1];
    //                var thrird = myDeserializedClass[2];
    //                var Four = myDeserializedClass[3];
    //                var fifth = myDeserializedClass[4];

    //                var firsdaychange = ChangeBetweenPercentage(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
    //                var secondaychange = ChangeBetweenPercentage(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
    //                var thirdchange = ChangeBetweenPercentage(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
    //                var fourthchange = ChangeBetweenPercentage(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


    //                var firsdaychange_INC = PriceIncrease(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
    //                var secondaychange_INC = PriceIncrease(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
    //                var thirdchange_INC = PriceIncrease(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
    //                var fourthchange_INC = PriceIncrease(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


    //                var firsdaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
    //                var secondaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(thrird.volume), Convert.ToDouble(First.volume));
    //                var thirdchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(Four.volume), Convert.ToDouble(First.volume));
    //                var fourthchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(fifth.volume), Convert.ToDouble(First.volume));

    //                //from var element in myDeserializedClass.GetNth(10) select element;
    //                //var resulss= from var element in myDeserializedClass.get(10) select element;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }
    //}


    class Program
    {
        private static async Task<string> Getdetails(string token, string APIKEY)
        {
            string hostname = "https://api.icicidirect.com/breezeapi/api/v1/";
            string endpoint = "customerdetails";
            string url = $"{hostname}{endpoint}";


            HttpClient client = new HttpClient();

            string jsonPayload = @"{
            ""SessionToken"": ""_token_"",
            ""AppKey"": ""_APIKEY_""
        }";

            StringContent content = new StringContent(jsonPayload.Replace("_token_", token).Replace("_APIKEY_", APIKEY), Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            //request.Headers.Add("Content-Type", "application/json");
            request.Content = content;

            Console.WriteLine("Request headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseBody);
            //return responseBody;
        }
        public static double ChangeBetweenPercentage(double v1, double v2)
        {
            return ((v1 - v2) / Math.Abs(v1)) * 100;
        }
        public static double PriceIncrease(double v1, double v2)
        {
            return ((v1 - v2));
        }

        public static double VolumneincreaseInCrores(double v1, double v2)
        {
            return (v1 - v2) ;
        }

        static async Task Main(string[] args)
        {

            API_MSN_Library.RunEquitiesVolumne();
            //#region working

            //string APIKEY = string.Empty;
            //string APISecret = string.Empty;
            //string token = string.Empty;
            //var url = "http://localhost:99/breezeOperation";
            //var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
            //string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //string HUbUrl = url;


            //string[] line;
            //string arg = "0";
            //if (args.Any())
            //    arg = args[0];
            //switch (0)
            //{
            //    case 0:
            //        line = lines[0].ToString().Split(',');
            //        APIKEY = line[0];
            //        APISecret = line[1];
            //        token = line[2];
            //        HUbUrl = "http://localhost:8080/BreezeOperation";
            //        break;
            //}
            //var bodyresponse = JsonConvert.DeserializeObject<Customer>(Getdetails(token, APIKEY).Result);
            //// App related Secret Key
            //string secretKey = APISecret;

            //// Payload as JSON
            //var payload = System.Text.Json.JsonSerializer.Serialize(new
            //{
            //    AppKey = APIKEY,
            //    SessionToken = token
            //});

            //// Time stamp generation for request-headers
            //string timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
            //Console.WriteLine(timeStamp);

            //// Checksum generation for request-headers
            //string checksumData = timeStamp + payload + secretKey;
            //using var sha256 = SHA256.Create();
            //byte[] checksumBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(checksumData));
            //string checksum = BitConverter.ToString(checksumBytes).Replace("-", "");
            //Console.WriteLine(checksum);



            //using var client = new HttpClient();

            //var queryString = "?stock_code=_stock_code_&exch_code=BSE&from_date=_FromDate_T00:00:00.000Z&to_date=_ToDate_T20:00:00.000Z&interval=1day&product_type=cash&expiry_date=&right=&strike_price=0";

            ////queryString = queryString.Replace("_stock_code_", "FLEXiTUFF").Replace("_FromDate_", DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString()).Replace("_ToDate_", DateTime.Now.Date.ToString("yyyy-MM-dd").ToString());

            ////queryString = queryString.Replace("_stock_code_", "LOTCHO").Replace("_FromDate_", "2024-08-12".ToString()).Replace("_ToDate_", "2024-08-14");


            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri("https://breezeapi.icicidirect.com/api/v2/historicalcharts" + queryString),
            //    Headers =
            //{
            //    { "X-SessionToken", bodyresponse.Success.session_token.ToString() },
            //    { "apikey", APIKEY }
            //},
            //    Content = null
            //};

            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            //var responseBody = await response.Content.ReadAsStringAsync();
            //var results = JsonConvert.DeserializeObject<Histry>(responseBody);

            //if (results.Status.ToString() == "200")
            //{
            //    var result = results.Success.OrderByDescending(x => x.datetime).ToList().Take(2).OrderBy(x=>x.datetime).ToList();


            //    // var results = breeze.getHistoricalData(interval: "1day", fromDate: DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd").ToString(), toDate: DateTime.Now.Date.ToString("yyyy-MM-dd").ToString(), stockCode: "BHEL", exchangeCode: "NSE", productType: "cash", "", "", "");


            //    //  List<HistoricalData> myDeserializedClass = JsonConvert.DeserializeObject<List<Histry>>(result.Success);
            //    var First = result[0];
            //    var second = result[1];
            //    //var thrird = result[2];
            //    //var Four = result[3];
            //    //var fifth = result[4];

            //    var firsdaychange = ChangeBetweenPercentage(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
            //    var firsdaychangeVolume = ChangeBetweenPercentage(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
            //    //var secondaychange = ChangeBetweenPercentage(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
            //    //var thirdchange = ChangeBetweenPercentage(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
            //    //var fourthchange = ChangeBetweenPercentage(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


            //    var firsdaychange_INC = PriceIncrease(Convert.ToDouble(second.close), Convert.ToDouble(First.close));
            //   // var secondaychange_INC = PriceIncrease(Convert.ToDouble(thrird.close), Convert.ToDouble(First.close));
            //    //var thirdchange_INC = PriceIncrease(Convert.ToDouble(Four.close), Convert.ToDouble(First.close));
            //    //var fourthchange_INC = PriceIncrease(Convert.ToDouble(fifth.close), Convert.ToDouble(First.close));


            //    var firsdaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(second.volume), Convert.ToDouble(First.volume));
            //    //var secondaychange_Volu = VolumneincreaseInCrores(Convert.ToDouble(thrird.volume), Convert.ToDouble(First.volume));
            //    //var thirdchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(Four.volume), Convert.ToDouble(First.volume));
            //    //var fourthchange_Volu = VolumneincreaseInCrores(Convert.ToDouble(fifth.volume), Convert.ToDouble(First.volume));

            //    //from var element in myDeserializedClass.GetNth(10) select element;
            //    //var resulss= from var element in myDeserializedClass.get(10) select element;
            //}


            //#endregion
         }
    }
}
