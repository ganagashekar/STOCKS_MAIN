using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSEBlockDeals.HIGH
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string symbol { get; set; }
        public string series { get; set; }
        public string comapnyName { get; set; }
        public decimal new52WHL { get; set; }
        public decimal prev52WHL { get; set; }
        public string prevHLDate { get; set; }
        public decimal ltp { get; set; }
        public string prevClose { get; set; }
        public decimal change { get; set; }
        public decimal pChange { get; set; }
    }

    public class NSE_lowHigh
    {
        public int high { get; set; }
        public int low { get; set; }
        public List<Datum> data { get; set; }
        public string timestamp { get; set; }
    }


}
