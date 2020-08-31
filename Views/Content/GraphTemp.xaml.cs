using HomeConnect.Class;
using HomeConnect.Models;
using Microcharts;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphTemp : ContentView
    {
        List<ChartEntry> entries = new List<ChartEntry>();
        public GraphTemp()
        {
            InitializeComponent();
            Persistence.LoadListTempModels();
            if (Persistence.settings.actuaID == 0)
            {
                Navigation.PopAsync();
            }
            List<GetTemps> toDisplay = Persistence.GetTemps.Where(temperature => temperature.toID == Persistence.settings.actuaID ).ToList();
            if ( toDisplay.Count < 1)
            {
                Navigation.PopAsync();
            }
            

            foreach (GetTemps temp in toDisplay) 
            {
                string colorHex = NumberToColor.GetColor(temp.average);
                string label = temp.average.ToString() + " °C";
                entries.Add(
                    new ChartEntry(temp.average) {
                        Color = SKColor.Parse(colorHex),
                        TextColor = SKColor.Parse(colorHex),
                        ValueLabel = label,
                        Label =  DateTime.Parse(temp.date).ToShortDateString()


                    });
            }
            Chart2.WidthRequest = Persistence.GetTemps.Count * 35;
            Chart2.Chart = new LineChart() { Entries = entries, IsAnimated = true, LabelTextSize=24, LabelOrientation=Orientation.Vertical, LineMode=LineMode.Spline, ValueLabelOrientation = Orientation.Horizontal, LineSize = 6, EnableYFadeOutGradient = true, AnimationDuration = TimeSpan.FromMilliseconds(3000), };
            
            
        }
        

    }
}