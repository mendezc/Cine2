using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Repository
{
    public class VentaRepository : IVentaRepository
    {
        public VentaRepository()
        {
            Trace.WriteLine("Instanciado VentaRepository");
        }

        public Venta Create(Venta venta)
        {
            Sesion sesion = null;
            //listado.Add(venta);
            using (var context = new SalasDB())
            {
                sesion = context.Sesiones.Find(venta.Sesion.SesionId);
                venta.Sesion = sesion;
                context.Ventas.Add(venta);
                context.SaveChanges();
            }
            Trace.WriteLine("Creada nueva venta de id:" + venta.VentaId);
            return venta;
        }

        public Venta Read(long id)
        {
            using (var context = new SalasDB())
            {
                Venta res = context.Ventas.Include("Sesion").Where(v => v.VentaId == id).FirstOrDefault();
                Trace.WriteLine("Leyendo venta de id" + id);
                return res;
            }
            
            //return listado.Find(v => v.Id == id);
        }

        public IList<Venta> List()
        {
            //List<Venta> res = null;
            using (var context = new SalasDB())
            {
                
                Trace.WriteLine("Listando ventas");
                return context.Ventas.Where(v => v.Devolucion == false).ToList();
                
                //return context.Set<Venta>().ToList();
                //return listado;
            }
            
           
        }

        public Venta Update(Venta venta)
        {
            Sesion sesion = null;
            using (var ctx = new SalasDB())
            {
                sesion = ctx.Sesiones.Find(venta.Sesion.SesionId);
                Venta antigua = ctx.Ventas.Find(venta.VentaId);
                if (antigua != null)
                {
                    venta.Sesion = sesion;
                    ctx.Entry(antigua).CurrentValues.SetValues(venta);
                    ctx.SaveChanges();
                    Trace.WriteLine("Actualizando venta de id " + venta.VentaId);
                }
                else
                {
                    throw new VentaException("La venta que intentó actualizar no existe.");
                }
                
                return antigua ;
            }
        }

        public Venta Delete(long id)
        {
            Venta borrado = null;
            using (var context = new SalasDB())
            { 
                borrado = context.Ventas.Find(id);
                if (borrado != null && borrado.Devolucion == false)
                {
                    borrado.Devolucion = true;
                    context.SaveChanges();
                }
            }
            return borrado;
        }

        public int ButacasVendidasSesion(long idSesion){
            using(var context = new SalasDB())
            {
                int resultado = context.Database
                    .SqlQuery<int>("select sum(numEntradas) as total from Ventas where Sesion_SesionId = @p0 group by Sesion_SesionId ", new SqlParameter("p0", idSesion))
                    .FirstOrDefault();
                return resultado;
            }
        }

       

        public int ButacasVendidasSala(int idSala)
        {
            int resultado = 0;
            using (var context = new SalasDB())
            {
                resultado = context.Database
                    .SqlQuery<int>("select sum(Ventas.numEntradas) as total from Ventas, Sesions where Ventas.Sesion_SesionId=Sesions.SesionId and Sesions.SalaId = @p0 group by Sesions.SalaId ", new SqlParameter("p0", idSala))
                    .FirstOrDefault();
            }
            return resultado;
        }

        public int ButacasVendidas()
        {
            int resultado = 0;
            using (var context = new SalasDB())
            {
                resultado = context.Database
                    .SqlQuery<int>("select sum(Ventas.numEntradas) as total from Ventas")
                    .FirstOrDefault();
            }
            return resultado;
        }
        

        public double TotalPrecioSesion(long idSesion)
        {
            using (var context = new SalasDB())
            {
                int resultado = context.Database
                    .SqlQuery<int>("select sum(Precio) as total from Ventas where Sesion_SesionId = @p0 group by Sesion_SesionId ", new SqlParameter("p0", idSesion))
                    .FirstOrDefault();
                return resultado;
            }
        }

        public double TotalPrecioSala(int idSala)
        {
            int resultado = 0;
            using (var context = new SalasDB())
            {
                resultado = context.Database
                    .SqlQuery<int>("select sum(Ventas.Precio) as total from Ventas, Sesions where Ventas.Sesion_SesionId=Sesions.SesionId and Sesions.SalaId = @p0 group by Sesions.SalaId ", new SqlParameter("p0", idSala))
                    .FirstOrDefault();
            }
            return resultado;
        }

        public double TotalPrecio()
        {
            int resultado = 0;
            using (var context = new SalasDB())
            {
                resultado = context.Database
                    .SqlQuery<int>("select sum(Precio) as total from Ventas")
                    .FirstOrDefault();
            }
            return resultado;
        }
    }
}
