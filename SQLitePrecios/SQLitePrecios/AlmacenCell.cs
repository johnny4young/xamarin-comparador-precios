using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SQLitePrecios
{
    public class AlmacenCell : ViewCell
    {
        public AlmacenCell()
        {
           
            var nombreTiendaLabel = new Label
            {
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            nombreTiendaLabel.SetBinding(Label.TextProperty, new Binding("Nombre"));


            var line1 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                     nombreTiendaLabel
                },
            };

                View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    line1,
                },
            };
        }
    }
}

