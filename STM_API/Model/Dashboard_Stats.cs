namespace STM_API.Model
{
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

    }
}
