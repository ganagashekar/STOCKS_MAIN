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

        public string? MutaulFOunds { get; set; }

        public decimal? MF_Increase { get; set; }

        public string? SecuirtyId { get; set; }

        public string? Postive { get; set; }
        public string? Negative { get; set; }
        public string? companyName { get; set; }
        public DateTime? Updated_on { get; set; }

        public string? ActiveCandle { set; get; }    

        public decimal? GAIN_FROM_LOW_52 { get; set; }
        public decimal? FALL_FROM_LOW_52 { get; set; }

        public decimal? EMA_5 { get; set; }
        public decimal? EMA_10 { get; set; }
        public decimal? EMA_12 { get; set; }
        public decimal? EMA_20 { get; set; }
        public decimal? EMA_26 { get; set; }
        public decimal? EMA_50 { get; set; }
        public decimal? EMA_100 { get; set; }
        public decimal? EMA_200 { get; set; }
        public decimal? SMA_5 { get; set; }
        public decimal? SMA_10 { get; set; }
        public decimal? SMA_20 { get; set; }
        public decimal? SMA_30 { get; set; }
        public decimal? SMA_150 { get; set; }
        public decimal? SMA_50 { get; set; }
        public decimal? SMA_100 { get; set; }
        public decimal? SMA_200 { get; set; }
        public decimal? Return_YTD { get; set; }
        public decimal? Return_1week { get; set; }
        public decimal? Return_1Month { get; set; }
        public decimal? Return_3Month { get; set; }
        public decimal? Return_6Month { get; set; }
        public decimal? Return_1Year { get; set; }
        public decimal? DayDelievery_Volume { get; set; }
        public decimal? WeekDelievery_Volume { get; set; }
        public decimal? MonthDelievery_Volume { get; set; }

    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Equities_Ratings_Model
    {
        public string PIVOT { get; set; }
        public string companyName { get; set; }
        public string FR1 { get; set; }
        public string FR2 { get; set; }
        public string FR3 { get; set; }
        public string FS1 { get; set; }
        public string FS2 { get; set; }
        public string FS3 { get; set; }
        public string RSI { get; set; }
        public string MACD { get; set; }
        public string MFI { get; set; }
        public string MCADSIG { get; set; }
        public string ADX { get; set; }
        public string ATR { get; set; }
        public string ROC125 { get; set; }
        public string ROC21 { get; set; }
        public string Williams { get; set; }
        public string Strengths { get; set; }
        public string Weakness { get; set; }
        public string Opportunity { get; set; }
        public string Threats { get; set; }
        public string Postive { get; set; }
        public string Negative { get; set; }
        public string ema_5day { get; set; }
        public string ema_10day { get; set; }
        public string ema_12day { get; set; }
        public string ema_20day { get; set; }
        public string ema_26day { get; set; }
        public string ema_50day { get; set; }
        public string ema_100day { get; set; }
        public string ema_200day { get; set; }
        public string sma_5day { get; set; }
        public string sma_10day { get; set; }
        public string sma_12day { get; set; }
        public string sma_20day { get; set; }
        public string sma_26day { get; set; }
        public string sma_50day { get; set; }
        public string sma_100day { get; set; }
        public string sma_200day { get; set; }
        public string loworHigh52 { get; set; }
        public string delivery_1day_Vol { get; set; }
        public string delivery_1week_Vol { get; set; }
        public string delivery_1Month_Vol { get; set; }
        public string return_1Month { get; set; }
        public string return_3Month { get; set; }
        public string return_6Month { get; set; }
        public string return_1year { get; set; }
        public string ActiveCandle { get; set; }
        public string sma_30day { get;  set; }
        public object sma_150day { get; internal set; }
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
        public string? exchange { get; set; }   
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

        public decimal? Week52High { set; get; }
        public decimal? Week52low { set; get; }

        public DateTime? LTT { set; get; }
        public decimal? LTP { set; get; }

        public decimal? ChangeOfNow { set; get; }
    }
}
