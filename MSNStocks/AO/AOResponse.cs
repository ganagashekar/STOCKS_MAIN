using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.AO
{
  


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Buy
    {
        public double price { get; set; }
        public int quantity { get; set; }
        public int orders { get; set; }
    }

    public class Data
    {
        public List<Fetched> fetched { get; set; }
        public List<object> unfetched { get; set; }
    }

    public class Depth
    {
        public List<Buy> buy { get; set; }
        public List<Sell> sell { get; set; }
    }

    public class Fetched
    {
        public string exchange { get; set; }
        public string tradingSymbol { get; set; }
        public string symbolToken { get; set; }
        public double ltp { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public int lastTradeQty { get; set; }
        public string exchFeedTime { get; set; }
        public string exchTradeTime { get; set; }
        public double netChange { get; set; }
        public double percentChange { get; set; }
        public double avgPrice { get; set; }
        public int tradeVolume { get; set; }
        public int opnInterest { get; set; }
        public double lowerCircuit { get; set; }
        public double upperCircuit { get; set; }
        public int totBuyQuan { get; set; }
        public int totSellQuan { get; set; }

        [JsonProperty("52WeekLow")]
        public double _52WeekLow { get; set; }

        [JsonProperty("52WeekHigh")]
        public double _52WeekHigh { get; set; }
        public Depth depth { get; set; }
    }

    public class AOResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public Data data { get; set; }
    }

    public class Sell
    {
        public double price { get; set; }
        public int quantity { get; set; }
        public int orders { get; set; }
    }


}
