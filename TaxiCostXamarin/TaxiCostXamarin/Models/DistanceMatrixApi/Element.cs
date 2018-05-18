using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiCost.Models.DistanceMatrixApi
{
    public class Element
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string Status { get; set; }
    }
}
