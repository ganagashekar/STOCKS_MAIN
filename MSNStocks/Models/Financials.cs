using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSE_Financilas
{
    public class FinancialsModel
    {
     
            public string symbol { get; set; }
            public string companyName { get; set; }
            public string industry { get; set; }
            public string audited { get; set; }
            public string cumulative { get; set; }
            public string indAs { get; set; }
            public string reInd { get; set; }
            public string period { get; set; }
            public string relatingTo { get; set; }
            public string financialYear { get; set; }
            public string filingDate { get; set; }
            public string seqNumber { get; set; }
            public string bank { get; set; }
            public string fromDate { get; set; }
            public string toDate { get; set; }
            public string oldNewFlag { get; set; }
            public string xbrl { get; set; }
            public string format { get; set; }
            public string @params { get; set; }
            public object resultDescription { get; set; }
            public object resultDetailedDataLink { get; set; }
            public string exchdisstime { get; set; }
            public string difference { get; set; }
            public string isin { get; set; }
            public string consolidated { get; set; }
            public string broadCastDate { get; set; }
        
    }
}
