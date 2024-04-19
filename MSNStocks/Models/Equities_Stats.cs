using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models
{
    public class Equities_Stats
    {
        public string StockCode { get; set; }

        public string Symbol { get; set; }
        public string StockName { get; set; }

        public long Id { get; set; }

        public double? Open { get; set; }

        public double? close { get; set; }

        public double? Change { get; set; }

        public double? Volume { get; set; }

        public double? MACD { get; set; }

        public double? RSI { get; set; }

        public string Match { get; set; }

        public DateTime? LTT { get; set; }

        public double? ThreedaysChange { get; set; }
        public double? FivedaysChange { get; set; }
        public double? SevendaysChange { get; set; }
        public double? TendaysChange { get; set; }
        public double? FifteendaysChange { get; set; }


        public double? ThreedaysPriceChange { get; set; }
        public double? FivedaysPriceChange { get; set; }
        public double? SevendaysPriceChange { get; set; }
        public double? TendaysPriceChange { get; set; }
        public double? FifteendaysPriceChange
        {
            get; set;




        }
    }
}
