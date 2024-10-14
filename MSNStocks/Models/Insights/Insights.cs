using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.Insights
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Details
    {
        public string propertyKey { get; set; }
        public double propertyValue { get; set; }
        public string benchmarkName { get; set; }
        public double benchmarkValue { get; set; }
        public double difference { get; set; }
        public double differencePercentage { get; set; }
        public string criteria { get; set; }
        public int priority { get; set; }
        public double rankScore { get; set; }
        public string evaluationStatus { get; set; }
    }

    public class Insight
    {
        public string insightId { get; set; }
        public string insightName { get; set; }
        public string category { get; set; }
        public string insightStatement { get; set; }
        public Details details { get; set; }
        public string benchmarkObjectId { get; set; }
        public string benchmarkObjectType { get; set; }
        public string benchmarkObjectName { get; set; }
        public string ruleId { get; set; }
        public DateTime timeLastUpdated { get; set; }
        public string shortInsightStatement { get; set; }
    }

    public class MSNInsights
    {
        public string _t { get; set; }
        public string instrumentId { get; set; }
        public string securityType { get; set; }
        public string symbo { get; set; }
        public string displayName { get; set; }
        public List<Insight> insights { get; set; }
        public string categoryIndex { get; set; }
        public DateTime timeLastUpdated { get; set; }
        public List<string> peers { get; set; }
    }


}
