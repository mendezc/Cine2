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
        private List<Venta> listado;
        private long ultimoId;
        private List<Venta> listaDevuelo;
        private Sesion[] Sesiones = new Sesion[9];
       

        public VentaRepository()
        {
            Trace.WriteLine("Instanciado VentaRepository");
            listado = new List<Venta>();
            listaDevuelo = new List<Venta>();
            ultimoId = 0;
            
            Sesiones[0] = new Sesion(1,1,17.5, false);
            Sesiones[1] = new Sesion(2,2,17);
            Sesiones[2] = new Sesion(3,3,17);
            Sesiones[3] = new Sesion(4,1,19.5);
            Sesiones[4] = new Sesion(5,2,19);
            Sesiones[5] = new Sesion(6,3,19.5);
            Sesiones[6] = new Sesion(7,1,22);
            Sesiones[7] = new Sesion(8,2,22);
            Sesiones[8] = new Sesion(9,3,22.5);
            
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
                Venta res = context.Ventas.Find(id);
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
            int indice = listado.FindIndex(v => v.VentaId == venta.VentaId);
            listado[indice] = venta;
            Trace.WriteLine("Actualizando venta de id " + venta.VentaId);
            return listado[indice];
        }

        public Venta Delete(long id)
        {
            int indice = listado.FindIndex(v => v.VentaId == id);
            Venta venta = listado[indice];
            Sesion sesion = venta.Sesion;

            if (!sesion.Cerrado)
            {
                listaDevuelo.Add(listado[indice]);
                listado.RemoveAt(indice);
                Trace.WriteLine("Eliminada venta de id " + id);
                return listaDevuelo.Last();
            }
            else
            {
                Trace.WriteLine("No se ha podido realizar la devolucion, la sesion esta cerrada");
                throw new Exception("No se pudo eliminar, Sesion cerrada");
            }
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
            List<Venta> selecteds = listado.FindAll(
                    venta => venta.Sesion.SalaId == idSala
                    );
            Trace.WriteLine("Calculada total de entradas por sala ");
            return selecteds;
        }

        public int EntradasVendidasTotalSala(int idSala)
        {
            List<Venta> selecteds = listado.FindAll(
                     venta => venta.Sesion.SalaId == idSala
                     );
            int sum = selecteds.Sum(venta => venta.numEntradas);
            Trace.WriteLine("Calculada total de entradas vendidas porsala ");
            return sum;
        }

        /// <summary>
        /// metodo que comrpueba que la sesion existe y que no esta cerrada
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
            foreach (Sesion s in Sesiones)
            {
                if (s.SesionId == sesionID)
                {
                    return s;
                }
            }
            return null;
        }

        public void CambiarCerradoSesion(int idSesion) //metodo para abrir y cerrar sesiones
        {
            foreach (Sesion ses in Sesiones)
            {
                if (ses.SesionId == idSesion)
                {
                    ses.Cerrado = !ses.Cerrado;
                }
            }
        }

        public IList<Venta> EntradasVendidasSesion(int idSesion)
        {
            List<Venta> selecteds = listado.FindAll(
                    venta => venta.Sesion.SesionId == idSesion
                    );
            Trace.WriteLine("Calculada total de entradas por sesion ");
            return selecteds;
        }

        public int EntradasVendidasTotalSesion(int idSesion)
        {
            List<Venta> selecteds = listado.FindAll(
                     venta => venta.Sesion.SesionId == idSesion
                     );
            int sum = selecteds.Sum(venta => venta.numEntradas);
            Trace.WriteLine("Calculada total de entradas vendidas por sesion ");
            return sum;
        }

      
    }
}
