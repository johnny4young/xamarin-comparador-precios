﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SQLitePrecios
{
    public class ProductoCell : ViewCell
    {
        private ProductoPage _pagina;

        public ProductoCell(ProductoPage pagina)
        {
            _pagina = pagina;

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



            var editProductoButton = new Button
            {
                Text = "Editar",
                Scale = 0.90
            };

            editProductoButton.SetBinding(Button.CommandParameterProperty, new Binding("IDProducto"));
            editProductoButton.Clicked += EditProductoButton_Clicked; 


            var deleteProductoButton = new Button
            {
                Text = "Borrar",
                Scale = 0.90
            };


            deleteProductoButton.SetBinding(Button.CommandParameterProperty, new Binding("IDProducto"));
            deleteProductoButton.Clicked += DeleteProductoButton_Clicked; 


            var line1 = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    empresaProductoLabel, nombreProductoLabel , precioProductoLabel, editProductoButton, deleteProductoButton
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

        private void DeleteProductoButton_Clicked(object sender, EventArgs e)
        {
            _pagina.DeleteProductoButton_Clicked(sender, e);
        }

        private void EditProductoButton_Clicked(object sender, EventArgs e)
        {
            _pagina.EditProductoButton_Clicked(sender, e);
        }
    }
}

