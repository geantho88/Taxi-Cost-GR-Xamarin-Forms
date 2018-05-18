using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaxiCost.ViewModel;
using TaxiCostXamarin.Enviroment;
using Xamarin.Forms;

namespace TaxiCostXamarin
{
    public class App : Application
    {
        public static CostCalculation costCalculation = new CostCalculation();
        public static string networkStatus;
        public App()
        {
            // The root page of your application
            //MainPage = new ContentPage {
            //    Content = new StackLayout {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //            new Label {
            //                XAlign = TextAlignment.Center,
            //                Text = "Welcome to Xamarin Forms!"
            //            }
            //        }
            //    }
            //};

            var mainPage = new NavigationPage(new MainPage());
            MainPage = mainPage;
        }


        protected override void OnStart()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
