using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models
{
    public class MSNExchangeStatistics
    {
        public string symbol { get; set; }


        public long Id { get; set; }

        public string CompanyName { get; set; }

        public string MSNID { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Type { get; set; }

    }
}
