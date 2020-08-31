using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HomeConnect.Class
{
    public static class NumberToColor
    {
        public static string GetColor(int temperature)
        {
            float temp = Convert.ToSingle(temperature);
            if (temp > 15 && temp <= 30)
            {

                double rMin = 0;
                double gMin = 255;
                double bMin = 255;

                double rMax = 255;
                double gMax = 0;
                double bMax = 0;

                var rAverage = rMin + (int)((rMax - rMin) * ((temp-15) / 15));
                var gAverage = gMin + (int)((gMax - gMin) * ((temp-15) / 15));
                var bAverage = bMin + (int)((bMax - bMin) * ((temp-15) / 15));
                return Color.FromRgb((int)rAverage, (int)gAverage, (int)bAverage).ToHex();
            }
            /*else if (temp <= 15 && temp >= 0)
            {
                double rMin = 0;
                double gMin = 255;
                double bMin = 255;

                double rMax = 130;
                double gMax = 130;
                double bMax = 130;

                var rAverage = rMin + (int)((rMax - rMin) * ((temp) / 15));
                var gAverage = gMin + (int)((gMax - gMin) * ((temp) / 15));
                var bAverage = bMin + (int)((bMax - bMin) * ((temp) / 15));


                return Color.FromRgb((int)rAverage, (int)gAverage, (int)bAverage).ToHex();
            }*/
            else if (temp >= -30 && temp <= 15)
            {

                double rMin = 0;
                double gMin = 0;
                double bMin = 255;

                double rMax = 0;
                double gMax = 255;
                double bMax = 255;

                var rAverage = rMin + (int)((rMax - rMin) * ((temp + 30) / 45));
                var gAverage = gMin + (int)((gMax - gMin) * ((temp + 30) / 45));
                var bAverage = bMin + (int)((bMax - bMin) * ((temp + 30) / 45));
                return Color.FromRgb((int)rAverage, (int)gAverage, (int)bAverage).ToHex();
            }
           
            else if (temp < -30)
            {
                return "#0000ff";
            }
            else if (temp > 30)
            {
                return "#ff0000";
            }
            else
            {
                return "ffffff";
            }
        }
    }
}
