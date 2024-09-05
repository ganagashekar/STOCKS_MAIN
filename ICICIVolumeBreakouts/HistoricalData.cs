using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIVolumeBreakouts
{
    

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class HistoricalData
    {
        public string datetime { get; set; }
        public string stock_code { get; set; }
        public string exchange_code { get; set; }
        public object product_type { get; set; }
        public object expiry_date { get; set; }
        public object right { get; set; }
        public object strike_price { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string volume { get; set; }
        public object open_interest { get; set; }
        public int count { get; set; }
    }


}
