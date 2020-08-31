using HomeConnect.Class;
using HomeConnect.CustomElement;
using HomeConnect.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomDevices : ContentPage
    {
        private int roomId = 0;
        private int dID = 0;
        private int toID = 0;
        ObservableCollection<DeviceUI> deviceUIsCollToRoom = new ObservableCollection<DeviceUI>();
        public ObservableCollection<DeviceUI> DeviceUIsCollToRoom { get { return deviceUIsCollToRoom; } }
        ObservableCollection<DeviceUI> deviceUIsColl = new ObservableCollection<DeviceUI>();
        public ObservableCollection<DeviceUI> DeviceUIsColl { get { return deviceUIsColl; } }
        public RoomDevices(int roomId)
        {
            this.roomId = roomId; 
            InitializeComponent();
            Expander.TranslationY = 750 - (header.HeightRequest + Expander.CornerRadius);
            Persistence.LoadAll();
            UpdateList();
        }

        public void UpdateList()
        {
            List<DeviceUI> devicesUIs = new List<DeviceUI>(GetRoomUIs(Persistence.listDevices.FindAll(x => x.room == roomId || x.room == 0)));
            deviceUIsCollToRoom.Clear();
            deviceUIsColl.Clear();
            foreach (DeviceUI device in devicesUIs)
            {
                deviceUIsCollToRoom.Add(device);
                if(device.RoomID == roomId)
                {
                    deviceUIsColl.Add(device);
                }
            }
            DevicesListView.ItemsSource = deviceUIsColl;
            ListEditDevices.ItemsSource = deviceUIsCollToRoom;
        }

        private List<DeviceUI> GetRoomUIs(List<DeviceModel> deviceModels)
        {
            List<DeviceUI> roomUIs = new List<DeviceUI>();
            foreach (DeviceModel device in deviceModels)
            {
                string source = "";
                bool switchable = false;
                string description = "Description";
                GetTemps get = Persistence.GetTemps.FindLast(x => x.toID == device.id);
                int temp = 0;
                if(get != null)
                {
                    temp = get.current;
                }
                switch (device.Type)
                {
                    case DeviceType.Type.RELAY:
                        source = "plug.png";
                        switchable = true;
                        break;
                    case DeviceType.Type.THERMOMETER:
                        source = "temperature.png";
                        description = temp.ToString() + " °C";
                        break;
                    default:
                        source = "home.png";
                        break;
                }
                roomUIs.Add(new DeviceUI
                {
                    ID = device.id,
                    Name = (device.Type == 0)? "Thermomètre - " + device.id :"Relai",
                    Source = source,
                    Description = description,
                    RoomID = device.room,
                    Sate = false,
                    Temperature = temp,
                    Type = device.Type,
                    Switchable = switchable,
                    OperationLogo = (device.room != 0)? "less.png": "Add.png"
                });
            }
            roomUIs.Sort(delegate (DeviceUI x, DeviceUI y)
            {
                return x.ID.CompareTo(y.ID);
            });
            return roomUIs;
        }

        private void DevicesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DeviceUI devicesUI = (DeviceUI)e.Item;
            if (devicesUI.Type == DeviceType.Type.THERMOMETER)
            {
                Navigation.PushAsync(new DisplayTemp(devicesUI.ID));
                
            }
            
        }

        public async Task SetExpander(bool value)
        {

            header.IsVisible = !value;
            Content.IsVisible = value;
            if (value)
            {
                await Expander.TranslateTo(0, 0, 500);
            }
            else
            {
                await Expander.TranslateTo(0, Content.Height - (header.HeightRequest + Expander.CornerRadius), 500);
            }
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
             SetExpander(header.IsVisible);
        }

        private void RemoveButton_Clicked(object sender, EventArgs e)
        {
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SetExpander(header.IsVisible);
        }

        private void EditStatButton_Clicked(object sender, EventArgs e)
        {
            Edit(sender);
        }

        

        private async Task DeviceEdit(int id, int toId)
        {
            toID = toId;
            dID = id;
            using(WebClient webClient = new WebClient())
            {
                string request = "http://" + Persistence.settings.HubIP + "/set_device_room.php?id=" + id.ToString() + "&roomID=" + toId.ToString();
                webClient.DownloadStringCompleted += DeviceRoomEdited;
                webClient.DownloadStringAsync(new Uri(request));
            }
        }

        private void DeviceRoomEdited(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                DisplayAlert("Erreur", "Echec cet appareil n'a pas été modifié", "ok");
                return;
            }
            Persistence.listDevices.Find(x => x.id == dID).room = toID;
            Persistence.Save(Persistence.listDevices, Persistence.KEY_DEVICESLIST);
            UpdateList();
            SetExpander(false);
        }

        private async Task Edit(object sender)
        {
            if(sender is RemoveButton)
            {
                var removeButton = sender as RemoveButton;
                string source = removeButton.Source.ToString().Split(' ')[1];
                if (source == "Add.png")
                {
                    if(await DisplayAlert("Attention", "Etes-vous certain de vouloir ajouter cet appareil à cette pièce ?", "Oui", "Non"))
                    {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                        DeviceEdit(removeButton.MyID, roomId);
                    }
                }
                else
                {
                    if (await DisplayAlert("Attention", "Etes-vous certain de vouloir retirer cet appareil à cette pièce ?", "Oui", "Non"))
                    {
                        DeviceEdit(removeButton.MyID, 0);
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel

                    }
                }

            }
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            SetExpander(false);
        }
    }

    public class DeviceUI
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public bool Sate { get; set; }
        public bool Switchable { get; set; }
        public string OperationLogo { get; set; }
        public double Temperature { get; set; }
        public DeviceType.Type Type { get; set; }
    }
}