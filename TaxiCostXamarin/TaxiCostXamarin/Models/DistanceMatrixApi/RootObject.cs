using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiCost.Models.DistanceMatrixApi
{
    public class RootObject
    {
        public List<string> Destination_addresses { get; set; }
        public List<string> Origin_addresses { get; set; }
        public List<Row> Rows { get; set; }
        public string Status { get; set; }
    }
}
