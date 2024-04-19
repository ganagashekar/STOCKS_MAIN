using System;
using System.Collections.Generic;

namespace MSNStocks.Models
{
    public class Equities
    {
        public string? msn_secid { get; set; }

        public string symbol { get; set; }
        public double? open { get; set; }
        public double? last { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? change { get; set; }
        public double? bPrice { get; set; }
        public int? bQty { get; set; }
        public double? sPrice { get; set; }
        public int? sQty { get; set; }
        public int? ltq { get; set; }
        public double? avgPrice { get; set; }
        public string quotes { get; set; }
        public int? ttq { get; set; }
        public int? totalBuyQt { get; set; }
        public int? totalSellQ { get; set; }
        public string ttv { get; set; }
        public string trend { get; set; }
        public double? lowerCktLm { get; set; }
        public double? upperCktLm { get; set; }
        public string ltt { get; set; }
        public double? close { get; set; }
        public string exchange { get; set; }
        public string stock_name { get; set; }
        public string VolumeC { set; get; }
        public string OI { get; set; }
        public string CHNGOI { get; set; }
        public string product_type { get; set; }
        public string expiry_date { get; set; }
        public string strike_price { get; set; }
        public string right { get; set; }

        public string SecurityId { get; set; }
    }
    public partial class Equity
    {
        public long Id { get; set; }
        public string SecurityCode { get; set; }
        public string? IssuerName { get; set; }
        public string? SecurityId { get; set; }
        public string? SecurityName { get; set; }
        public string? Status { get; set; }
        public string? Group { get; set; }
        public double? FaceValue { get; set; }
        public string? Isinno { get; set; }
        public string? Industry { get; set; }
        public string? Instrument { get; set; }
        public string? SectorName { get; set; }
        public string? IndustryNewName { get; set; }
        public string? IgroupName { get; set; }
        public string? IsubgroupName { get; set; }
        public string? Symbol { get; set; }
        public bool? IsActive { get; set; }
        public int? Rating { get; set; }
        public string? Recommondations { get; set; }
        public string? MSN_SECID { get; set; }

        public string? JsonData { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public bool? IsLatestQuaterUpdated { get; set; }
        public DateTime? FinancialUpdatedOn { get; set; }
    }
}
