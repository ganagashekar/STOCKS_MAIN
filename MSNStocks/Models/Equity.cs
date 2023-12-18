using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public partial class Equity
    {
        public double? SecurityCode { get; set; }
        public string? IssuerName { get; set; }
        public string? SecurityId { get; set; }
        public string? SecurityName { get; set; }
        public string? Status { get; set; }
        public string? Group { get; set; }
        public double? FaceValue { get; set; }
        public string? IsinNo { get; set; }
        public string? Industry { get; set; }
        public string? Instrument { get; set; }
        public string? SectorName { get; set; }
        public string? IndustryNewName { get; set; }
        public string? IgroupName { get; set; }
        public string? IsubgroupName { get; set; }
        public long Id { get; set; }
    }
}
