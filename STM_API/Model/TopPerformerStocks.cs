namespace STM_API.Model
{
    public class TopPerformerStocks
    {

        public string stock_name { set; get; }
        public double Volume { set; get; }
        public double min_last { set; get; }
        public double avg { set; get; }
        public double max_last { set; get; }
        public  double Open { set; get; }

        public double min_change { set; get; }
        public double max_change { set; get; }
        public double min_bPrice { set; get; }
        public double max_bPrice { set; get; }
        public double min_sPrice { set; get; }
        public double max_sPrice { set; get; }
         public double bPrice { set; get; }
        public double sPrice { set; get; }


        public string symbol { set; get; }

        public string volumeC { set; get; }

    }
}
