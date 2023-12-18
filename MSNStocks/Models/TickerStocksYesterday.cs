using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public partial class TickerStocksYesterday
    {
        public string? Symbol { get; set; }
        public decimal? Open { get; set; }
        public decimal? Last { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Change { get; set; }
        public decimal? BPrice { get; set; }
        public decimal? BQty { get; set; }
        public decimal? SPrice { get; set; }
        public decimal? SQty { get; set; }
        public decimal? Ltq { get; set; }
        public decimal? AvgPrice { get; set; }
        public string? Quotes { get; set; }
        public decimal? Ttq { get; set; }
        public long? TotalBuyQt { get; set; }
        public long? TotalSellQ { get; set; }
        public string? Ttv { get; set; }
        public string? Trend { get; set; }
        public decimal? LowerCktLm { get; set; }
        public decimal? UpperCktLm { get; set; }
        public DateTime? Ltt { get; set; }
        public decimal? Close { get; set; }
        public string? Exchange { get; set; }
        public string? StockName { get; set; }
        public long? RnAsc { get; set; }
        public long? RnDesc { get; set; }
        public string? VolumeC { get; set; }
    }
}
