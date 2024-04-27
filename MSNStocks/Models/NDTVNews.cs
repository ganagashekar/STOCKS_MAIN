using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNStocks.Models
{
    public class NDTVNews
    {
        public long Id { get; set; }
        public string ContentId { get; set; }
        public string Headline { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
