using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIFTYTraders.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ItmTotalCalls
    {
        public double itm_total_calls_oi { get; set; }
        public double itm_total_calls_change_oi { get; set; }
        public double itm_total_calls_volume { get; set; }
        public double itm_total_calls_oi_value { get; set; }
        public double itm_total_calls_change_oi_value { get; set; }
    }

    public class ItmTotalPuts
    {
        public double itm_total_puts_oi { get; set; }
        public double itm_total_puts_change_oi { get; set; }
        public double itm_total_puts_volume { get; set; }
        public double itm_total_puts_oi_value { get; set; }
        public double itm_total_puts_change_oi_value { get; set; }
    }

    public class OpData
    {
        public double calls_oi { get; set; }
        public double calls_change_oi { get; set; }
        public double calls_volume { get; set; }
        public double calls_iv { get; set; }
        public double calls_ltp { get; set; }
        public double calls_net_change { get; set; }
        public double calls_bid_price { get; set; }
        public double calls_ask_price { get; set; }
        public double calls_open { get; set; }
        public double calls_high { get; set; }
        public double calls_low { get; set; }
        public double calls_offer_price { get; set; }
        public double calls_oi_value { get; set; }
        public double calls_change_oi_value { get; set; }
        public double calls_average_price { get; set; }
        public double previous_eod_calls_oi { get; set; }
        public double calls_change_oi_per { get; set; }
        public double calls_ltp_per { get; set; }
        public double call_delta { get; set; }
        public double call_gamma { get; set; }
        public double call_vega { get; set; }
        public double call_theta { get; set; }
        public double call_rho { get; set; }
        public string calls_builtup { get; set; }
        public double calls_intrisic { get; set; }
        public double calls_time_value { get; set; }
        public int calls_bid_quantity { get; set; }
        public int calls_offer_quantity { get; set; }
        public string symbol_name { get; set; }
        public DateTime expiry_date { get; set; }
        public double strike_price { get; set; }
        public double index_close { get; set; }
        public double pcr { get; set; }
        public double change_oi_pcr { get; set; }
        public double volume_pcr { get; set; }
        public DateTime created_at { get; set; }
        public string time { get; set; }
        public double puts_oi { get; set; }
        public double puts_change_oi { get; set; }
        public double puts_volume { get; set; }
        public double puts_iv { get; set; }
        public double puts_ltp { get; set; }
        public double puts_net_change { get; set; }
        public double puts_bid_price { get; set; }
        public double puts_ask_price { get; set; }
        public double puts_open { get; set; }
        public double puts_high { get; set; }
        public double puts_low { get; set; }
        public double puts_offer_price { get; set; }
        public double puts_oi_value { get; set; }
        public double puts_change_oi_value { get; set; }
        public double puts_average_price { get; set; }
        public double previous_eod_puts_oi { get; set; }
        public double puts_change_oi_per { get; set; }
        public double puts_ltp_per { get; set; }
        public double put_delta { get; set; }
        public double put_gamma { get; set; }
        public double put_vega { get; set; }
        public double put_theta { get; set; }
        public double put_rho { get; set; }
        public string puts_builtup { get; set; }
        public double puts_intrisic { get; set; }
        public double puts_time_value { get; set; }
        public int puts_bid_quantity { get; set; }
        public int puts_offer_quantity { get; set; }
    }

    public class OpTotals
    {
        public ItmTotalCalls itm_total_calls { get; set; }
        public ItmTotalPuts itm_total_puts { get; set; }
        public OtmTotalCalls otm_total_calls { get; set; }
        public OtmTotalPuts otm_total_puts { get; set; }
        public TotalCallsPuts total_calls_puts { get; set; }
    }

    public class OtmTotalCalls
    {
        public double otm_total_calls_oi { get; set; }
        public double otm_total_calls_change_oi { get; set; }
        public double otm_total_calls_volume { get; set; }
        public double otm_total_calls_oi_value { get; set; }
        public double otm_total_calls_change_oi_value { get; set; }
    }

    public class OtmTotalPuts
    {
        public double otm_total_puts_oi { get; set; }
        public double otm_total_puts_change_oi { get; set; }
        public double otm_total_puts_volume { get; set; }
        public double otm_total_puts_oi_value { get; set; }
        public double otm_total_puts_change_oi_value { get; set; }
    }

    public class ResultData
    {
        public List<OpData> opDatas { get; set; }
        public OpTotals opTotals { get; set; }
    }

    public class NiftyTrader
    {
        public int result { get; set; }
        public string resultMessage { get; set; }
        public ResultData resultData { get; set; }
    }

    public class TotalCallsPuts
    {
        public double total_calls_oi { get; set; }
        public double total_calls_change_oi { get; set; }
        public double total_calls_volume { get; set; }
        public double total_calls_oi_value { get; set; }
        public double total_calls_change_oi_value { get; set; }
        public double total_puts_oi { get; set; }
        public double total_puts_change_oi { get; set; }
        public double total_puts_volume { get; set; }
        public double total_puts_oi_value { get; set; }
        public double total_puts_change_oi_value { get; set; }
    }


}
