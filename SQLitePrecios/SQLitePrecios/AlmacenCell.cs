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
        private AlmacenPage _pagina;

        public AlmacenCell(AlmacenPage pagina)
        {

            _pagina = pagina;

            var nombreTiendaLabel = new Label
            {
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            nombreTiendaLabel.SetBinding(Label.TextProperty, new Binding("Nombre"));



            var editTiendaButton = new Button
            {
                Text = "Editar",
                Scale = 0.90                
            };
            
            editTiendaButton.SetBinding(Button.CommandParameterProperty, new Binding("IDAlmacen"));
            editTiendaButton.Clicked += EditTiendaButton_Clicked; 


            var deleteTiendaButton = new Button
            {
                Text = "Borrar",
                Scale = 0.90
            };

            
            deleteTiendaButton.SetBinding(Button.CommandParameterProperty, new Binding("IDAlmacen"));
            deleteTiendaButton.Clicked += DeleteTiendaButton_Clicked; 

            var line1 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                     nombreTiendaLabel,
                     editTiendaButton,
                     deleteTiendaButton
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

        private void DeleteTiendaButton_Clicked(object sender, EventArgs e)
        {
            _pagina.DeleteTiendaButton_Clicked(sender, e);
        }

        private void EditTiendaButton_Clicked(object sender, EventArgs e)
        {
            _pagina.EditTiendaButton_Clicked(sender, e);
        }
    }
}

