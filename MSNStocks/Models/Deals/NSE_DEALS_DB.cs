using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.Deals
{
   

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
