using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.Deals
{

    public class NSELOWHIGH
    {
        public long Id { set; get; }
        public string symbol { get; set; }

        public string series { get; set; }

        public string comapnyName { get; set; }

        public decimal new52WHL { get; set; }

        public decimal prev52WHL { get; set; }

        public string prevHLDate { get; set; }

        public decimal ltp { get; set; }

        public decimal prevClose { get; set; }

        public decimal change { get; set; }

        public decimal pChange { get; set; }

        public string Type { get; set; }

        public string CreatedON { get; set; }

    }

    public class NSE_DEALS_DB
    {
        public string Id { get; set; }

        public DateTime? BD_DT_DATE { get; set; }

        public string BD_SYMBOL { get; set; }

        public string BD_SCRIP_NAME { get; set; }

        public string BD_CLIENT_NAME { get; set; }

        public string BD_BUY_SELL { get; set; }

        public int? BD_QTY_TRD { get; set; }

        public decimal? BD_TP_WATP { get; set; }

        public string? BD_REMARKS { get; set; }

        public DateTime? TIMESTAMP { get; set; }

        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

       public string? DEALTYPE { get; set; }

        public string? mTIMESTAMP { get; set; }

    }

    

}
