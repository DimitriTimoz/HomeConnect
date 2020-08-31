using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomAdd : ContentView
    {
        public RoomAdd()
        {
            InitializeComponent();
            List<RoomUI> roomUIs = new List<RoomUI>();
            roomUIs.Add(new RoomUI() { Source = "ROOM.png" , ID = 0});
            roomUIs.Add(new RoomUI() { Source = "LIVINGROOM.png", ID = 1 });
            roomUIs.Add(new RoomUI() { Source = "BATHROOM.png", ID = 2 });
            roomUIs.Add(new RoomUI() { Source = "TOILET.png", ID = 3 });
            roomUIs.Add(new RoomUI() { Source = "KITCHEN.png", ID = 4 });
            roomUIs.Add(new RoomUI() { Source = "BASEMENT.png", ID = 5 });
            roomUIs.Add(new RoomUI() { Source = "ENTRANCE.png", ID = 6 });
            roomUIs.Add(new RoomUI() { Source = "DINNINGROOM.png", ID = 7 });
            roomUIs.Add(new RoomUI() { Source = "GARAGE.png", ID = 8 });
            CollectionSelector.ItemsSource = roomUIs;        
        }
    }
}