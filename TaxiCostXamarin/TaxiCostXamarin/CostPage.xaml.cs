using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiCost.Models.Enums;
using TaxiCost.Models.ViewModel;
using Xamarin.Forms;

namespace TaxiCostXamarin
{
    public partial class CostPage : ContentPage
    {
        public CostPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadData();
        }

        void LoadData()
        {
            RoutDescription.Text = App.costCalculation.FromAddressText + " - " + App.costCalculation.ToAddress;
            TotalCostLabel.Text = App.costCalculation.TotalCost + " €";
            DurationLabel.Text = App.costCalculation.DurationText;
            DistanceDescription.Text = "Απόσταση "+ App.costCalculation.DistanceText;
            CreateAnalyticCostVM();
        }

        async void MenuButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage(), true);
        }

        async void MapButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage(), true);
        }

        void DetailedCostList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e == null) return;
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null; // de-select the row after ripple effect
        }

        public void CreateAnalyticCostVM()
        {
            ObservableCollection<AnalyticCost> analyticCostList = new ObservableCollection<AnalyticCost>();

            AnalyticCost analyticCost = new AnalyticCost();

            analyticCost.StartPriceLabel = "Εκκίνηση. Πτώση σημαίας";
            analyticCost.StartPriceCost = App.costCalculation.StartPriceCost.ToString() + " €";
            analyticCost.StartPriceIcon = "flag.png";

            analyticCost.TimePerHourText = "Χρόνος αναμονής (Kίνηση - Φανάρια)";
            analyticCost.TimePerHourCost = App.costCalculation.TrafficCost.ToString("0.##") + " €";
            analyticCost.TimePerHourIcon = "clock.png";

            analyticCost.TotalKmText = "Διαδρομή " + App.costCalculation.KmPriceCost.ToString() + " € / χλμ. Χ " + App.costCalculation.DistanceText.ToString();
            analyticCost.TotalKmCost = App.costCalculation.TotalKmCost.ToString("0.##") + " €";
            analyticCost.TotalKmCostIcon = "road.png";


            if (App.costCalculation.CurrentArea == Area.Athens)
            {
                AnalyticDuration.Text = "Διαδρομή " + App.costCalculation.DrivingTime.TotalMinutes.ToString("0.##") + " λεπτά. Αναμονή (φανάρια - κίνηση) " + App.costCalculation.TrafficTime.ToString("0.##")+" λεπτά.";

                var Cleancost = double.Parse(App.costCalculation.TotalCost.ToString()) - double.Parse(App.costCalculation.Vat.ToString());
                AnalyticCost.Text = "Κόστος διαδρομής " + Cleancost + " €. ΦΠΑ διαδρομής " + App.costCalculation.Vat.ToString() + " €";
            }

            analyticCost.LuggageText = "Κόστος αποσκευών / τεμάχιο";
            analyticCost.LuggageIcon = "bags.png";
            if (App.costCalculation.Luggage == Luggage.Heavy)
            {
                analyticCost.LuggageCost = App.costCalculation.LuggageCost.ToString() + " €";
            }
            else
            {
                analyticCost.LuggageCost = "0.00 €";
            }

            analyticCost.PickUpPlaceIcon = "travel.png";
            analyticCost.PickUpPlaceCost = "0.00 €";
            if (App.costCalculation.CurrentArea == Area.Athens)
            {
                analyticCost.PickUpPlaceText = "Παραλαβή απο αεροδρόμιο";
            }
            else
            {
                analyticCost.PickUpPlaceText = "Παραλαβή απο αεροδρόμιο (Επαρχίας)";
            }
            if (App.costCalculation.PickUpPlace == PickUpPlace.Airport)
            {
                analyticCost.PickUpPlaceCost = App.costCalculation.PickUpPlacepCost.ToString() + " €";
            }

            if (App.costCalculation.PickUpPlace == PickUpPlace.BusStationOrRailStation)
            {
                analyticCost.PickUpPlaceText = "Παραλαβή απο Κτελ ή Σιδ. σταθμό";
                analyticCost.PickUpPlaceIcon = "transport.png";
                analyticCost.PickUpPlaceCost = App.costCalculation.PickUpPlacepCost.ToString() + " €";
            }

            if (App.costCalculation.TimeZone == TaxiCost.Models.Enums.TimeZone.Day)
            {
                analyticCost.TimeZoneText = "Μονή ταρίφα. Κόστος χιλιομέτρου";
                analyticCost.TimeZoneIcon = "sun.png";
                analyticCost.TimeZoneCost = App.costCalculation.KmPriceCost.ToString() + " €";
            }

            if (App.costCalculation.TimeZone == TaxiCost.Models.Enums.TimeZone.Night)
            {
                analyticCost.TimeZoneText = "Διπλή ταρίφα. Κόστος χιλιομέτρου";
                analyticCost.TimeZoneIcon = "moon.png";
                analyticCost.TimeZoneCost = App.costCalculation.KmPriceCost.ToString() + " €";
            }

            analyticCostList.Add(analyticCost);
            DetailedCostList.ItemsSource = analyticCostList;
        }
    }
}
