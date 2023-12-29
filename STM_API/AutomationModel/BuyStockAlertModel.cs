namespace STM_API.AutomationModel
{
    public class BuyStockAlertModel
    {
       
        public string symbol { get; set; }
       
        public string stockName { get; set; }
       
        public decimal buyATPrice { get; set; }
       
        public decimal? buyATChange { get; set; }
        public decimal sellATPrice { get; set; }
       
        public decimal currentPrice { get; set; }
        public decimal currentChange { get; set; }

        public string stockCode { get; set; }
    }

}
