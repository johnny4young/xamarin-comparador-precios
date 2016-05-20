using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SQLitePrecios
{
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();

            //almacen
            connection = new SQLiteConnection(config.Plataforma,
                System.IO.Path.Combine(config.DirectorioDB, "Almacen.db3"));
            connection.CreateTable<Almacen>();
            connection.CreateTable<Producto>(); //producto
            connection.CreateTable<PrecioCompetidor>(); //precioCompetidor                        

        }




        #region Operaciones con ALMACEN

        public void InsertAlmacen(Almacen almacen)
        {
            connection.Insert(almacen);
        }


        public void UpdateAlmacen(Almacen almacen)
        {
            connection.Update(almacen);
        }

        public void DeleteAlmacen(Almacen almacen)
        {
            //eliminamos todos los productos de una almacen en precios
            var almacenes = connection.Table<PrecioCompetidor>().Where(c => c.IDAlmacen == almacen.IDAlmacen).ToList();
            foreach (var item in almacenes)
            {
                connection.Delete(item);
            }            
            //luego procedemos a  borrar
            connection.Delete(almacen);
        }

        public Almacen GetAlmacen(int IDAlmacen)
        {
            return connection.Table<Almacen>().FirstOrDefault(c => c.IDAlmacen == IDAlmacen);
        }

        public List<Almacen> GetAlmacen()
        {
            return connection.Table<Almacen>().OrderBy(c => c.IDAlmacen).ToList();
        }
        #endregion




        #region operaciones con PRODUCTO

        public int InsertProducto(Producto producto)
        {
            connection.Insert(producto);

            return producto.IDProducto;
        }


        /// <summary>
        /// obtenemos todos los productos relacionados a un almacén
        /// </summary>
        /// <param name="IDAlmacen"></param>
        /// <returns></returns>
        public List<DtoProducto> GetProductos(int IDAlmacen)
        {

            var productos = connection.Table<Producto>().ToList();

            var x = connection.Table<PrecioCompetidor>().Where(c => c.IDAlmacen == IDAlmacen).Select(f =>
                new DtoProducto()
                {
                    IDProducto = f.IDProducto,
                    Precio =  f.Precio,
                    Nombre = productos.Find(d => d.IDProducto == f.IDProducto).Nombre
                }).ToList();
            
            return x;
        }


        /// <summary>
        /// obtenemos una consulta las empresas que tiene el mismo producto por NOMBRE
        /// </summary>
        /// <param name="nombreProducto"></param>
        /// <returns></returns>
        public List<DtoProducto> GetComparacion(string nombreProducto)
        {

            //creamos los dto
            var almacenes = connection.Table<Almacen>().ToList();
            var precios   = connection.Table<PrecioCompetidor>().Select(f =>
            new DtoProducto()
            {
                Empresa = almacenes.Find(d => d.IDAlmacen == f.IDAlmacen).Nombre,
                IDProducto =  f.IDProducto,
                Nombre = "",
                Precio = f.Precio
            }).ToList();
                        

            //realizamos el query y ordenados por precios ascendentemente
            var x = connection.Table<Producto>().Where(c => c.Nombre == nombreProducto).Select(f =>
                new DtoProducto()
                {
                    IDProducto = f.IDProducto,
                    Empresa = precios.Find(d => d.IDProducto == f.IDProducto).Empresa,
                    Precio = precios.Find(d => d.IDProducto == f.IDProducto).Precio,
                    Nombre = f.Nombre
                    
                }).OrderBy(o => o.Precio).ToList();

            return x;
        }


        public void InsertPrecioCompetidor(PrecioCompetidor precioCompetidor)
        {            
            connection.Insert(precioCompetidor);
        }


        public void UpdateProducto(DtoProducto producto)
        {
            //obtenemos y actualizamos producto
            var prod = connection.Table<Producto>().FirstOrDefault(c => c.IDProducto == producto.IDProducto);
            prod.Nombre = producto.Nombre;
            connection.Update(prod);

            //obtenemos y actualizamos precio
            var precio = connection.Table<PrecioCompetidor>().FirstOrDefault(c => c.IDProducto == producto.IDProducto);
            precio.Precio = producto.Precio;

            connection.Update(precio);
        }

        public void DeleteProducto(DtoProducto producto)
        {
            //borramos todas las referencia de los productos en precios
            var precio = connection.Table<PrecioCompetidor>().FirstOrDefault(c => c.IDProducto == producto.IDProducto);
            connection.Delete(precio);
            //borramos el producto
            var prod = connection.Table<Producto>().FirstOrDefault(c => c.IDProducto == producto.IDProducto);
            connection.Delete(prod);
        }


        #endregion



        public void resetProductos()
        {
            connection.DeleteAll<PrecioCompetidor>();
            connection.DeleteAll<Producto>();
        }


        public void Dispose()
        {
            connection.Dispose();
        }
    }
}

