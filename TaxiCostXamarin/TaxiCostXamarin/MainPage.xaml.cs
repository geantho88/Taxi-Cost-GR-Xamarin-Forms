using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
using Android.Views;
using System.Net;
using Newtonsoft.Json;
using TaxiCost.Models.DistanceMatrixApi;
using TaxiCost.Models.Enums;
using System.Globalization;
using XLabs.Platform.Services.Geolocation;
using Android.Locations;
using Android.Content;
using Android.Provider;


namespace TaxiCostXamarin
{
    public partial class MainPage : ContentPage
    {
        IGeolocator geolocator;
        CancellationTokenSource cancelSource;
        private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        string PositionStatus = String.Empty;
        string PositionLatitude = String.Empty;
        string PositionLongitude = String.Empty;

        static Area areaEnum = Area.Athens;
        static TaxiCost.Models.Enums.TimeZone timezoneEnum = TaxiCost.Models.Enums.TimeZone.Day;
        static PickUpPlace pickuplaceEnum = PickUpPlace.None;
        static Luggage luggageEnum = Luggage.None;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void MenuButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage(), true);
        }

        async void AreaButtonTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Επιλέξτε περιοχή", "Άκυρο", null, "Αθήνα ή Θεσαλονίκη", "Υπόλοιπη Ελλάδα", "Νησιά");

            switch (action)
            {
                case "Αθήνα ή Θεσαλονίκη":
                    AreaText.Text = "Αθήνα ή Θεσσαλονίκη";
                    AreaImage.Source = ImageSource.FromFile("group.png");
                    areaEnum = Area.Athens;
                    break;
                case "Υπόλοιπη Ελλάδα":
                    AreaText.Text = "Υπόλοιπη Ελλάδα";
                    AreaImage.Source = ImageSource.FromFile("nature.png");
                    areaEnum = Area.MainLand;
                    break;
                case "Νησιά":
                    AreaText.Text = "Νησιά";
                    AreaImage.Source = ImageSource.FromFile("beach.png");
                    areaEnum = Area.Islands;
                    break;
            }
        }

        async void PeriodButtonTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Επιλέξτε Χρονική περίοδο", "Άκυρο", null, "Ημέρα (05:00 π.μ - 12:00 π.μ)", "Βράδυ (12:00 π.μ - 05:00 π.μ)");

            switch (action)
            {
                case "Ημέρα (05:00 π.μ - 12:00 π.μ)":
                    PeriodText.Text = "Ημέρα (05:00 π.μ - 12:00 π.μ)";
                    PeriodImage.Source = ImageSource.FromFile("sun.png");
                    timezoneEnum = TaxiCost.Models.Enums.TimeZone.Day;
                    break;
                case "Βράδυ (12:00 π.μ - 05:00 π.μ)":
                    PeriodText.Text = "Βράδυ (12:00 π.μ - 05:00 π.μ)";
                    PeriodImage.Source = ImageSource.FromFile("moon.png");
                    timezoneEnum = TaxiCost.Models.Enums.TimeZone.Night;
                    break;
            }
        }

        async void PickUpPlaceButtonTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Τοποθεσία Παραλαβής", "Άκυρο", null, "Όχι", "Αεροδρόμιο", "Κτελ ή Σιδ. Σταθμός");

            switch (action)
            {
                case "Όχι":
                    PickUpPlaceText.Text = "Όχι";
                    PickUpPlaceImage.Source = ImageSource.FromFile("round.png");
                    pickuplaceEnum = PickUpPlace.None;
                    break;

                case "Αεροδρόμιο":
                    PickUpPlaceText.Text = "Αεροδρόμιο";
                    PickUpPlaceImage.Source = ImageSource.FromFile("travel.png");
                    pickuplaceEnum = PickUpPlace.Airport;
                    break;
                case "Κτελ ή Σιδ. Σταθμός":
                    PickUpPlaceText.Text = "Κτελ ή Σιδηροδρομικός σταθμός";
                    PickUpPlaceImage.Source = ImageSource.FromFile("transport.png");
                    pickuplaceEnum = PickUpPlace.BusStationOrRailStation;
                    break;
            }
        }

        async void LuggageButtonTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Επιλέξτε Αποσκευές", "Άκυρο", null, "Χωρίς αποσκευές", "Ελαφρύτερες των 10 κιλών", "Βαρύτερες των 10 κιλών");

            switch (action)
            {
                case "Χωρίς αποσκευές":
                    LuggageText.Text = "Χωρίς αποσκευές";
                    LuggageImage.Source = ImageSource.FromFile("round.png");
                    luggageEnum = Luggage.None;
                    break;

                case "Ελαφρύτερες των 10 κιλών":
                    LuggageText.Text = "Ελαφρύτερες των 10 κιλών";
                    LuggageImage.Source = ImageSource.FromFile("suitcase.png");
                    luggageEnum = Luggage.Light;
                    break;
                case "Βαρύτερες των 10 κιλών":
                    LuggageText.Text = "Βαρύτερες των 10 κιλών";
                    LuggageImage.Source = ImageSource.FromFile("bags.png");
                    luggageEnum = Luggage.Heavy;
                    break;
            }
        }

        async void GPSButtonTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Χρήση της τοποθεσίας μου", "Άκυρο", null, "Όχι", "Ναί");

            switch (action)
            {
                case "Όχι":
                    GPSText.Text = "Όχι";
                    GPSImage.Source = ImageSource.FromFile("round.png");
                    FromAddressInput.IsVisible = true;
                    FromAddressInputText.Text = "Από....";
                    break;

                case "Ναί":
                    if (IsGpsAvailable())
                    {
                        GPSText.Text = "Ναί";
                        GPSImage.Source = ImageSource.FromFile("gps.png");
                        FromAddressInput.IsVisible = false;
                        ShowLoadingPanel();
                        await GetPosition();
                        HideLoadingPanel();
                    }
                    else
                    {
                        DisplayAlert("Taxi Cost GR", "Οι υπηρεσίες θέσης είναι απανεργοποιημένες. Πρέπει να ενεργοποιήσετε τον αισθητήρα Gps της συσκευής σας", "OK");
                    }
                    break;
            }
        }

        void CancelGps_ButtonClicked(object sender, EventArgs e)
        {
            CancelPosition();
        }

        async void EstimateButtonClicked(object sender, EventArgs e)
        {
            if (App.networkStatus == "Connected")
            {
                if (!FromAddressInput.IsVisible)
                {
                    if (PositionLongitude.Length > 0 && PositionLatitude.Length > 0)
                    {
                        FromAddressInputText.Text = PositionLatitude.ToString(CultureInfo.InvariantCulture).Replace(",",".") + "," + PositionLongitude.ToString(CultureInfo.InvariantCulture).Replace(",",".");
                        App.costCalculation.FromAddressText = "Η τοποθεσία μου (GPS)";
                        App.costCalculation.FromAddress = FromAddressInputText.Text;
                    }

                    else
                    {
                        DisplayAlert("Taxi Cost GR", "Η τοποθεσία σας δεν βρέθηκε... παρακαλώ ξαναδοκιμάστε", "OK");
                        return;
                    }
                }

                else
                {
                    TaxiCostXamarin.App.costCalculation.FromAddressText = FromAddressInputText.Text;
                    TaxiCostXamarin.App.costCalculation.ToAddress = ToAddressInputText.Text;
                }

                try
                {
                    WebClient wc = new WebClient();
                    var queryuri = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + FromAddressInputText.Text + "&destinations=" + ToAddressInputText.Text + "&mode=driving&language=el-GR&key=AIzaSyB6d2_0qC97u8wqE-MdeClmMQa5OPzAY6E";
                    var result = await wc.DownloadStringTaskAsync(new Uri(queryuri));
                    var apiQueryResult = JsonConvert.DeserializeObject<ApiDistanceMatrixQueryResults>(result);

                    TaxiCostXamarin.App.costCalculation.PickUpPlace = pickuplaceEnum;
                    TaxiCostXamarin.App.costCalculation.CurrentArea = areaEnum;
                    TaxiCostXamarin.App.costCalculation.TimeZone = timezoneEnum;
                    TaxiCostXamarin.App.costCalculation.Luggage = luggageEnum;
                    TaxiCostXamarin.App.costCalculation.ToAddress = ToAddressInputText.Text;
                    TaxiCostXamarin.App.costCalculation.CurrentAreaText = AreaText.Text;
                    TaxiCostXamarin.App.costCalculation.TimeZoneText = PeriodText.Text;
                    TaxiCostXamarin.App.costCalculation.LuggageText = LuggageText.Text;
                    TaxiCostXamarin.App.costCalculation.PickUpPlaceText = PickUpPlaceText.Text;
                    TaxiCostXamarin.App.costCalculation.DistanceText = apiQueryResult.Rows.FirstOrDefault().Elements.FirstOrDefault().Distance.Text;
                    TaxiCostXamarin.App.costCalculation.DurationText = apiQueryResult.Rows.FirstOrDefault().Elements.FirstOrDefault().Duration.Text;
                    TaxiCostXamarin.App.costCalculation.CalculateTotalCost();
                    //App.costCalculation.DrawRoute();

                    await Navigation.PushAsync(new CostPage(), true);
                }
                catch (Exception exp)
                {
                    DisplayAlert("Taxi Cost GR", "Η διαδρομή απέτυχε να υπολογιστεί . Παρακαλώ ελέγξτε την ορθότητα των στοιχείων και τη συνδεσιμότήτα στο ίντερνετ και ξαναπροσπαθήστε", "OK");
                }
            }

            else
            {
                DisplayAlert("Taxi Cost GR", "Δεν βρέθηκε σύνδεση στο Ίντερνετ. Ελέγξτε την σύνδεση σας και ξαναπροσπαθήστε", "OK");
            }
        }

        void FromAddressInput_Focused(object sender, EventArgs e)
        {
            if (FromAddressInputText.Text == "Από....")
            {
                FromAddressInputText.Text = string.Empty;
            }
        }

        void ToAddressInput_Focused(object sender, EventArgs e)
        {
            if (ToAddressInputText.Text == "Προς....")
            {
                ToAddressInputText.Text = string.Empty;
            }
        }

        void ShowLoadingPanel()
        {
            AreaButton.IsVisible = false;
            PeriodButton.IsVisible = false;
            PickUpPlaceButton.IsVisible = false;
            LuggageButton.IsVisible = false;
            EstimateButton.IsVisible = false;
            GPSButton.IsVisible = false;
            FromAddressInput.IsVisible = false;
            ToAddressInput.IsVisible = false;
            EstimateArrow.IsVisible = false;
            LoadingPanel.IsVisible = true;
        }

        void HideLoadingPanel()
        {
            AreaButton.IsVisible = true;
            PeriodButton.IsVisible = true;
            PickUpPlaceButton.IsVisible = true;
            LuggageButton.IsVisible = true;
            EstimateButton.IsVisible = true;
            GPSButton.IsVisible = true;
            //FromAddressInput.IsVisible = true;
            ToAddressInput.IsVisible = true;
            EstimateArrow.IsVisible = true;
            LoadingPanel.IsVisible = false;
        }

        #region GpsOperations
        void SetupGpsLocator()
        {
            DependencyService.Register<Geolocator>();
            if (this.geolocator != null)
                return;
            this.geolocator = DependencyService.Get<IGeolocator>();
            //    this.geolocator.PositionError += OnListeningError;
            //    this.geolocator.PositionChanged += OnPositionChanged;
        }
        private void CancelPosition()
        {
            if (cancelSource != null)
            {
                cancelSource.Cancel();
            }
        }
        async Task GetPosition()
        {
            SetupGpsLocator();

            this.cancelSource = new CancellationTokenSource();

            IsBusy = true; await this.geolocator.GetPositionAsync(timeout: 10000, cancelToken: this.cancelSource.Token, includeHeading: true)
                .ContinueWith(t =>
                {
                    IsBusy = false;
                    if (t.IsFaulted)
                    {
                        PositionStatus = ((GeolocationException)t.Exception.InnerException).Error.ToString();
                        DisplayAlert("Taxi Cost GR", "Προέκυψε κάποιο σφάλμα . Παρακαλώ βεβαιωθείτε οτι έχετε ενεργοποιήσει το GPS στη συσκευή σας και ξαναπροσπαθήστε", "OK");
                        GPSText.Text = "Όχι";
                        FromAddressInput.IsVisible = true;
                        GPSImage.Source = ImageSource.FromFile("round.png");
                        FromAddressInput.IsVisible = true;
                    }
                    else if (t.IsCanceled)
                    {
                        PositionStatus = "Canceled";
                        GPSText.Text = "Όχι";
                        GPSImage.Source = ImageSource.FromFile("round.png");
                        FromAddressInput.IsVisible = true;
                        FromAddressInputText.Text = "Από....";
                        DisplayAlert("Taxi Cost GR", "Η θέση σας δεν βρέθηκε...", "OK");                     
                    }
                    else
                    {
                        FromAddressInput.IsVisible = false;
                        PositionStatus = t.Result.Timestamp.ToString("G");
                        PositionLatitude = t.Result.Latitude.ToString("N4");
                        PositionLongitude = t.Result.Longitude.ToString("N4");
                        DisplayAlert("Taxi Cost GR", "Η Θέση σας βρέθηκε !", "OK");                       
                    }

                }, scheduler);
        }
        bool IsGpsAvailable()
        {
            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                return false;
                //Intent gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
                //Forms.Context.StartActivity(gpsSettingIntent);
            }

            return true;
        }
        #endregion
    }
}
