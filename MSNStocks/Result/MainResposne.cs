using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Result
{
  

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    

    //public class _2023
    //{
    //    public double averagePE { get; set; }
    //    public double bookValuePerShare { get; set; }
    //    public double debtToEquityRatio { get; set; }
    //    public long ebit { get; set; }
    //    public double eps { get; set; }
    //    public long netIncome { get; set; }
    //    public double operatingIncome { get; set; }
    //    public double netProfitMargin { get; set; }
    //    public double priceToBookRatio { get; set; }
    //    public double priceToSalesRatio { get; set; }
    //    public double returnOnAssets { get; set; }
    //    public double returnOnEquity { get; set; }
    //    public long revenue { get; set; }
    //    public double taxRate { get; set; }
    //    public double depreciation { get; set; }
    //    public double interestCoverage { get; set; }
    //    public double assets { get; set; }
    //    public double liabilities { get; set; }
    //    public long longTermDebt { get; set; }
    //    public long sharesOutstanding { get; set; }
    //    public string currency { get; set; }
    //    public string source { get; set; }
    //    public DateTime sourceDate { get; set; }
    //    public DateTime reportDate { get; set; }
    //    public DateTime endDate { get; set; }
    //    public double fiscalYearEndMonth { get; set; }
    //}

    public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
    }

    public class Analysis
    {
       //public //AnnualStatements annualStatements { get; set; }
        public KeyMetrics keyMetrics { get; set; }
        public Estimate estimate { get; set; }
        public IndustryMetrics industryMetrics { get; set; }
        public CompanyMetrics companyMetrics { get; set; }
        public ShareStatistics shareStatistics { get; set; }
    }

    public class AnalystRecommendation
    {
        public double strongBuy { get; set; }
        public double buy { get; set; }
        public double hold { get; set; }
        public double underperform { get; set; }
        public double sell { get; set; }
    }

    //public class AnnualStatements
    //{
    //    [JsonProperty("2000")]
    //    public _2000 _2000 { get; set; }

    //    [JsonProperty("2001")]
    //    public _2001 _2001 { get; set; }

    //    [JsonProperty("2002")]
    //    public _2002 _2002 { get; set; }

    //    [JsonProperty("2003")]
    //    public _2003 _2003 { get; set; }

    //    [JsonProperty("2004")]
    //    public _2004 _2004 { get; set; }

    //    [JsonProperty("2005")]
    //    public _2005 _2005 { get; set; }

    //    [JsonProperty("2006")]
    //    public _2006 _2006 { get; set; }

    //    [JsonProperty("2007")]
    //    public _2007 _2007 { get; set; }

    //    [JsonProperty("2008")]
    //    public _2008 _2008 { get; set; }

    //    [JsonProperty("2009")]
    //    public _2009 _2009 { get; set; }

    //    [JsonProperty("2010")]
    //    public _2010 _2010 { get; set; }

    //    [JsonProperty("2011")]
    //    public _2011 _2011 { get; set; }

    //    [JsonProperty("2012")]
    //    public _2012 _2012 { get; set; }

    //    [JsonProperty("2013")]
    //    public _2013 _2013 { get; set; }

    //    [JsonProperty("2014")]
    //    public _2014 _2014 { get; set; }

    //    [JsonProperty("2015")]
    //    public _2015 _2015 { get; set; }

    //    [JsonProperty("2016")]
    //    public _2016 _2016 { get; set; }

    //    [JsonProperty("2017")]
    //    public _2017 _2017 { get; set; }

    //    [JsonProperty("2018")]
    //    public _2018 _2018 { get; set; }

    //    [JsonProperty("2019")]
    //    public _2019 _2019 { get; set; }

    //    [JsonProperty("2020")]
    //    public _2020 _2020 { get; set; }

    //    [JsonProperty("2021")]
    //    public _2021 _2021 { get; set; }

    //    [JsonProperty("2022")]
    //    public _2022 _2022 { get; set; }

    //    [JsonProperty("2023")]
    //    public _2023 _2023 { get; set; }
    //}

    

    public class Company
    {
        public Address address { get; set; }
        public List<Director> directors { get; set; }
        public string description { get; set; }
        public double employees { get; set; }
        public double establishedYear { get; set; }
        public string industry { get; set; }
        public string name { get; set; }
        public string sector { get; set; }
        public string style { get; set; }
        public string type { get; set; }
        public string website { get; set; }
    }

    public class CompanyMetrics
    {
        public double pE5YearHighRatio { get; set; }
        public double pE5YearLowRatio { get; set; }
        public double revenueYTDYTD { get; set; }
        public double revenueQQLastYearGrowthRate { get; set; }
        public double netIncomeYTDYTDGrowthRate { get; set; }
        public double netIncomeQQLastYearGrowthRate { get; set; }
        public double revenue5YearAverageGrowthRate { get; set; }
        public double interestCoverage { get; set; }
        public double priceCashFlowRatio { get; set; }
        public double revenue3YearAverage { get; set; }
        public double trailingAnnualDividendYield { get; set; }
        public double priceBookRatio { get; set; }
        public double priceSalesRatio { get; set; }
        public double bookValueShareRatio { get; set; }
        public long operatingCashFlow { get; set; }
        public double payoutRatio { get; set; }
        public double quickRatio { get; set; }
        public double current { get; set; }
        public double debtEquityRatio { get; set; }
        public double grossMargin { get; set; }
        public double preTaxMargin { get; set; }
        public double netProfitMargin { get; set; }
        public double averageGrossMargin5Year { get; set; }
        public double averagePreTaxMargin5Year { get; set; }
        public double averageNetProfitMargin5Year { get; set; }
        public double operatingMargin { get; set; }
        public double netMarginPercent { get; set; }
        public double returnOnAssetCurrent { get; set; }
        public double returnOnAsset5YearAverage { get; set; }
        public double returnOnCapitalCurrent { get; set; }
        public double assetTurnover { get; set; }
        public double inventoryTurnover { get; set; }
        public double receivableTurnover { get; set; }
    }

    

    public class Director
    {
        public DateTime asOfDate { get; set; }
        public string name { get; set; }
        public string title { get; set; }
    }

    

    public class Entity
    {
        public string displayName { get; set; }
        public string instrumentId { get; set; }
        public string shortName { get; set; }
        public string securityType { get; set; }
        public string symbol { get; set; }
    }

  

    public class Equity
    {
        public double amountPaid { get; set; }
        public Analysis analysis { get; set; }
        public double beta { get; set; }
        public Company company { get; set; }
        public List<RelatedStock> relatedStocks { get; set; }
        
        public List<object> optionChainMaturityDates { get; set; }
        public string securityDescription { get; set; }
        public string assetCategory { get; set; }
        public string assetClass { get; set; }
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string exchangeName { get; set; }
        public string offeringStatus { get; set; }
        public string displayName { get; set; }
        public string shortName { get; set; }
        public string securityType { get; set; }
        public string instrumentId { get; set; }
        public string symbol { get; set; }
        public string country { get; set; }
        public string market { get; set; }

        public DateTime timeLastUpdated { get; set; }
        public string currency { get; set; }
        public string _p { get; set; }
        public string id { get; set; }
        public string _t { get; set; }
    }

   

    public class Estimate
    {
        public double numberOfAnalysts { get; set; }
        public double recommendationRate { get; set; }
        public string recommendation { get; set; }
        public string currency { get; set; }
        public double numberOfPriceTargets { get; set; }
        public double meanPriceTarget { get; set; }
        public double highPriceTarget { get; set; }
        public double lowPriceTarget { get; set; }
        public double medianPriceTarget { get; set; }
        public double medianEpsTarget { get; set; }
        public AnalystRecommendation analystRecommendation { get; set; }
        public DateTime dateLastUpdated { get; set; }
    }

   
    public class Exchange
    {
        public string exchangeCode { get; set; }
        public string displayName { get; set; }
        //public LocalizedAttributes localizedAttributes { get; set; }
        public string countryCode { get; set; }
        public double delayPeriod { get; set; }
        public bool quotePushAllowed { get; set; }
        public string utcOffset { get; set; }
        public string timeZone { get; set; }
        public string timeZoneDBId { get; set; }
        public string region { get; set; }
        public List<TradingDay> tradingDays { get; set; }
        public string _p { get; set; }
        public string id { get; set; }
        public string _t { get; set; }
        public long _ts { get; set; }
    }

   
    public class IndustryMetrics
    {
        public double assetTurnover { get; set; }
        public double averageGrossMargin5Year { get; set; }
        public double averageNetProfitMargin5Year { get; set; }
        public double averagePreTaxMargin5Year { get; set; }
        public double bookValueShareRatio { get; set; }
        public double currentRatio { get; set; }
        public double debtEquityRatio { get; set; }
        public double dividendYield { get; set; }
        public double dividendYield5YearAverage { get; set; }
        public double grossMargin { get; set; }
        public double incomeEmployee { get; set; }
        public double interestCoverage { get; set; }
        public double inventoryTurnover { get; set; }
        public double netIncomeQQLastYearGrowthRate { get; set; }
        public double netIncomeYTDYTDGrowthRate { get; set; }
        public double netProfitMargin { get; set; }
        public double pEGrowthRatio { get; set; }
        public double preTaxMargin { get; set; }
        public double priceBookRatio { get; set; }
        public double priceCashFlowRatio { get; set; }
        public double priceSalesRatio { get; set; }
        public double quickRatio { get; set; }
        public double returnOnAsset5YearAverage { get; set; }
        public double returnOnAssetCurrent { get; set; }
        public double returnOnCapital5YearAverage { get; set; }
        public double returnOnCapitalCurrent { get; set; }
        public double returnOnEquity5YearAverage { get; set; }
        public double returnOnEquityCurrent { get; set; }
        public double revenueEmployee { get; set; }
        public double revenueQQLastYearGrowthRate { get; set; }
        public double revenueYTDYTD { get; set; }
        public double receivableTurnover { get; set; }
        public double leverageRatio { get; set; }
    }

    

    public class KeyMetrics
    {
        public double debtToEquityRatio { get; set; }
        public double eps { get; set; }
        public double forwardPriceToEPS { get; set; }
        public double payoutRatio { get; set; }
        public double priceToBookRatio { get; set; }
        public string profitability { get; set; }
        public double stockGrowth { get; set; }
        public long latestRevenue { get; set; }
        public long latestIncome { get; set; }
        public double latestNetProfitMargin { get; set; }
        public double latestRevenuePerShare { get; set; }
    }

    

    public class Quote
    {
        public double price { get; set; }
        public double priceChange { get; set; }
        public double priceDayHigh { get; set; }
        public double priceDayLow { get; set; }
        public DateTime timeLastTraded { get; set; }
        public double priceDayOpen { get; set; }
        public double pricePreviousClose { get; set; }
        public DateTime datePreviousClose { get; set; }
        public double priceAsk { get; set; }
        public double askSize { get; set; }
        public double priceBid { get; set; }
        public double bidSize { get; set; }
        public double accumulatedVolume { get; set; }
        public double averageVolume { get; set; }
        public double peRatio { get; set; }
        public double priceChangePercent { get; set; }
        public double price52wHigh { get; set; }
        public double price52wLow { get; set; }
        public double priceClose { get; set; }
        public double yieldPercent { get; set; }
        public double priceChange1Week { get; set; }
        public double priceChange1Month { get; set; }
        public double priceChange3Month { get; set; }
        public double priceChange6Month { get; set; }
        public double priceChangeYTD { get; set; }
        public double priceChange1Year { get; set; }
        public double return1Week { get; set; }
        public double return1Month { get; set; }
        public double return3Month { get; set; }
        public double return6Month { get; set; }
        public double returnYTD { get; set; }
        public double return1Year { get; set; }
        public string sourceExchangeCode { get; set; }
        public string sourceExchangeName { get; set; }
        public long marketCap { get; set; }
        public string marketCapCurrency { get; set; }
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string exchangeName { get; set; }
        public string offeringStatus { get; set; }
        public string displayName { get; set; }
        public string shortName { get; set; }
        public string securityType { get; set; }
        public string instrumentId { get; set; }
        public string symbol { get; set; }
        public string country { get; set; }
        public string market { get; set; }
      
        public DateTime timeLastUpdated { get; set; }
        public string currency { get; set; }
        public string _p { get; set; }
        public string id { get; set; }
        public string _t { get; set; }
    }

    public class RelatedStock
    {
        public string instrumentId { get; set; }
        public string displayName { get; set; }
        public string shortName { get; set; }
        public string exchangeId { get; set; }
        public string exchangeCode { get; set; }
        public string securityType { get; set; }
        public string symbol { get; set; }
    }

    public class MainResposne
    {
        public string instrumentId { get; set; }
        public Quote quote { get; set; }
        public Exchange exchange { get; set; }
        public Equity equity { get; set; }
        public string id { get; set; }
        public string _t { get; set; }
    }

    public class RuRu
    {
        public string displayName { get; set; }
    }

    public class ShareStatistics
    {
        public string lastSplitFactor { get; set; }
        public DateTime lastSplitDate { get; set; }
        public double dividendYield { get; set; }
        public double exDividendAmount { get; set; }
        public double sharesOutstanding { get; set; }
        public double enterpriseValue { get; set; }
    }


    public class TradingDay
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string utcOffset { get; set; }
        public string timeZone { get; set; }
        public List<TradingSession> tradingSessions { get; set; }
    }

    public class TradingSession
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string marketStatus { get; set; }
    }

   


}
