using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SQLitePrecios
{
    public partial class ProductoPage : ContentPage
    {
        public Almacen AlmacenSeleccionado;

        public ProductoPage(Almacen almacenSeleccionado)
        {
            InitializeComponent();

            AlmacenSeleccionado = almacenSeleccionado;

            listaListView.ItemTemplate = new DataTemplate(typeof(ProductoCell));
            

            listaListView.RowHeight = 70;

            //creamos el evento click a los item del listview     
            listaListView.ItemTapped += ListaListViewOnItemTapped;

            using (var datos = new DataAccess())
            {
                listaListView.ItemsSource = datos.GetProductos(AlmacenSeleccionado.IDAlmacen);
            }

            agregarButton.Clicked += AgregarButton_Clicked;
        }

        


        /// <summary>
        /// al seleccionar uno de los producto, procedemos a mostrar la pantalla de verificar comparación
        /// pasándole como parámetro el ID del producto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListaListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //evitar el disparar este evento en la deselección
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            bool resultado =  await DisplayAlert("Pregunta", "Desea comparar precios?", "SI", "NO");

            if (resultado == true)
            {
                //mostramos la comparación
                //llamamos a la otra vista de resultados
                ComparacionPage page = new ComparacionPage((DtoProducto) e.Item);

                await Navigation.PushModalAsync(page);
                
            }

            
        }

        private async void AgregarButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(productoEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un producto", "Aceptar");
                productoEntry.Focus();
                return;
            }

            if (string.IsNullOrEmpty(precioEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un precio", "Aceptar");
                precioEntry.Focus();
                return;
            }

            decimal precioDecimal = Decimal.Zero;
            if ( !Decimal.TryParse(precioEntry.Text, out precioDecimal) )
            {
                await DisplayAlert("Error", "Debe ingresar valor numérico en  precio", "Aceptar");
                precioEntry.Focus();
                return;
            }


            var producto = new Producto
            {
                Nombre = productoEntry.Text,
            };

            using (var datos = new DataAccess())
            {
                int consecutivo = datos.InsertProducto(producto);

                var precioCompetidor = new PrecioCompetidor
                {
                    IDAlmacen = AlmacenSeleccionado.IDAlmacen,
                    IDProducto = consecutivo,
                    Precio = precioDecimal
                };

                datos.InsertPrecioCompetidor(precioCompetidor);

                listaListView.ItemsSource = datos.GetProductos(AlmacenSeleccionado.IDAlmacen);
            }

            productoEntry.Text = string.Empty;
            precioEntry.Text = string.Empty;           
            //await DisplayAlert("Confirmación", "Producto agregado", "Aceptar");
        }
    }
}
