using Android.Views;
using HomeConnect.Class;
using HomeConnect.CustomElement;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomEditor : ContentView
    {
        private int id;
  
        public RoomEditor()
        {
            InitializeComponent();
      
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.IsVisible = false;
            RoomManager roomManager = this.Parent.Parent.Parent as RoomManager;
            roomManager.IsAddMod = true;
            roomManager.roomAdd.IsVisible = true;
        }

        private void RemoveButton_Clicked(object sender, EventArgs e)
        {
            Delete(sender);
        }

        private async Task Delete(object sender)
        {
            if(await Application.Current.MainPage.DisplayAlert("Attention", "Etes-vous certain de vouloir supprimer cette pièce ?", "Oui", "Annuler"))
            {
                if (sender is RemoveButton)
                {
                    RemoveButton removeButton = sender as RemoveButton;
                    id = removeButton.MyID;
                
                    using(WebClient webClient = new WebClient())
                    {
                        webClient.DownloadStringCompleted += IsDelete;
                        string request = "http://" + Persistence.settings.HubIP + "/delete_room.php?id=" + id.ToString();
                        webClient.DownloadStringAsync(new Uri(request));
                    }

                   
                }
            }

        }

        private void IsDelete(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            if(e.Result == "true")
            {
                Persistence.listRoom.RemoveAll(x => x.id == id);
                Persistence.listDevices.FindAll(x => x.room == id).ForEach(X => X.room = 0);
                Persistence.Save(Persistence.listRoom, Persistence.KEY_ROOMLIST);
                Persistence.Save(Persistence.listDevices, Persistence.KEY_DEVICESLIST);
                Page currPage;

                if (Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                {
                    int index = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count - 1;

                    currPage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack[index];
                    if (currPage is RoomList)
                    {
                        RoomList roomList = currPage as RoomList;
                        roomList.UpdateRoomList();
                    }
                }
            }
            
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}