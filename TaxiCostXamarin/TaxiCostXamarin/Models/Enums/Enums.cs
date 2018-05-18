using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiCost.Models.Enums
{
    public enum Area
    {
        Athens,
        Islands,
        MainLand
    }

    public enum TimeZone
    {
        Night,
        Day
    }

    public enum PickUpPlace
    {
        None,
        Airport,
        BusStationOrRailStation
    }

    public enum Luggage
    {
        None,
        Heavy,
        Light
    }
}
