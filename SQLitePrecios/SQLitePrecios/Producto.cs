using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace SQLitePrecios
{
    public class Producto
    {
        [PrimaryKey, AutoIncrement]
        public int IDProducto { get; set; }        
        public string Nombre { get; set; }        

        public override string ToString()
        {
            return string.Format("{0} {1}", IDProducto, Nombre);
        }
    }
}
