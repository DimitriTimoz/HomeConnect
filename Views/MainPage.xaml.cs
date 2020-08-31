using HomeConnect.Class;
using HomeConnect.Models;
using HomeConnect.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeConnect
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private bool animButton = true;

       
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            FadeAnim();
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
        }

        private async Task FadeAnim()
        {
            while (animButton)
            {
                animButton = true;
                await SearchHome.ScaleTo(1.1, 1200);
                await SearchHome.ScaleTo(1, 1200);
            }

        }

        private async void SearchHome_Clicked(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            await SwitchButtonState(false);
     
            ScanLocalNetwork.Ping_all();
            while (!ScanLocalNetwork.IsOk) ;
            Navigation.PushAsync(new ListHubs(ScanLocalNetwork.hubs));
        }

      
      
        private async Task SwitchButtonState(bool isOn = false)
        {
            if (isOn)
            {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                SearchIndicator.ScaleTo(0, 1500);
                SearchIndicator.FadeTo(0, 1500);
                await Task.Delay(500);
                SearchHome.IsVisible = true;
                SearchHome.ScaleTo(1, 1500);
                await SearchHome.FadeTo(1, 1500);
                SearchIndicator.IsVisible = false;
                FadeAnim();
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel

            }
            else
            {
                SearchIndicator.Scale = 0;
                animButton = false;
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                SearchHome.ScaleTo(0, 600);
                SearchHome.FadeTo(0, 600);
                await Task.Delay(400);
                SearchIndicator.IsVisible = true;
                SearchIndicator.IsRunning = true;
                SearchIndicator.IsEnabled= true;
                await SearchIndicator.ScaleTo(1, 600);
                
                SearchHome.IsVisible = false;
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            }

        }
    }
}
