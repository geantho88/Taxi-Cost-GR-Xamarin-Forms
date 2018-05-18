using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiCostXamarin.Enviroment
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
    }
}
