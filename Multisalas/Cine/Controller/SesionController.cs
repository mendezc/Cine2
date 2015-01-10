using Cine.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Controller
{
    public class SesionController : ISesionController
    {
        private ISesionService SesionServicio;

        public SesionController(ISesionService sesionService)
        {
            SesionServicio = sesionService;
        }
        public bool EstaAbierta(long id)
        {
            Sesion sesion = SesionServicio.Read(id);
            if (sesion == null)
            {
                Trace.WriteLine("SesionController: .IsOpen() Se intento comprobar si la sesión {0} estaba abierta pero no existe.");
                throw new Exception("La sesión no existe");
            }
            return !sesion.EstaCerrada;
        }
        public void Cerrar(long id)
        {
            SesionServicio.Cerrar(id);
        }

        public void Abrir(long id)
        {
            SesionServicio.Abrir(id);
        }
    }
}