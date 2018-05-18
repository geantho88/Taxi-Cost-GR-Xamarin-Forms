using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;

namespace TaxiCostXamarin
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var from = (App.costCalculation.FromAddressText == "Η τοποθεσία μου (GPS)")? App.costCalculation.FromAddress : App.costCalculation.FromAddressText;
            var to = App.costCalculation.ToAddress;
            string htmlsource = MapString();
            string htmlMap1 = htmlsource.Replace("origin: '',", "origin: '" + from + "',");
            var MapRoute = htmlMap1.Replace("destination: '',", "destination: '" + to + "',");
            var html = new HtmlWebViewSource
            {
                Html = MapRoute
            };

            Browser.Source = html;
            RoutDescription.Text = App.costCalculation.FromAddressText + " - " + App.costCalculation.ToAddress;
            DistanceDescription.Text = App.costCalculation.DistanceText;
            DurationDescription.Text = App.costCalculation.DurationText;
            CostDescription.Text = App.costCalculation.TotalCost + " €";
        }

        public string MapString()
        {
            return
            @"<!DOCTYPE html><html> <head> <meta name=""viewport"" content=""initial-scale=1.0, user-scalable=no""> <meta charset=""utf-8""> <title>Directions service</title> <style> html, body { height: 100%; margin: 0; padding: 0; } #map { height: 100%; } #floating-panel { position: absolute; top: 10px; left: 25%; z-index: 5; background-color: #fff; padding: 5px; border: 1px solid #999; text-align: center; font-family: 'Roboto','sans-serif'; line-height: 30px; padding-left: 10px; } </style> </head> <body> <div id=""map""></div> <script> function initMap() { var directionsService = new google.maps.DirectionsService; var directionsDisplay = new google.maps.DirectionsRenderer; var map = new google.maps.Map(document.getElementById('map'), { preserveViewport : true, center: {lat: 38.89, lng: 24.08} }); directionsDisplay.setMap(map); calculateAndDisplayRoute() ; function calculateAndDisplayRoute() { directionsService.route({ origin: '', destination: '', travelMode: google.maps.TravelMode.DRIVING }, function(response, status) { if (status === google.maps.DirectionsStatus.OK) { directionsDisplay.setDirections(response); } else { window.alert('Directions request failed due to ' + status); } }); } } </script> <script async defer src=""https://maps.googleapis.com/maps/api/js?key=AIzaSyB6d2_0qC97u8wqE-MdeClmMQa5OPzAY6E&callback=initMap""></script> </body></html>";
        }
    }
}
