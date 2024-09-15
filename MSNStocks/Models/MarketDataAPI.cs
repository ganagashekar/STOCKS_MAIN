using System.ComponentModel.DataAnnotations;



public class AO_Depth
{
    [Key]
    public long Id { get; set; }

    public decimal? price { get; set; }

    public long? quantity { get; set; }

    public long? orders { get; set; }

    public string symbolToken { get; set; }

    public string Type {  get; set; }
    public int OrderID { get; set; }

}

public class AOEquities
{
    [Key]
    public long Id { get; set; }

    public string exchange { get; set; }

    public string tradingSymbol { get; set; }

    public string symbolToken { get; set; }

    public decimal? ltp { get; set; }

    public decimal? open { get; set; }

    public decimal? high { get; set; }

    public decimal? low { get; set; }

    public decimal? close { get; set; }

    public decimal? lastTradeQty { get; set; }

    public DateTime? exchFeedTime { get; set; }

    public DateTime? exchTradeTime { get; set; }

    public decimal? netChange { get; set; }

    public decimal? percentChange { get; set; }

    public decimal? avgPrice { get; set; }

    public decimal? tradeVolume { get; set; }

    public decimal? opnInterest { get; set; }

    public decimal? lowerCircuit { get; set; }

    public decimal? upperCircuit { get; set; }

    public decimal? totBuyQuan { get; set; }

    public decimal? totSellQuan { get; set; }

    public decimal? WeekLow52 { get; set; }

    public decimal? WeekHigh52 { get; set; }

    public string symbol { get; set; }

    public string NSECODE { get; set; }

}


