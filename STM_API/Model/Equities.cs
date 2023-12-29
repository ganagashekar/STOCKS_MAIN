namespace STM_API.Model
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
        public int bQty { get; set; }
        public double? sPrice { get; set; }
        public int sQty { get; set; }
        public int ltq { get; set; }
        public double? avgPrice { get; set; }
        public string quotes { get; set; }
        public int ttq { get; set; }
        public int totalBuyQt { get; set; }
        public int totalSellQ { get; set; }
        public string ttv { get; set; }
        public string trend { get; set; }
        public double? lowerCktLm { get; set; }
        public double? upperCktLm { get; set; }
        public string ltt { get; set; }
        public double? close { get; set; }
        public string exchange { get; set; }
        public string stock_name { get; set; }
        public string VolumeC { set; get; }
    }

    public class ChartData
    {
        public double value { get; set; }
        public string extremum { get; set; }
    }
    public class EquitiesHsitry
    {
        public string? msn_secid { get; set; }

        public string symbol { get; set; }
        public double? open { get; set; }
        public double? last { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? change { get; set; }
        public double? bPrice { get; set; }
        public int bQty { get; set; }
        public double? sPrice { get; set; }
        public int sQty { get; set; }
        public int ltq { get; set; }
        public double? avgPrice { get; set; }
        public string quotes { get; set; }
        public int ttq { get; set; }
        public int totalBuyQt { get; set; }
        public int totalSellQ { get; set; }
        public string ttv { get; set; }
        public string trend { get; set; }
        public double? lowerCktLm { get; set; }
        public double? upperCktLm { get; set; }
        public string ltt { get; set; }
        public double? close { get; set; }
        public string exchange { get; set; }
        public string stock_name { get; set; }
        public string securityId { get; set; }
        public string VolumeC { set; get; }

        public List<double> Data { get; set; }

        public object DataPoint { get; set; }

        public string href { get; set; }
        public string stockdetailshref { get; set; }

        public string SECId { get; set; }

        public int min { set; get; }
        public int max { set;get; }

        public double return1d { set; get; }
        public double return1w { set; get; }
        public double return1m { set; get; }
        public double return3m { set; get; }
        public double return6m { set; get; }
        public double return1Year { set; get; }
        public double returnYTD { set; get; }

        public string recmdtn { get; set; }
        public double noofrec { set; get; }
        public string beta { get; set; }
        public string eps { get; set; }
        public string target { get; set; }
        public bool isfavorite { get; set; }
        public double priceChange_Day { set; get; }
        public double priceChange_1w { set; get; }
        public double priceChange_1m { set; get; }
        public double priceChange_3m { set; get; }
        public double priceChange_6m { set; get; }
        public double priceChange_1year { set; get; }
        public double priceChange_YTD { set; get; }
        public double price52Weekslow { set; get; }
        public double price52Weekshigh { set; get; }
        public bool IsLowerCircuite { get; set; }
        public bool IsUpperCircuite { get;set; }

        public double buyat { set; get; }
        public string? buyatChange { set; get; }
        public bool isenabledforautoTrade { get; set; }

        public string tdays { set; get; } 
        public string WacthList { set; get; }   

        public string pr_change { set; get; }   
        public string pr_close { set; get; }
        public string pr_open { set; get;}  

        public string pr_volume { set; get;}
        public string pr_date { set; get; }
    }

    public class StockBuy
    {
        public string symbol { get; set; }
        public double open { get; set; }
        public double last { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double change { get; set; }
        public double close { get; set; }

        public string ltt { get; set; }
        public double avgPrice { get; set; }
        
        public double   ttv { get; set; }
        public double prev { get; set; }
        public double Ratio { get; set; }

        public string stock_name { get; set; }

        public double bPrice { get; set; }
        public double sPrice { get; set; }

        public string VolumeC { get; set; }

        public double INC { set; get; }

    }
}
