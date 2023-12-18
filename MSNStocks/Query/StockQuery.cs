using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Query
{
    

    public class Data
    {
        public List<string> stocks { get; set; }
    }

    public class StockQuery
    {
        public int count { get; set; }
        public Data data { get; set; }
    }

    public class AC042Indexs
    {
        public string AC042Index { get; set; }
    }

    public class StockQueryFirst
    {
        public string RT00S { get; set; }
        public string RT00SIndex { get; set; }
        public string OS001 { get; set; }
        public string OS001Index { get; set; }
        public string OS01W { get; set; }
        public string OS01WIndex { get; set; }
        public string RT0SN { get; set; }
        public string RT0SNIndex { get; set; }
        public string FullInstrument { get; set; }
        public string OS0LN { get; set; }
        public string OS0LNIndex { get; set; }
        public string OS010 { get; set; }
        public string OS01V { get; set; }
        public string RT00T { get; set; }
        public string RT0EC { get; set; }
        public string ExMicCode { get; set; }
        public string OS05J { get; set; }
        public string OS05JIndex { get; set; }
        public string AC040 { get; set; }
        public string RT00E { get; set; }
        public string LS01Z { get; set; }
        public string FriendlyName { get; set; }
        public string DisplayName { get; set; }
        public string SecId { get; set; }
        public int rankLocal { get; set; }
        public int rankGlobal { get; set; }
        public string AC042 { get; set; }
        public List<AC042Indexs> AC042Index { get; set; }
        public string Description { get; set; }
        public string locale { get; set; }
        public string insertTime { get; set; }
        public List<object> AliasIndex { get; set; }
    }


}
