using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models
{
    public class Stock_Financial_Results 
    {
        
        public long ID { get; set; }

        public string Stock_Name { get; set; }

        public string Symbol { get; set; }

        public decimal? Revenue { get; set; }

        public decimal? NET_PROFIT { get; set; }

        public decimal? EPS { get; set; }

        public decimal? Cash_EPS { get; set; }

        public decimal? OPM_Percentage { get; set; }

        public decimal? NPM_Percentage { get; set; }

        public decimal? PE_Ratio { get; set; }

        
        public string URL { get; set; }

        public DateTime? QuarterEnd { get; set; }

        public string CurrencyIn { get; set; }

        public DateTime? CREATED_ON { get; set; }

        public DateTime? UPDATED_ON { get; set; }

        [NotMapped]
        public string VType { get; set; }

        public decimal? RevenueIncrease { get; set; }
        public decimal? Profit_Increase { get; set; }
        public decimal? EPS_INcrease { get; set; }

        public decimal? RevenueDifference { get; set; }
        public decimal? ProfitDifference { get; set; }
        public decimal? EPSDifference { get; set; }

    }


    public class Stock_Financial_Results_NSE
    {

        public long ID { get; set; }

        public string? Stock_Name { get; set; }

        public string? Symbol { get; set; }

        public decimal? Revenue { get; set; }

        public decimal? NET_PROFIT { get; set; }

        public decimal? LTP { get; set; }

        public decimal? EPS { get; set; }

        public decimal? Cash_EPS { get; set; }

        public decimal? OPM_Percentage { get; set; }

        public decimal? NPM_Percentage { get; set; }

        public decimal? PE_Ratio { get; set; }


        public string? URL { get; set; }

        public DateTime? QuarterEnd { get; set; }

        public string? CurrencyIn { get; set; }

        public DateTime? CREATED_ON { get; set; }

        public DateTime? UPDATED_ON { get; set; }

        [NotMapped]
        public string VType { get; set; }

        [NotMapped]
        public string Last { get; set; }

        public decimal? RevenueIncrease { get; set; }
        public decimal? Profit_Increase { get; set; }
        public decimal? EPS_INcrease { get; set; }

        public decimal? RevenueDifference { get; set; }
        public decimal? ProfitDifference { get; set; }
        public decimal? EPSDifference { get; set; }



    }


}
