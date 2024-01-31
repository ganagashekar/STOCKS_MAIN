using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPOS.Models
{
    public class ipo_current_issue
    {
        public long Id { get; set; }

        public string symbol { get; set; }

        public string companyName { get; set; }

        public string series { get; set; }

        public string issueStartDate { get; set; }

        public string issueEndDate { get; set; }

        public string status { get; set; }

        public int? issueSize { get; set; }

        public string issuePrice { get; set; }

        public string srNo { get; set; }

        public string category { get; set; }

        public string noOfSharesOffered { get; set; }

        public string noOfsharesBid { get; set; }

        public decimal noOfTime { get; set; }

        public string isBse { get; set; }

    }

    public class ipo_Upcomming
    {
        public long Id { get; set; }

        public string symbol { get; set; }

        public string companyName { get; set; }

        public string series { get; set; }

        public string issueStartDate { get; set; }

        public string issueEndDate { get; set; }

        public string status { get; set; }

        public int? issueSize { get; set; }

        public string issuePrice { get; set; }

        public int? sr_no { get; set; }

        public string isBse { get; set; }

        public int? lotSize { get; set; }

        public string priceBand { get; set; }

        public string filename { get; set; }

    }
}
