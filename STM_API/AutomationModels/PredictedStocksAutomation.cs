namespace STM_API.AutomationModels
{
    public class PredictedStocksAutomation
    {
        public string symbol { get; set; }
        public int bulishCount { get; set; }
        public int bearishCount { get; set; }

        public DateTime ltt { get; set; }

        public double? candleResult_Price { get; set; }

        public double? candleResult_Match { get; set; }

        public double? candleResult_Size { get; set; }

        public double? candleResult_Body { get; set; }

        public double? candleResult_UpperWick { get; set; }

        public double? candleResult_LowerWick { get; set; }

        public double? candleResult_BodyPct { get; set; }

        public double? candleResult_UpperWickPct { get; set; }

        public double? candleResult_LowerWickPct { get; set; }

        public bool candleResult_IsBullish { get; set; }

        public bool candleResult_IsBearish { get; set; }

        public double? candleResult_Volume { get; set; }

        public double? macdresult_Macd { get; set; }

        public double? macdresult_Signal { get; set; }

        public double? macdresult_FastEma { get; set; }

        public double? macdresult_SlowEma { get; set; }

        public double? macdresult_Rsi { get; set; }

        public double? Volatilityresults_Sar { get; set; }

        public double? Volatilityresults_UpperBand { get; set; }

        public double? Volatilityresults_LowerBand { get; set; }

        public string Volatilityresults_IsStop { get; set; }

    }

}
