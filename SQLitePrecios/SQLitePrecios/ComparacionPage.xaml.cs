using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SQLitePrecios
{
    public partial class ComparacionPage : ContentPage
    {
        public DtoProducto ProductoSeleccionado;

        public ComparacionPage(DtoProducto productoSeleccionado)
        {
            InitializeComponent();

            ProductoSeleccionado = productoSeleccionado;

            //cargamos la comparación            
            listaListView.ItemTemplate = new DataTemplate(typeof(PreciosCell));


            listaListView.RowHeight = 70;            

            using (var datos = new DataAccess())
            {
                listaListView.ItemsSource = datos.GetComparacion(ProductoSeleccionado.Nombre);
            }
        }
    }
}
