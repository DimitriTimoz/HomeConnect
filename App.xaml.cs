using HomeConnect.Class;
using HomeConnect.Models;
using HomeConnect.Views;
using System.Collections.Generic;
using System.Net;
using Xamarin.Forms;

namespace HomeConnect
{
    public partial class App : Application
    {
        public App()
        {
          
            Persistence.LoadAll();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk0MjQ0QDMxMzgyZTMyMmUzMG5LMnpKcHdWSzVjWXhzZm9GSVp6cmgvQUR3Q2dBWkcybFVmaUp0Q0pSelE9");
           
            InitializeComponent();
     
            if (Persistence.settings.HubRegistered)
            {
               
                MainPage = new NavigationPage(new RoomList())
                {
                    BarBackgroundColor = Color.FromHex("#1D7A98")
                };

               
            }
            else
            {
                MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#1D7A98")
                };
            }
           

        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            Persistence.SaveAll();
        }

        protected override void OnResume()
        {
            Persistence.LoadAll();
        }


    }
}
