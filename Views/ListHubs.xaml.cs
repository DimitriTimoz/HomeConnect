using HomeConnect.Class;
using HomeConnect.Models;
using HomeConnect.Views.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListHubs : ContentPage
    {
        private List<HubModel> Hubs;
     
        public ListHubs(List<HubModel> hubs)
        {
            InitializeComponent();
            Hubs = hubs;
            ListDevices.ItemsSource = hubs;

        }

        private async void ListDevices_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (await DisplayAlert("Attention", "Etes-vous certain de vouloir congigurer: " + Hubs[e.ItemIndex].Name + " ?", "Oui", "Annuler"))
            {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                hubIpSave(Hubs[e.ItemIndex].Ip);
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            }
            else
            {
                return;
            }

        }
        private bool ipTest(string ip)
        {
            using(var webClient = new WebClient())
            {
                try
                {
                    string wordkey = webClient.DownloadString("http://" + ip + "/me.txt");
                    if(wordkey == "56ds54dz52d1d2sqs")
                    {
                        return true;
                    }
                }
                catch (WebException exp)
                {
                    Console.Error.WriteLine(exp.Message);
                }
            }
            return false;
        }
        private void hubIpSave(string ip)
        {
            if (ipTest(ip))
            {
                Persistence.settings.HubIP = ip;
                Persistence.settings.HubRegistered = true;
                Persistence.Save(Persistence.settings, Persistence.KEY_SETTINGS);
                Navigation.PushAsync(new RoomList());
                
            }
            else
            {
                Navigation.PopAsync();
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Ip du hub", "Quelle est l'ip locale du hub ?");
            hubIpSave(result);
        }
    }
}