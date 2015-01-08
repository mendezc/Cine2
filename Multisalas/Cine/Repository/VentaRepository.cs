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
                return context.Ventas.ToList();
                
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
            //int indice = listado.FindIndex(v => v.VentaId == id);
            //Venta venta = listado[indice];
            //Sesion sesion = venta.Sesion;

            //if (!sesion.Cerrado)
            //{
            //    listaDevuelo.Add(listado[indice]);
            //    listado.RemoveAt(indice);
            //    Trace.WriteLine("Eliminada venta de id " + id);
            //    return listaDevuelo.Last();
            //}
            //else
            //{
            //    Trace.WriteLine("No se ha podido realizar la devolucion, la sesion esta cerrada");
            //    throw new Exception("No se pudo eliminar, Sesion cerrada");
            //}
            return null;
        }

        public int ButacasVendidas(long idSesion){
            using(var context = new SalasDB())
            {
                int resultado = context.Database
                    .SqlQuery<int>("select sum(numEntradas) as total from Ventas where Sesion_SesionId = @p0 group by Sesion_SesionId ", new SqlParameter("p0", idSesion))
                    .FirstOrDefault();
                return resultado;
            }
        }

        public IList<Venta> EntradasVendidasSala(int idSala)
        {
            //List<Venta> selecteds = listado.FindAll(
            //        venta => venta.Sesion.SalaId == idSala
            //        );
            //Trace.WriteLine("Calculada total de entradas por sala ");
            //return selecteds;
            return null;
        }

        public int EntradasVendidasTotalSala(int idSala)
        {
            //List<Venta> selecteds = listado.FindAll(
            //         venta => venta.Sesion.SalaId == idSala
            //         );
            //int sum = selecteds.Sum(venta => venta.numEntradas);
            //Trace.WriteLine("Calculada total de entradas vendidas porsala ");
            //return sum;
            return 0;
        }

        /// <summary>
        /// Metodo que comrpueba que la sesion existe y que no esta cerrada
        /// </summary>
        /// <param name="sesionId"></param>
        /// <returns></returns>
        public bool SesionValida(int sesionId)
        {            
            bool valida = false;
            using(var context = new SalasDB())
            {
                Sesion sesion = context.Sesiones.Find(sesionId);
                if (sesion != null && !sesion.Cerrado)
                {
                    valida = true;
                }
            }
            return valida;
        }

        public Sesion BuscaSesion(int sesionID)
        {
            //foreach (Sesion s in Sesiones)
            //{
            //    if (s.SesionId == sesionID)
            //    {
            //        return s;
            //    }
            //}
            return null;
        }

        public IList<Venta> EntradasVendidasSesion(int idSesion)
        {
            //List<Venta> selecteds = listado.FindAll(
            //        venta => venta.Sesion.SesionId == idSesion
            //        );
            //Trace.WriteLine("Calculada total de entradas por sesion ");
            //return selecteds;
            return null;
        }

        public int EntradasVendidasTotalSesion(int idSesion)
        {
            //List<Venta> selecteds = listado.FindAll(
            //         venta => venta.Sesion.SesionId == idSesion
            //         );
            //int sum = selecteds.Sum(venta => venta.numEntradas);
            //Trace.WriteLine("Calculada total de entradas vendidas por sesion ");
            //return sum;
            return 0;
        }

      
    }
}
