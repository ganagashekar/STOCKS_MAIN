namespace STMAPI.AutomationModels
{
    public  class EquityModelAutomation
    {
       
        public string symbol { set; get; }
      
        public string stockName { set; get; }
       
        public decimal buyATPrice { set; get; }
       
        public decimal? buyATChange { set; get; }
     
        public decimal sellATPrice { set; get; }
       
        public decimal currentPrice { set; get; }

       
        public decimal currentChange { set; get; }

        
        public string stockCode { set; get; }

        
        public int qty { set; get; }

        
        public bool IsBuy { set; get; }

        
        public bool IsSell { set; get; }

        
        public int bgcolor { set; get; }

        
        public string match { set; get; }


        
        public int bullishCount { set; get; }

      

        public int bearishCount { set; get; }

        
        public DateTime lttDateTime { set; get; }

        public string data { set; get; }

        public int bullishCount_100 { set; get; }

        public int bullishCount_95 { set; get; }
        
        public decimal volumeDifferecne { get; set; }

        public decimal? triggredPrice { set; get; }
        public DateTime triggredLtt { set; get; }
    }
}
