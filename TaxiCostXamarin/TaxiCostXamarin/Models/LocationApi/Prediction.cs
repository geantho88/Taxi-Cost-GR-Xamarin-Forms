using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiCost.Models.LocationApi
{
    public class Prediction
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public List<MatchedSubstring> Matched_substrings { get; set; }
        public string Place_id { get; set; }
        public string Reference { get; set; }
        public List<Term> Terms { get; set; }
        public List<string> Types { get; set; }
    }

}
