using System.Windows.Input;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace SQLitePrecios
{
    public class Almacen 
    {
        [PrimaryKey, AutoIncrement]
        public int IDAlmacen { get; set; }
        [Unique]
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", IDAlmacen, Nombre, Activo);
        }

    }
}
       