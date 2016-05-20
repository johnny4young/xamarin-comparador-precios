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

        public List<Almacen> Almacenes;
        public Almacen ItemEditar;
        public bool modoNuevo = true;

        public AlmacenPage()
        {
            InitializeComponent();

            Padding = Device.OnPlatform(
               new Thickness(10, 20, 10, 10),
               new Thickness(10),
               new Thickness(10));

            listaListView.ItemTemplate = new DataTemplate(  () => new AlmacenCell(this)  );
                                                
            //creamos el evento click a los item del listview
            listaListView.ItemTapped += ListaListViewOnItemTapped;
             
            listaListView.RowHeight = 70;

            ItemEditar = new Almacen();

            using (var datos = new DataAccess())
            {
                //siempre comentar esto!!!!
                //datos.resetProductos();

                Almacenes = new List<Almacen>();
                Almacenes = datos.GetAlmacen();
                listaListView.ItemsSource = Almacenes;
            }

            agregarButton.Clicked += AgregarButton_Clicked;
            limpiarButton.Clicked += LimpiarButton_Clicked;
        }

        private void LimpiarButton_Clicked(object sender, EventArgs e)
        {
            almacenEntry.Text = "";
            modoNuevo = true;
            almacenEntry.Focus();
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


            //insertamos
            using (var datos = new DataAccess())
            {
                if (modoNuevo)
                {
                    datos.InsertAlmacen(almacen);
                }
                else
                {
                    ItemEditar.Nombre = almacenEntry.Text;
                    //actualizamos
                    datos.UpdateAlmacen(ItemEditar);
                }
                
                Almacenes = new List<Almacen>();
                Almacenes = datos.GetAlmacen();
                listaListView.ItemsSource = Almacenes;
                almacenEntry.Text = string.Empty;
            }
                                                
        }

        /// <summary>
        /// cargamos en la interfaz para actualizar
        /// https://blog.xamarin.com/simplifying-events-with-commanding/
        /// http://www.c-sharpcorner.com/blogs/handling-child-control-event-in-listview-using-xamarinforms1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditTiendaButton_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var parameter = Convert.ToInt32(item.CommandParameter);

            ItemEditar = (from itm in Almacenes
                             where itm.IDAlmacen == parameter
                             select itm).FirstOrDefault<Almacen>();

            almacenEntry.Text = ItemEditar.Nombre;
            almacenEntry.Focus();
            modoNuevo = false;


        }


        /// <summary>
        /// preguntamos confirmación de eliminar, en caso positivo, borramos item y refrescamos la data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void DeleteTiendaButton_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var parameter = Convert.ToInt32(item.CommandParameter);

            bool resultado = await DisplayAlert("Pregunta", "Desea elimiar almacén?", "SI", "NO");

            if (resultado == true)
            {
                ItemEditar = (from itm in Almacenes
                              where itm.IDAlmacen == parameter
                              select itm).FirstOrDefault<Almacen>();

                using (var datos = new  DataAccess())
                {
                    datos.DeleteAlmacen(ItemEditar);
                    Almacenes = new List<Almacen>();
                    Almacenes = datos.GetAlmacen();
                    listaListView.ItemsSource = Almacenes;
                }
                

            }
        }
    }
}
