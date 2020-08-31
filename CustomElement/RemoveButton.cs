using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeConnect.CustomElement
{
    public class RemoveButton : ImageButton
    {
        public static readonly BindableProperty MyIDProperty =
            BindableProperty.Create(
                nameof(MyID),
                typeof(int),
                typeof(RemoveButton),
                0);

        public int MyID
        {
            get { return (int)GetValue(MyIDProperty); }
            set { SetValue(MyIDProperty, value); }
        }

    
    }
}
