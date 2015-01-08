using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Repository
{
    public class SesionRepository : ISesionRepository
    {

        public Sesion Read(long id)
        {
            using (var context = new SalasDB())
            {
                Sesion res = context.Sesiones.Find(id);
                Trace.WriteLine("Leyendo venta de id" + id);
                return res;
            }
        }

        public Sesion Update(long id, bool cerrada)
        {
            Sesion sesion = null;
            using (var context = new SalasDB())
            {
                sesion = context.Sesiones.Find(id);
                sesion.Cerrado = cerrada;
                context.SaveChanges();
            }
            return sesion;
        }
    }
}