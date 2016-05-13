using SQLite.Net.Attributes;

namespace SQLitePrecios
{
    public class Almacen
    {
        [PrimaryKey, AutoIncrement]
        public int IDAlmacen { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", IDAlmacen, Nombre, Activo);
        }
    }
}
       