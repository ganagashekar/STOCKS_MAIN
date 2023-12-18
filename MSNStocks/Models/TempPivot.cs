using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public partial class TempPivot
    {
        public string? Symbol { get; set; }
        public string? StockName { get; set; }
        public decimal Value { get; set; }
        public DateTime? Date { get; set; }
    }
}
