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
        public List<DtoProducto> Productos;
        public DtoProducto ItemEditar;
        public bool modoNuevo = true;

        public ProductoPage(Almacen almacenSeleccionado)
        {
            InitializeComponent();

            AlmacenSeleccionado = almacenSeleccionado;

            listaListView.ItemTemplate = new DataTemplate(() => new ProductoCell(this) );
            

            listaListView.RowHeight = 70;

            //creamos el evento click a los item del listview     
            listaListView.ItemTapped += ListaListViewOnItemTapped;

            using (var datos = new DataAccess())
            {
                Productos = new List<DtoProducto>();
                Productos = datos.GetProductos(AlmacenSeleccionado.IDAlmacen);
                listaListView.ItemsSource = Productos;
            }

            agregarButton.Clicked += AgregarButton_Clicked;
            limpiarButton.Clicked += LimpiarButton_Clicked; ;
        }


        /// <summary>
        /// limpiar la data actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LimpiarButton_Clicked(object sender, EventArgs e)
        {
            productoEntry.Text = "";
            precioEntry.Text = "";
            modoNuevo = true;
            productoEntry.Focus();
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


            //insertamos
            using (var datos = new DataAccess())
            {
                if (modoNuevo)
                {
                    int consecutivo = datos.InsertProducto(producto);

                    var precioCompetidor = new PrecioCompetidor
                    {
                        IDAlmacen = AlmacenSeleccionado.IDAlmacen,
                        IDProducto = consecutivo,
                        Precio = precioDecimal
                    };

                    datos.InsertPrecioCompetidor(precioCompetidor);
                }
                else
                {
                    //obtenemos el producto y lo actualizamos
                    ItemEditar.Nombre = productoEntry.Text;
                    ItemEditar.Precio = precioDecimal;
                    //actualizamos
                    datos.UpdateProducto(ItemEditar);
                }
                

                Productos = new List<DtoProducto>();
                Productos = datos.GetProductos(AlmacenSeleccionado.IDAlmacen);
                listaListView.ItemsSource = Productos;

                productoEntry.Text = string.Empty;
                precioEntry.Text = string.Empty;
            }            
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void DeleteProductoButton_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var parameter = Convert.ToInt32(item.CommandParameter);

            bool resultado = await DisplayAlert("Pregunta", "Desea eliminar producto?", "SI", "NO");

            if (resultado == true)
            {
                ItemEditar = (from itm in Productos
                              where itm.IDProducto == parameter
                              select itm).FirstOrDefault<DtoProducto>();

                using (var datos = new DataAccess())
                {
                    datos.DeleteProducto(ItemEditar);
                    Productos = new List<DtoProducto>();
                    Productos = datos.GetProductos(AlmacenSeleccionado.IDAlmacen);
                    listaListView.ItemsSource = Productos;
                }


            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditProductoButton_Clicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var parameter = Convert.ToInt32(item.CommandParameter);

            ItemEditar = (from itm in Productos
                          where itm.IDProducto == parameter
                          select itm).FirstOrDefault<DtoProducto>();

            productoEntry.Text = ItemEditar.Nombre;
            precioEntry.Text =  ItemEditar.Precio.ToString();
            productoEntry.Focus();
            modoNuevo = false;

        }


    }
}
