using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public partial class TempPivotResult
    {
        public string? Symbol { get; set; }
        public string? StockName { get; set; }
        public decimal? _20230901 { get; set; }
        public decimal? _20230904 { get; set; }
        public decimal? _20230905 { get; set; }
    }
}
