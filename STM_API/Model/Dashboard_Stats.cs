using System.Runtime.InteropServices;

namespace STM_API.Model
{
    public class NiftyTrader_Stats_DB
    {
        public string puts_builtup { get; set; }
        public int CNTS { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
    }
    public class NiftyTrader_Stats
    {
        public int? Nifty_Put_Buying { get; set; }
        public int? Nifty_Put_Long_Covering { get; set; }
        public int? Nifty_Put_Short_Covering { get; set; }
        public int? Nifty_Put_Writing { set; get; }
        public int? Nifty_Call_Buying { set; get; }
        public int? Nifty_Call_Long_Covering { set; get; }
        public int? Nifty_Call_Short_Covering { set; get; }
        public int? Nifty_Call_Writing { set; get; }


        public int? BANK_Put_Buying { get; set; }
        public int? BANK_Put_Long_Covering { get; set; }
        public int? BANK_Put_Short_Covering { get; set; }
        public int? BANK_Put_Writing { set; get; }
        public int? BANK_Call_Buying { set; get; }
        public int? BANK_Call_Long_Covering { set; get; }
        public int? BANK_Call_Short_Covering { set; get; }
        public int? BANK_Call_Writing { set; get; }
        public string Lastupdated_Nifty_Trader_Date { set; get; }
    }

    public class Dashboard_Stats
    {

        public int? T_Adavcne_Decline { get; set; }
        public double? T_Change { get; set; }
        public int? T_Counts { get; set; }
        public DateTime? T_Date { get; set; }
        public double? T_TotalAvg { get; set; }
        public string T_Type { get; set; }
        public int? y_Adavcne_Decline { get; set; }
        public double? y_Change { get; set; }
        public int? y_Counts { get; set; }
        public DateTime y_Date { get; set; }
        public double? y_TotalAvg { get; set; }
        public string y_Type { get; set; }
    }

    public class Dashboard_Counts_For_QTY
    {

        public double Buy { get; set; }
        public double Sell { get; set; }
        public string Type { get; set; }
        public string Symbol { get; set; }
        public double XTimes { get; set; }
        public double CHG { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }

    public class Maain_Dahsbaord_Stats
    {
        public double Nifty_Current_AvgChange { set; get; }
        public double Nifty_Current_Advance { set; get; }
        public double Nifty_Current_Decline { set; get; }
        public double Nifty_Previous_AvgChange { set; get; }
        public double Nifty_Previous_Advance { set; get; }
        public double Nifty_Previous_Decline { set; get; }
        public string NiftyName { get; set; }
        public double BankNifty_Current_AvgChange { set; get; }
        public double BankNifty_Current_Advance { set; get; }
        public double BankNifty_Current_Decline { set; get; }
        public double BankNifty_Previous_AvgChange { set; get; }
        public double BankNifty_Previous_Advance { set; get; }
        public double BankNifty_Previous_Decline { set; get; }
        public string BankNiftyName { get; set; }
        public double Option_Current_AvgChange { set; get; }
        public double Option_Current_Advance { set; get; }
        public double Option_Current_Decline { set; get; }
        public double Option_Previous_AvgChange { set; get; }
        public double Option_Previous_Advance { set; get; }
        public double Option_Previous_Decline { set; get; }
        public string OptionName { get; set; }
        public double PSU_Current_AvgChange { set; get; }
        public double PSU_Current_Advance { set; get; }
        public double PSU_Current_Decline { set; get; }
        public double PSU_Previous_AvgChange { set; get; }
        public double PSU_Previous_Advance { set; get; }
        public double PSU_Previous_Decline { set; get; }
        public string PSUName { get; set; }
        public double Financials_Advance { get;  set; }
        public double Financials_Decline { get;  set; }
        public double Healthcare_Advance { get;  set; }
        public double Healthcare_Decline { get;  set; }
        public double Industrials_Advance { get;  set; }
        public double Industrials_Decline { get;  set; }
        public double Technology_Advance { get;  set; }
        public double Technology_Decline { get;  set; }
        public double Energy_Advance { get;  set; }
        public double Engery_Decline { get;  set; }
        public double RealEstate_Advance { get;  set; }
        public double RealEstate_Decline { get;  set; }
        public double? TotalSectorAvg { get;  set; }
        public int? TotalSectorAvg_Advance { get;  set; }
        public int? TotalSectorAvg_Decline { get; set; }

        public string LastUpdateDateTime { get; set; }
    }

    public class Dashboard_High_low
    {
        public string OH_OL { set; get; }
        public Double Counts { set; get; }
        public string Type { set; get; }

    }

    public class Dashboard_High_low_Stats
    {
        public int isnifty_lowtrend { set; get; }
        public int isnifty_uptrend { set; get; }
        public int ispsu_lowtrend { set; get; }
        public int ispsu_uptrend { set; get; }
        public int isoptions_lowtrend { set; get; }
        public int isoptions_uptrend { set; get; }
        public int isbanknifty_lowtrend { set; get; }
        public int isbanknifty_uptrend { set; get; }
    }

}
