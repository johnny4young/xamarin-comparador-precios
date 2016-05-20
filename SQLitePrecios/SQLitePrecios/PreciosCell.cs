using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SQLitePrecios
{
    public class PreciosCell : ViewCell
    {        

        public PreciosCell()
        {            

            var empresaProductoLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
            };

            empresaProductoLabel.SetBinding(Label.TextProperty, new Binding("Empresa"));


            var nombreProductoLabel = new Label
            {
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            nombreProductoLabel.SetBinding(Label.TextProperty, new Binding("Nombre"));


            var precioProductoLabel = new Label
            {
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            precioProductoLabel.SetBinding(Label.TextProperty, new Binding("Precio"));
           


            var line1 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    empresaProductoLabel, nombreProductoLabel , precioProductoLabel
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
