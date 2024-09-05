using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIVolumeBreakouts.Models.Histrry
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Histry
    {
        public object Error { get; set; }
        public int Status { get; set; }
        public List<Success> Success { get; set; }
    }

    public class Success
    {
        public double close { get; set; }
        public string datetime { get; set; }
        public string exchange_code { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double open { get; set; }
        public string stock_code { get; set; }
        public int volume { get; set; }
    }


}
