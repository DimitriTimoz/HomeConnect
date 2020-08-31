using HomeConnect.Class;
using HomeConnect.Models;
using Microcharts;
using SkiaSharp;
using Syncfusion.SfGauge.XForms;
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
    public partial class ActualTemp : ContentView
    {


        public ActualTemp()
        {
            InitializeComponent();
            if(Persistence.settings.actuaID == 0)
            {
                Pop();
            }
            List<GetTemps> toDisplay = Persistence.GetTemps.Where(temperature => temperature.toID == Persistence.settings.actuaID).ToList();
            if (toDisplay.Count > 0)
            {
                int index = toDisplay.Count - 1;

                SetGauge(GaugeTemp, toDisplay[index].current);
                SetGauge(GaugeAverage, toDisplay[index].average);
                SetGauge(GaugeMin, toDisplay[index].min);
                SetGauge(GaugeMax, toDisplay[index].max);
            }
            else
            {
                Pop();

            }

        }
        private void Pop()
        {
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
   
        private void SetGauge(SfCircularGauge GaugeTemp, int value)
        {
            string hexMax = NumberToColor.GetColor(value);
            GaugeTemp.Headers[0].Text = value.ToString() + " °C";
            GaugeTemp.Scales[0].Pointers[0].Color = Color.FromHex(hexMax);
            GaugeTemp.Scales[0].Pointers[0].Value = value;
        }

    }
}