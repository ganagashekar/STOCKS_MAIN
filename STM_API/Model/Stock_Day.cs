namespace STM_API.Model
{
    public class Stock_Days
    {
        public string Symbol { get; set; }

        public long? Days { get; set; }

        public long? BearishCount { get; set; }

        public long? BullishCount { get; set; }

        public long? CUNT { get; set; }

    }

    public class Stock_Days_Results
    {
        public string stock_name { get; set; }

        public double? open { set; get; }
        public double? change { set; get; }

        public double? close { set; get; }
        public long? Days { get; set; }

        public long? BearishCount { get; set; }

        public long? BullishCount { get; set; }

        public string estimate_recommendation { get; set; }
        public string? volumeC { get;  set; }
        public string msn_secid { get; internal set; }
    }
}
