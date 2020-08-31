using Android.App;
using HomeConnect.Class;
using HomeConnect.Models;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomList : ContentPage
    {
        public List<RoomUI> roomUIs = new List<RoomUI>();
        ObservableCollection<RoomUI> roomUIsColl = new ObservableCollection<RoomUI>();
        public ObservableCollection<RoomUI> RoomUIsColl { get { return roomUIsColl; } }
        public RoomList()
        {
            InitializeComponent();
            Persistence.LoadListRoomModels();
            if (!Persistence.settings.ISInLocalNetwork)
            {
                Expander.IsVisible = false;
            }
            else
            {
                RoomManager.ExapnderOff += SetExpanderOff;
                Expander.TranslationY = 750 - (header.HeightRequest + Expander.CornerRadius);
            }

        }
        protected override void OnAppearing()
        {
            UpdateRoomList();
        }
      
        protected override void OnDisappearing()
        {
            Persistence.LoadListRoomModels();
        }

        public void UpdateRoomList()
        {
            Persistence.LoadListRoomModels();
            List<RoomUI> roomUIs = GetRoomUIs(Persistence.listRoom);
            roomUIsColl.Clear();
            foreach (RoomUI ui in roomUIs)
            {
                roomUIsColl.Add(ui);
            }
            RoomsListView.ItemsSource = roomUIsColl;
            RoomManager.roomEditor.RoomListViewEdit.ItemsSource = roomUIsColl;

        }

        public async Task SetExpander(bool value)
        {
            
            header.IsVisible = !value;
            RoomManager.IsVisible = value;
            ValidateButton.IsVisible = !value;
            RoomManager.IsAddMod = false;
            if (value)
            {
                await Expander.TranslateTo(0, 0, 500);
            }
            else
            {
                await Expander.TranslateTo(0, Content.Height - (header.HeightRequest + Expander.CornerRadius), 500);
            }
            RoomManager.roomEditor.IsVisible = true;
            RoomManager.roomAdd.IsVisible = false;
        }

        private void SetExpanderOff(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            SetExpander(false);
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
        }

        private List<RoomUI> GetRoomUIs(List<RoomModel> roomModels)
        {
            List<RoomUI> UIs = new List<RoomUI>();
            foreach (RoomModel room in roomModels)
            {
                string source = room.type.ToString() + ".png";
                UIs.Add(new RoomUI
                {
                    ID = room.id,
                    Name = room.name,
                    Source = source,
                    Description = room.description
                });
            }
            UIs.Sort(delegate (RoomUI x, RoomUI y)
            {
                return x.ID.CompareTo(y.ID);
            });
            return UIs;
        }

        private void RoomsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RoomUI room = (RoomUI)e.Item;
            Navigation.PushAsync(new RoomDevices(room.ID));
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
            SetExpander(true);
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel

        }
    }
    public class RoomUI
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }
}