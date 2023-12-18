namespace STM_API.Services
{
    public class PushServices
    {
        public PushServices() { }

        public  async Task SendPushServicesAsync(string tittle,string token,string user,string priority,string message,string retry,string expire,string sound )
        {
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
                ["sound"] = sound
            };
            using var client = new HttpClient();
            var response = await client.PostAsync("https://api.pushover.net/1/messages.json", new
            FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
        }

        
    }
}
