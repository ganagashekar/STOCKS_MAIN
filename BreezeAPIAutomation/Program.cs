//public class Program
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;
using Breeze;
using RestSharp;
using System.Diagnostics;
using System;
using static System.Collections.Specialized.BitVector32;



public class Program
{
    static async Task Main(string[] args)
    {


        try
        {
            string APIKEY = string.Empty;
            string APISecret = string.Empty;
            string token = string.Empty;
            var url = "http://localhost:99/breezeOperation";
            var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string HUbUrl = url;
            

            string[] line;
            string arg = "0";
            if (args.Any())
                arg = args[0];
            switch (0)
            {
                case 0:
                    line = lines[0].ToString().Split(',');
                    APIKEY = line[0];
                    APISecret = line[1];
                    token = line[2];
                    HUbUrl = "http://localhost:8080/BreezeOperation";
                    break;
                case 1:
                    line = lines[1].ToString().Split(',');
                    APIKEY = line[0];
                    APISecret = line[1];
                    token = line[2];
                    HUbUrl = "http://localhost:91/BreezeOperation";
                    break;
                case 2:
                    line = lines[2].ToString().Split(',');
                    APIKEY = line[0];
                    APISecret = line[1];
                    token = line[2];
                    HUbUrl = "http://localhost:92/BreezeOperation";
                    break;
                case 3:
                    line = lines[3].ToString().Split(',');
                    APIKEY = line[0];
                    APISecret = line[1];
                    token = line[2];
                    HUbUrl = "http://localhost:99/BreezeOperation";
                    break;
                case 4:
                    line = lines[4].ToString().Split(',');
                    APIKEY = line[0];
                    APISecret = line[1];
                    token = line[2];
                    HUbUrl = "http://localhost:49/BreezeOperation";
                    break;
            }

            BreezeConnect breeze = new BreezeConnect(APIKEY);
            breeze.generateSessionAsPerVersion(APISecret, token);
            var result = breeze.placeOrder("NIFTY", "NFO", "futures", "buy", "limit", null, "60.85", "50", "DAY",null,null, "02-May-2024","call", "22400",null,null,null);
            string time_stamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
            //string time_stamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
            //"02-May-2024"
            var resultss=breeze.placeOrder("NIFTY","NFO","options","buy","limit","","50","11.25","day",time_stamp,"0","02-May-2024","call","22400","Test","limit","20");
            //breeze.modifyOrder()
            var ressss = resultss;
           //var result= breeze.placeOrder("ICIBAN",
           //           "NFO",
           //           "futures",
           //          "buy",
           //          "limit",
           //          "0",
           //          "15",
           //          "60.85",
           //          "day", null, null, "02-May-2024", "call", "22400", null, null, null
           //           );
            //string time_stamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
            //var client = new RestClient("https://api.icicidirect.com/breezeapi/api/v1/order");
            //var body = @"{
            //" + "\n" +
            //@"    ""stock_code"": ""ITC"",
            //" + "\n" +
            //@"    ""exchange_code"": ""NSE"",
            //" + "\n" +
            //@"    ""product"": ""cash"",
            //" + "\n" +
            //@"    ""action"": ""buy"",
            //" + "\n" +
            //@"    ""order_type"": ""market"",
            //" + "\n" +
            //@"    ""quantity"": ""1"",
            //" + "\n" +
            //@"    ""price"": ""263.15"",
            //" + "\n" +
            //@"    ""validity"": ""ioc""
            //" + "\n" +
            //@"}";
            ////client.Timeout = -1;
            //var request = new RestRequest(Method.Post.ToString());
            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("X-Checksum", "token " + GetCheckSum(APISecret, APIKEY, token) + "\r\n" + body);
            //request.AddHeader("X-Timestamp", time_stamp);
            //request.AddHeader("X-AppKey", APIKEY);
            //request.AddHeader("X-SessionToken", token);
            
            //request.AddParameter("application/json", body, ParameterType.RequestBody);
            //var response = client.Execute(request);
            //Console.WriteLine(response.Content);

        }
        catch (Exception ex)
        {

            throw;
        }


    }

    public static string GetCheckSum(string secret_key, string Appkey, string SessionToken)
    {
        // secret_key = secret_key;

        // 'body' is the request-body of your current request
        var payload = new
        {
            AppKey = Appkey,
            SessionToken = SessionToken
        };
        string payloadJson = JsonSerializer.Serialize(payload);

        //time_stamp & checksum generation for request-headers
        string time_stamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
        //Console.WriteLine(time_stamp);

        string dataToHash = time_stamp + payloadJson + secret_key;
        byte[] dataToHashBytes = Encoding.UTF8.GetBytes(dataToHash);

        string checksum;
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(dataToHashBytes);
            checksum = BitConverter.ToString(hashBytes).Replace("-", "");
        }
        //Console.WriteLine(checksum);
        return checksum;
    }
}



