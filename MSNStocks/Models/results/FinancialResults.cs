using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models.results
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Resultin
    {
        public string FY { get; set; }
        public string LQ { get; set; }
        public string SQ { get; set; }
        public string LFY { get; set; }
        public string LLQ { get; set; }
        public string LSQ { get; set; }
    }

    public class ResultinCr
    {
        public string title { get; set; }
        public string v1 { get; set; }
        public string v2 { get; set; }
        public string v3 { get; set; }
    }

    public class ResultinM
    {
        public string title { get; set; }
        public string v1 { get; set; }
        public string v2 { get; set; }
        public string v3 { get; set; }
    }

    public class FinancialResults
    {
        public string col1 { get; set; }
        public string col2 { get; set; }
        public string col3 { get; set; }
        public string col4 { get; set; }
        public List<ResultinCr> resultinCr { get; set; }
        public List<ResultinM> resultinM { get; set; }
        public List<Resultin> resultinS { get; set; }
    }


}
