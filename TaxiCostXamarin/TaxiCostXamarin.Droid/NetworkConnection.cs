using System;
using Android.Net;
using Android.App;
using Android.Content;
using Xamarin.Forms;
using TaxiCostXamarin.Enviroment;
using TaxiCostXamarin.Droid;

[assembly: Dependency(typeof(NetworkConnection))]
namespace TaxiCostXamarin.Droid
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }
        public void CheckNetworkConnection()
        {
            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}

