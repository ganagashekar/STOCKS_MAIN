using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCKT
{
    public  class STOCK_CKT
    {

        public decimal last { set; get; }
        public string stock_name { set; get; }

        public string symbol { set; get; }

        public DateTime LTT { set; get; }
        public string CurrentCHG { set; get; }
        public int signal { set; get; }
        public decimal ttv { set; get; }
        public string VolumeC { set; get; }

        public string Previous_Change { set; get; }
        public int IsSame { set; get; }
        public decimal ttq { get; internal set; }
    }


    public class ListOfCKT
    {
       

        public string Symbol { set; get; }
        public int DaysLWR_CKT { set; get; }
        public int DaysUPR_CKT { set; get; }

        public bool IsInLowerCKT { set; get; }

        public int DifferenceOfLWR_TO_UpperCKT { set; get; }

        public string Stock_name { set; get; }
        public decimal last { get; internal set; }
        public DateTime ltt { get; internal set; }

        public decimal TTV { set; get; }
        public string TTVC { set; get; }
    }

}
