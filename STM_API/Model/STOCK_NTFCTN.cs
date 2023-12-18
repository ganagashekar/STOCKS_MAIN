using System.Security.Cryptography.X509Certificates;

namespace STM_API.Model
{
    public class STOCK_NTFCTN
    {
        public decimal Last { set; get; }
        public string symbol { set; get; }
        public DateTime Date { set; get; }
        public string STOCKName { set; get; }
        public bool IsNotified { set; get; }
        public bool IsUppCKT { set; get; }
        public bool ISSell { set; get; }
        public bool ISPrict { set; get; }
        public decimal Change { set; get; } 

        public string PO_KEY_NAME { get; set; }

        public string PO_KEY_TOKEN { get; set; }

        public string PO_DESCRIPTION { get; set; }

        public string user { get; set; }

        public int? priority { get; set; }

        public string title { get; set; }

        public int? retry { get; set; }

        public int? expire { get; set; }

        public string sound { get; set; }
        public long Id { get; internal set; }
    }
}
