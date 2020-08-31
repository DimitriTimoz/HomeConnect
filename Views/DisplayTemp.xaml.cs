using HomeConnect.Class;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeConnect.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayTemp : ContentPage
    {
        
        public DisplayTemp(int id)
        {
            Persistence.LoadListTempModels();
            Persistence.settings.actuaID = id;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            graph.Scroll.ScrollToAsync(graph.Scroll.Width+150, 0, true);
            base.OnAppearing();
        }
    }
}