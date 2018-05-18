using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiCost.Models.LocationApi
{
    public class ApiLocationQueryResult
    {
        public string Status { get; set; }
        public List<Prediction> Predictions { get; set; } 
    }
}
