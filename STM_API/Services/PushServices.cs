using System.Security.Cryptography.X509Certificates;

namespace STM_API.Services
{
    public class PushServices
    {
        public PushServices() { }

        public async Task SendPushServicesAsync(string tittle, string token, string user, string priority, string message, string retry, string expire, string sound)
        {
            //var iphonelis = new List<string>() { "SELL_STOCK_DOWN", "Orders", "BSE_NEWS", "IPO_UpComming", "IPO_UpComming", "IPO_Current" };
            var iphonelis = new List<string>() { "SELL_STOCK_DOWN", "Orders" };
            var parameters = new Dictionary<string, string>
            {
                ["token"] = token,
                ["user"] = user,
                ["priority"] = priority,
                ["message"] = message,
                ["title"] = tittle,
                ["retry"] = retry,
                ["expire"] = expire,
                ["html"] = "1",
                ["sound"] = sound,
                ["device"] = iphonelis.Contains(tittle.ToString()) ? "iphone" : "ipad"
            };

            using var client = new HttpClient();
            var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
            FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
        }

        public async Task SendPushServicesAsyncASAP(string stockName, string volume, string price)
        {
            //var iphonelis = new List<string>() { "SELL_STOCK_DOWN", "BSE_NEWS", "IPO_UpComming", "IPO_UpComming", "IPO_Current" };
            //var iphonelis = new List<string>() { "SELL_STOCK_DOWN" };
            var parameters = new Dictionary<string, string>
            {
                ["token"] = "ambc8ipagkeeqcw89u1j6wyaofcdq9",
                ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                ["priority"] = "1",
                ["message"] = "This stock is raising with 100 Bullish " + stockName + " difference in lakhs " + Convert.ToDecimal(volume) / 100000 + " , Triggerred Price" + price,
                ["title"] = "Bullish_100",
                ["retry"] = "30",
                ["expire"] = "300",
                ["html"] = "1",
                ["sound"] = "echo",
                ["device"] = "ipad"
            };

            using var client = new HttpClient();
            var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
            FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
        }

        public async Task SendPushServicesAsyncASAPMsg(string tittle, string message)
        {
            //var iphonelis = new List<string>() { "SELL_STOCK_DOWN", "BSE_NEWS", "IPO_UpComming", "IPO_UpComming", "IPO_Current" };
            //var iphonelis = new List<string>() { "SELL_STOCK_DOWN" };
            var parameters = new Dictionary<string, string>
            {
                ["token"] = "ajd8p1o564xjvq9rydmfrupgzcbgsk",
                ["user"] = "uh61jjrcvyy1tebgv184u67jr2r36x",
                ["priority"] = "1",
                ["message"] = message,
                ["title"] = tittle,
                ["retry"] = "30",
                ["expire"] = "300",
                ["html"] = "1",
                ["sound"] = "echo",
                ["device"] = "iphone"
            };

            using var client = new HttpClient();
            var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
            FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
        }


    }
}
