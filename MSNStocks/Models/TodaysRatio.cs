using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public partial class TodaysRatio
    {
        public string? Symbol { get; set; }
        public DateTime? Ltt { get; set; }
        public string? StockName { get; set; }
        public decimal? Last { get; set; }
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public string? Ttv { get; set; }
        public string? Prev { get; set; }
        public decimal? Ratio { get; set; }
        public string? VolumeC { get; set; }
    }
}
