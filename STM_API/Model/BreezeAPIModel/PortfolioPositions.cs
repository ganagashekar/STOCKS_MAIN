namespace STM_API.Model.BreezeAPIModel
{
    

    public class PortfolioPositions
    {
        public string segment { get; set; }
        public string product_type { get; set; }
        public string exchange_code { get; set; }
        public string stock_code { get; set; }
        public object expiry_date { get; set; }
        public object strike_price { get; set; }
        public object right { get; set; }
        public string action { get; set; }
        public string quantity { get; set; }
        public string average_price { get; set; }
        public string settlement_id { get; set; }
        public string margin_amount { get; set; }
        public string ltp { get; set; }
        public string price { get; set; }
        public object stock_index_indicator { get; set; }
        public string cover_quantity { get; set; }
        public object stoploss_trigger { get; set; }
        public string stoploss { get; set; }
        public string take_profit { get; set; }
        public string available_margin { get; set; }
        public string squareoff_mode { get; set; }
        public string mtf_sell_quantity { get; set; }
        public string mtf_net_amount_payable { get; set; }
        public string mtf_expiry_date { get; set; }
        public string order_id { get; set; }
        public string cover_order_flow { get; set; }
        public string cover_order_executed_quantity { get; set; }
        public string pledge_status { get; set; }
        public string pnl { get; set; }
        public object underlying { get; set; }
        public string order_segment_code { get; set; }
    }

    
}
