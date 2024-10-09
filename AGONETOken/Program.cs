using AngelBroking;
using Newtonsoft.Json;


namespace AGONETOken
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            string Client_code = "A60479487";  //YOUR CLIENT CODE
            string Password = "2711"; //YOUR PASSWORD
            string api_key = "E8kj0Y88";
            string JWTToken = "";  // optional
            string RefreshToken = ""; // optional

            Console.WriteLine("Please Enter token");
           string key= Console.ReadLine();
            //var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\angel.txt");
            //string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //string HUbUrl = url;


            //string[] line;
            //string arg = "0";


            SmartApi connect = new SmartApi(api_key, JWTToken, RefreshToken);

            OutputBaseClass obj = new OutputBaseClass();

            //Login by client code and password
            obj = connect.GenerateSession(Client_code, Password, key);
            AngelToken agr = obj.TokenResponse;

            Console.WriteLine("------GenerateSession call output-------------");
           Console.WriteLine(JsonConvert.SerializeObject(agr));
            Console.WriteLine("----------------------------------------------");

            //Get Token
            obj = connect.GenerateToken();
            agr = obj.TokenResponse;

            Console.WriteLine("------GenerateToken call output-------------");
           Console.WriteLine(JsonConvert.SerializeObject(agr));
            Console.WriteLine("----------------------------------------------");
            
            System.IO.File.WriteAllText("C:\\Hosts\\ICICI_Key\\angel.txt", JsonConvert.SerializeObject(obj.TokenResponse));
        }
    }
}
