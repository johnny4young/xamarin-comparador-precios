using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace SQLitePrecios
{    
    public class PrecioCompetidor
    {
        [PrimaryKey, AutoIncrement]
        public int IDPrecioCompetidor { get; set; }
        public int IDAlmacen { get; set; }
        public int IDProducto { get; set; }        
        public Decimal Precio { get; set; }        

        public override string ToString()
        {
            return string.Format("{0} {1} {2} : {3}", IDPrecioCompetidor, IDAlmacen, IDProducto, Precio);
        }
    }
}
