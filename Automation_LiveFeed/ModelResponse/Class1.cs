using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksBuy.model.BreezeModel.Quote
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class QuotesData
    {
        public List<Success> Success { get; set; }
        public int Status { get; set; }
        public object Error { get; set; }
    }

    public class Success
    {
        public string exchange_code { get; set; }
        public string product_type { get; set; }
        public string stock_code { get; set; }
        public string expiry_date { get; set; }
        public string right { get; set; }
        public double strike_price { get; set; }
        public double ltp { get; set; }
        public string ltt { get; set; }
        public double best_bid_price { get; set; }
        public string best_bid_quantity { get; set; }
        public double best_offer_price { get; set; }
        public string best_offer_quantity { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double previous_close { get; set; }
        public double ltp_percent_change { get; set; }
        public double upper_circuit { get; set; }
        public double lower_circuit { get; set; }
        public string total_quantity_traded { get; set; }
        public string spot_price { get; set; }
    }


}
