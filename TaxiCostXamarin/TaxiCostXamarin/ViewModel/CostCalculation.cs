using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TaxiCost.Models.DistanceMatrixApi;
using TaxiCost.Models.Enums;

namespace TaxiCost.ViewModel
{
    public class CostCalculation
    {
        public string FromAddress { get; set; }
        public string FromAddressText { get; set; }
        public string ToAddress { get; set; }
        public string DistanceText { get; set; }
        public double DistanceDouble { get; set; }
        public string DurationText { get; set; }
        public TimeSpan? DurationTime { get; set; }
        public string CurrentAreaText { get; set; }
        public Area CurrentArea { get; set; }
        public string TimeZoneText { get; set; }
        public TaxiCost.Models.Enums.TimeZone TimeZone { get; set; }
        public string PickUpPlaceText { get; set; }
        public PickUpPlace PickUpPlace { get; set; }
        public double PickUpPlacepCost { get; set; }
        public string LuggageText { get; set; }
        public Luggage Luggage { get; set; }
        public double LuggageCost { get; set; }
        public string Awaitingtime { get; set; }
        public double AwaitingtimeCost { get; set; }
        public string KmPrice { get; set; }
        public double KmPriceCost { get; set; }
        public double TotalKmCost { get; set; }
        public string StartPrice { get; set; }
        public double StartPriceCost { get; set; }
        public double LeastCost { get; set; }
        public string TotalCost { get; set; }
        public double TotalDistanceCost { get; set; }
        public string Vat { get; set; }
        public string MapRoute { get; set; }
        public double TimePerHourCost { get; set; }
        public double ActualTimePerHourCost { get; set; }
        public TimeSpan DrivingTime { get; set; }
        public double TrafficTime { get; set; }
        public double TrafficCost { get; set; }


        public void CalculateTotalCost()
        {
            double totalCost = 0.0;
            double vat = 0.0;
            double distance = 0.0;

            if (CurrentArea == Area.Athens)
            {
                StartPriceCost = 1.29;

                // Tarifa Moni - Diplh
                if (TimeZone == TaxiCost.Models.Enums.TimeZone.Day)
                {
                    KmPriceCost = 0.74;
                }

                if (TimeZone == TaxiCost.Models.Enums.TimeZone.Night)
                {
                    KmPriceCost = 1.29;
                }

                // Pickup Place
                if (PickUpPlace == PickUpPlace.Airport)
                {
                    PickUpPlacepCost = 4.18;
                }

                if (PickUpPlace == PickUpPlace.BusStationOrRailStation)
                {
                    PickUpPlacepCost = 1.17;
                }

                if (PickUpPlace == PickUpPlace.None)
                {
                    PickUpPlacepCost = 0.0;
                }

                // Aposkeues
                if (Luggage == Luggage.Heavy)
                {
                    LuggageCost = 0.43;
                }
            }

            if (CurrentArea == Area.MainLand || CurrentArea == Area.Islands)
            {
                StartPriceCost = 1.29;

                // Tarifa Moni - Diplh
                if (TimeZone == TaxiCost.Models.Enums.TimeZone.Day)
                {
                    KmPriceCost = 0.74;
                }

                if (TimeZone == TaxiCost.Models.Enums.TimeZone.Night)
                {
                    KmPriceCost = 1.29;
                }

                // Pickup Place
                if (PickUpPlace == PickUpPlace.Airport)
                {
                    PickUpPlacepCost = 2.83;
                }

                if (PickUpPlace == PickUpPlace.BusStationOrRailStation)
                {
                    PickUpPlacepCost = 1.17;
                }

                if (PickUpPlace == PickUpPlace.None)
                {
                    PickUpPlacepCost = 0.0;
                }

                // Aposkeues
                if (Luggage == Luggage.Heavy)
                {
                    LuggageCost = 0.43;
                }
            }

            string stringReplace = "χλμ";

            distance = double.Parse(DistanceText.Replace(stringReplace, string.Empty).Replace(",", ".").Trim(), CultureInfo.InvariantCulture);
            DistanceDouble = distance;

            if (GetTimeFromRouteresult() != null)
            {
                DurationTime = GetTimeFromRouteresult();
            }

            if (CurrentArea == Area.Athens)
            {
                //calculate waiting time, calculate driving time... speed aprox. 35 kmh/h or 583 meters per minute
                DrivingTime = TimeSpan.FromMinutes((DistanceDouble * 1000) / 583.00);
                TrafficTime = (DurationTime.Value.TotalMinutes - DrivingTime.TotalMinutes);
                TrafficCost = TrafficTime * 0.196;

                totalCost = StartPriceCost + PickUpPlacepCost + LuggageCost + TrafficCost + (distance * KmPriceCost);
            }

            else
            {
                TrafficCost = DurationTime.Value.TotalMinutes * 0.10 * 0.19;
                totalCost = StartPriceCost + PickUpPlacepCost + LuggageCost + (distance * KmPriceCost) + TrafficCost;

            }

            if (CurrentArea == Area.Athens)
            {
                if (totalCost < 3.44)
                {
                    totalCost = 3.44;
                }
            }

            else
            {
                if (totalCost < 3.69)
                {
                    totalCost = 3.69;
                }
            }

            vat = totalCost * 0.23;

            TimePerHourCost = 11.81; // 0.19 lepta ana lepto anamonis 

            // ActualTimePerHourCost = (DurationTime.Value.TotalMinutes * 0.10 * 0.19);

            TotalKmCost = distance * KmPriceCost;

            TotalCost = String.Format("{0:0.##}", totalCost);
            Vat = String.Format("{0:0.##}", vat);
        }


        public TimeSpan? GetTimeFromRouteresult()
        {
            int hours = 0;
            int minutes = 0;
            TimeSpan? duration = null;

            try
            {
                if (DurationText.ToLowerInvariant().Contains("ώρ"))
                {
                    var endHourIndex = DurationText.IndexOf("ώρ");
                    hours = int.Parse(DurationText.Substring(0, endHourIndex).Trim());
                    minutes = int.Parse(DurationText.Substring(6, 3).Trim());
                    duration = new TimeSpan(0, hours, minutes, 0);
                    return duration;
                }

                else
                {
                    minutes = int.Parse(DurationText.Substring(0, 2).Trim());
                    duration = new TimeSpan(0, hours, minutes, 0);

                    return duration;
                }
            }

            catch (Exception exp)
            {
                return null;
            }
        }
    }
}
