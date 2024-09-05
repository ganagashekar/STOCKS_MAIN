using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIVolumeBreakouts.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExgStatus
    {
        public string NSE { get; set; }
        public string BSE { get; set; }
        public string FNO { get; set; }
        public string NDX { get; set; }
    }

    public class ExgTradeDate
    {
        public string NSE { get; set; }
        public string BSE { get; set; }
        public string FNO { get; set; }
        public string NDX { get; set; }
    }

    public class Customer
    {
        public Success Success { get; set; }
        public int Status { get; set; }
        public object Error { get; set; }
    }

    public class SegmentsAllowed
    {
        public string Trading { get; set; }
        public string Equity { get; set; }
        public string Derivatives { get; set; }
        public string Currency { get; set; }
    }

    public class Success
    {
        public ExgTradeDate exg_trade_date { get; set; }
        public ExgStatus exg_status { get; set; }
        public SegmentsAllowed segments_allowed { get; set; }
        public string idirect_userid { get; set; }
        public string session_token { get; set; }
        public string idirect_user_name { get; set; }
        public string idirect_ORD_TYP { get; set; }
        public string idirect_lastlogin_time { get; set; }
        public string mf_holding_mode_popup_flg { get; set; }
        public string commodity_exchange_status { get; set; }
        public string commodity_trade_date { get; set; }
        public string commodity_allowed { get; set; }
    }


}
