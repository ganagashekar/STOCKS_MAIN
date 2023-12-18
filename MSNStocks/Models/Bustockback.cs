using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public partial class Bustockback
    {
        public string? Symbol { get; set; }
        public DateTime? Ltt { get; set; }
        public string? StockName { get; set; }
        public decimal? Last { get; set; }
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Ttv { get; set; }
        public decimal? Low { get; set; }
        public decimal? Change { get; set; }
        public decimal? AvgPrice { get; set; }
        public decimal? BPrice { get; set; }
        public decimal? SPrice { get; set; }
        public decimal? Prev { get; set; }
        public decimal? Ratio { get; set; }
        public string? VolumeC { get; set; }
    }
}
