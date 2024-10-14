using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    public class Equities_Volume_Stats
    {
        [Key]
        public long Id { get; set; }
        public string? Symbol { set; get; }
        public decimal? VolumeUPBy_Percentage { get; set; }
        public DateTime? Updated { get; set; }

        public string? Stock_name { get; set; }
        public decimal? LTP { get; set; }
        public decimal? Yesterday_volume { get; set; }
        public decimal? Todays_volume { get; set; }
    }
    public class Equities_Ratings
    {
        [Key]
        public long ID { get; set; }
        public string? Symbol { get; set; }

        public decimal? PIVOT { get; set; }

        public decimal? FR1 { get; set; }

        public decimal? FR2 { get; set; }

        public decimal? FR3 { get; set; }

        public decimal? FS1 { get; set; }

        public decimal? FS2 { get; set; }

        public decimal? FS3 { get; set; }

        public decimal? RSI { get; set; }

        public decimal? MACD { get; set; }

        public decimal? MFI { get; set; }

        public decimal? MCADSIG { get; set; }

        public decimal? ADX { get; set; }

        public decimal? ATR { get; set; }

        public decimal? ROC125 { get; set; }

        public decimal? ROC21 { get; set; }

        public decimal? Williams { get; set; }

        public decimal? Strengths { get; set; }

        public decimal? Weakness { get; set; }

        public decimal? Opportunity { get; set; }

        public decimal? Threats { get; set; }

        public string? FII { get; set; }

        public decimal? FFI_INCREASE { get; set; }

        public string MutaulFOunds { get; set; }

        public decimal? MF_Increase { get; set; }

        public string? SecuirtyId { get; set; }

        public string? Postive { get; set; }
        public string? Negative { get; set; }
        public string companyName { get; set; }
        public DateTime? Updated_on { get; set; }

    }

    //Completion time: 2024-09-01T17:46:04.2279970+05:30

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


        public bool? IsOptions { get; set; }

        public bool? IsNifty { get; set; }

        public bool? IsBankNifty { get; set; }

        public bool? IsSensex { get; set; }

        public bool? IsFinNifty { get; set; }

        public bool? Is52Low { get; set; }

        public bool? Is52High { get; set; }

        public int? MSN_Valuation { get; set; }

        public int? MSN_Health { get; set; }

        public int? MSN_Growth { get; set; }

        public int? MSN_Performance { get; set; }

        public int? MSN_Earnings { get; set; }
    }
}
