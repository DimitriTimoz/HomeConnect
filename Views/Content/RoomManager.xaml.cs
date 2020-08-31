using Android.App;
using Android.Views;
using HomeConnect.Class;
using HomeConnect.CustomElement;
using HomeConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views.Content
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomManager : ContentView
    {
        public event EventHandler ExapnderOff;
        private bool isAddMod = false;
        public bool IsAddMod 
        {
            get
            {
                return isAddMod;
            } 
            set 
            {
                isAddMod = value;
                ModChanged();
            }
        } 
        
        private RoomModel newRoom;
        public RoomManager()
        {
            isAddMod = false;
            InitializeComponent();
        }
        private void ModChanged()
        {
            if (isAddMod)
            {
                lbIndic.Text = "Créé une pièce de ton choix.";
            }
            else
            {
                lbIndic.Text = "Créé ou supprime une pièce de ton choix.";
            }
        }
        private async Task AddAsync(EventArgs e)
        {
            if (isAddMod)
            {
                if (!await App.Current.MainPage.DisplayAlert("Attention", "Etes-vous certain de vouloir créer cette pièce ?", "Oui", "Annuler"))
                {
                    return;
                }
                RoundedEntry editor = roomAdd.FindByName("RoomNameEditor") as RoundedEntry;
                string description = editor.Text;
                CollectionView collection = roomAdd.FindByName("CollectionSelector") as CollectionView;
                RoomUI roomSelected = collection.SelectedItem as RoomUI;
                int type = -1;
                try
                {
                    type = roomSelected.ID;
                }
                catch
                {
                    await App.Current.MainPage.DisplayAlert("Erreur", "Vous devez séléctionner un type de pièce.", "ok");
                    return;
                }

                newRoom = CreatRoomObject(description, type);
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        string request = "http://" + Persistence.settings.HubIP + "/add_room.php?type=" + ((int)newRoom.type).ToString() + "&name=" + newRoom.name + "&description=" + newRoom.description;
                        webClient.DownloadStringCompleted += AddNewRoomCompleted;
    
                        webClient.DownloadStringAsync(new Uri(request));
                        editor.Text = string.Empty;
                    }
                    catch (WebException exp)
                    {
                        await App.Current.MainPage.DisplayAlert("Erreur", "L'ajout de cette pièce a échoué.", "ok");
                        Console.WriteLine(exp.Message);
                        return;
                    }
                }
            }
            else
            {
                ExapnderOff(this, null);
            }   
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            AddAsync(e);
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
        }

        private RoomModel CreatRoomObject(string description, int type)
        {
            string name;
            switch (type)
            {
                case 0:
                    name = "Chambre";
                    break;
                case 1:
                    name = "Salle de vie";
                    break;
                case 2:
                    name = "Salle de bain";
                    break;
                case 3:
                    name = "Toilettes";
                    break;
                case 4:
                    name = "Cuisine";
                    break;
                case 5:
                    name = "Sous sol";
                    break;
                case 6:
                    name = "Entrée";
                    break;
                case 7:
                    name = "Salle à manger";
                    break;
                case 8:
                    name = "Garage";
                    break;
                default:
                    name = "Inconnue";
                    break;
            }
            return new RoomModel() { description = description, name = name,  type= (RoomType.Type)type};
        }

        private void AddNewRoomCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                App.Current.MainPage.DisplayAlert("Erreur", "Cette pièce n'a pas pu être créé.", "ok");
                return;
            }
            if (e.Result.Replace(" ", string.Empty) == "true")
            {
                Persistence.listRoom.Add(newRoom);
                Persistence.Save(Persistence.listRoom, Persistence.KEY_ROOMLIST);
                Page currPage;

                if (Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                {

                    int index = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count - 1;

                    currPage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack[index];
                    if (currPage is RoomList)
                    {
                        RoomList roomList = currPage as RoomList;
                        roomList.UpdateRoomList();
                        ExapnderOff(this, e);
                    }
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
          
       
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            Page currPage;
            if (Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {

                int index = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count - 1;

                currPage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack[index];
                if(currPage is RoomList)
                {
                    RoomList roomList = currPage as RoomList;
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                    roomList.SetExpander(false);
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                }
            }
        }
    }
}