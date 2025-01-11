using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.Deals
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BLOCKDEALSDATum
    {
        public string date { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string clientName { get; set; }
        public string buySell { get; set; }
        public string qty { get; set; }
        public string watp { get; set; }
        public object remarks { get; set; }
    }

    public class BULKDEALSDATum
    {
        public string date { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string clientName { get; set; }
        public string buySell { get; set; }
        public string qty { get; set; }
        public string watp { get; set; }
        public string remarks { get; set; }
    }

    public class NSEALLDEALS
    {
        public string as_on_date { get; set; }
        public List<BULKDEALSDATum> BULK_DEALS_DATA { get; set; }
        public string BULK_DEALS { get; set; }
        public string SHORT_DEALS { get; set; }
        public string BLOCK_DEALS { get; set; }
        public List<SHORTDEALSDATum> SHORT_DEALS_DATA { get; set; }
        public List<BLOCKDEALSDATum> BLOCK_DEALS_DATA { get; set; }
    }

    public class SHORTDEALSDATum
    {
        public string date { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public object clientName { get; set; }
        public object buySell { get; set; }
        public string qty { get; set; }
        public object watp { get; set; }
        public object remarks { get; set; }
    }


}
