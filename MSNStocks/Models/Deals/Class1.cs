using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.Deals
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string _id { get; set; }
        public string BD_DT_DATE { get; set; }
        public string BD_SYMBOL { get; set; }
        public string BD_SCRIP_NAME { get; set; }
        public string BD_CLIENT_NAME { get; set; }
        public string BD_BUY_SELL { get; set; }
        public int BD_QTY_TRD { get; set; }
        public double BD_TP_WATP { get; set; }
        public string BD_REMARKS { get; set; }
        public DateTime TIMESTAMP { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int __v { get; set; }
        public string mTIMESTAMP { get; set; }
    }

    public class Meta
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public List<object> symbols { get; set; }
    }

    public class Deals
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }
    }


}
