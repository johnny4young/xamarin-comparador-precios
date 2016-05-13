using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLitePrecios
{
    public  class DtoProducto
    {
        public int IDProducto { get; set; }
        public string Nombre  { get; set; }
        public string Empresa { get; set; }
        public Decimal Precio { get; set; }

    }
}
