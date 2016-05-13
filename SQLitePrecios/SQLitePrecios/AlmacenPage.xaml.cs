using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SQLitePrecios
{
    public partial class AlmacenPage : ContentPage
    {
        public AlmacenPage()
        {
            InitializeComponent();

            Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10));

            listaListView.ItemTemplate = new DataTemplate(typeof(AlmacenCell));
            
            //creamos el evento click a los item del listview
            listaListView.ItemTapped += ListaListViewOnItemTapped;
             
            listaListView.RowHeight = 70;

            using (var datos = new DataAccess())
            {
                //siempre comentar esto!!!!
                //datos.resetProductos();


                listaListView.ItemsSource = datos.GetAlmacen();
            }

            agregarButton.Clicked += AgregarButton_Clicked;
        }

        /// <summary>
        /// al seleccionar uno de los almacenes, procedemos a mostrar la pantalla de productos,
        /// pasándole como parámetro el ID del almacén.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListaListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //evitar el disparar este evento en la deselección
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }


            ProductoPage productoPage = new ProductoPage((Almacen)e.Item);            
            
            Navigation.PushModalAsync(productoPage);

            //DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
        }       


        private async void AgregarButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(almacenEntry.Text))
            {
                await DisplayAlert("Error", "Debe ingresar una  tienda", "Aceptar");
                almacenEntry.Focus();
                return;
            }


            var almacen = new Almacen
            {
                Nombre = almacenEntry.Text,
            };

            using (var datos = new DataAccess())
            {
                datos.InsertAlmacen(almacen);
                listaListView.ItemsSource = datos.GetAlmacen();
            }

            almacenEntry.Text = string.Empty;
            //await DisplayAlert("Confirmación", "Almacen agregado", "Aceptar");
        }
    }
}
