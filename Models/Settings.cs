using Android.Net;
using Android.Telecom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace HomeConnect.Models
{
    public class Settings
    {
        public string HubMAC { get; set; }
        public string HubIP { get; set; }
        public int actuaID { get; set; }
        public bool HubRegistered { get; set; }
        public bool ISInLocalNetwork
        {
            get
            {
                var profiles = Connectivity.ConnectionProfiles;
                
                if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
                
            }
        }
    }
}
